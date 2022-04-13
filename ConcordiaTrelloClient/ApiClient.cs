namespace ConcordiaTrelloClient;

using AutoMapper;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Dto;
using Dto.Fake;
using ConcordiaLib.Domain;
using ConcordiaLib.Abstract;
using Options;

public class ApiClient : IApiClient
{
    private readonly ApiOptions _options;
    private readonly IMapper _mapper;
    private readonly HttpClient _client;

    private readonly string BoardEndpoint;
    private readonly string ApiAuth;

    public ApiClient(ApiOptions options)
    {
        //Automapper setup
        var automapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CardDto, Card>();
            cfg.CreateMap<CardListDto, CardList>();
            cfg.CreateMap<PersonDto, Person>();
            cfg.CreateMap<CommentDto, Comment>();
        });
        var mapper = automapperConfig.CreateMapper();
        _mapper = mapper;

        //Options config
        _options = options;
        BoardEndpoint = $"{_options.BaseURL}/boards/{_options.ConcordiaBoardID}";
        ApiAuth = $"key={_options.ApiKey}&token={_options.ApiToken}";
        //HTTP client setup and config
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Add("User-Agent", "Concordia Trello Client");
    }

    public async Task<DatabaseImage> GetDataFromApiAsync()
    {


        List<CardList> cardLists = await GetListsAsync();
        List<Card> cards = await GetCardsAsync();
        List<Person> people = await GetPeopleAsync();
        List<Comment> comments = await GetCommentsAsync();
        List<Assignment> assignments = await GetAssignmentsAsync();

        return new DatabaseImage(cards, people, comments, assignments, cardLists);
    }

    private async Task<List<CardList>> GetListsAsync()
    {
        var apiQuery = $"{BoardEndpoint}/lists?{ApiAuth}";
        return await GetThingsAsync<CardList, CardListDto>(apiQuery);
    }

    private async Task<List<Card>> GetCardsAsync()
    {
        var apiQuery = $"{BoardEndpoint}/cards?{ApiAuth}";
        return await GetThingsAsync<Card, CardDto>(apiQuery);
    }

    private async Task<List<Person>> GetPeopleAsync()
    {
        var apiQuery = $"{BoardEndpoint}/members?{ApiAuth}";
        return await GetThingsAsync<Person, PersonDto>(apiQuery);
    }

    private async Task<List<Tresult>> GetThingsAsync<Tresult, Tdto>(string ApiQuery)
    {
        var result = new List<Tresult>();
        var task = _client.GetStreamAsync(ApiQuery);
        var dtos = await JsonSerializer.DeserializeAsync<List<Tdto>>(await task) ?? new List<Tdto>();
        foreach (var dto in dtos)
        {
            result.Add(_mapper.Map<Tdto, Tresult>(dto));
        }
        return result;
    }

    private async Task<List<Comment>> GetCommentsAsync()
    {
        var task = _client.GetStreamAsync($"{BoardEndpoint}/actions?filter=commentCard&fields=id,data,date,idMemberCreator&{ApiAuth}");
        var fakes = await JsonSerializer.DeserializeAsync<List<FakeComment>>(await task) ?? new List<FakeComment>();
        var result = new List<Comment>();
        var temp = new List<Comment>();
        foreach (var fake in fakes)
        {
            var dto = new CommentDto(fake);
            temp.Add(_mapper.Map<CommentDto, Comment>(dto));
        }

        //Only keep the most recent comment for each card
        ILookup<string, Comment> test = temp.ToLookup(c => c.CardId, c => c);

        foreach (var commentsInCard in test)
        {
            Comment newestComment = commentsInCard.Aggregate((Comment newest, Comment next) => next.CreatedAt > newest.CreatedAt ? next : newest);
            result.Add(newestComment);
        }

        return result;
    }

    private async Task<List<Assignment>> GetAssignmentsAsync()
    {
        var task = _client.GetStreamAsync($"{BoardEndpoint}/cards?fields=id,idMembers&{ApiAuth}");
        var fakes = await JsonSerializer.DeserializeAsync<List<FakeCard>>(await task) ?? new List<FakeCard>();
        var result = new List<Assignment>();
        foreach (var fake in fakes)
        {
            foreach (var assigneeId in fake.AssigneesIds)
            {
                var assignment = new Assignment()
                {
                    CardId = fake.Id,
                    PersonId = assigneeId
                };
                result.Add(assignment);
            }
        }

        return result;
    }

}


