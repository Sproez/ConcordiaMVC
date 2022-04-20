using ConcordiaTrelloClient.Mapping;

namespace ConcordiaTrelloClient;

using AutoMapper;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Dto;
using Dto.NestedProperties;
using ConcordiaLib.Domain;
using ConcordiaLib.Abstract;
using ConcordiaLib.Collections;
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
        //Options config
        _options = options;
        BoardEndpoint = $"{_options.BaseURL}/boards/{_options.ConcordiaBoardID}";
        ApiAuth = $"key={_options.ApiKey}&token={_options.ApiToken}";

        //Automapper setup
        var pResolver = new PriorityResolver(_options);

        var automapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CardDto, Card>()
                .ForCtorParam("Priority",
                opt => opt.MapFrom(src => pResolver.Resolve(src.LabelIds)));
            cfg.CreateMap<CardListDto, CardList>();
            cfg.CreateMap<PersonDto, Person>();
            cfg.CreateMap<CommentDto, Comment>();
        });

        var mapper = automapperConfig.CreateMapper();
        _mapper = mapper;

        //HTTP client setup and config
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Add("User-Agent", "Concordia Trello Client");
    }

    public async Task<DatabaseImage> GetDataFromApiAsync()
    {

        var apiListsQuery = $"{BoardEndpoint}/lists?{ApiAuth}";
        var apiCardsQuery = $"{BoardEndpoint}/cards?{ApiAuth}";
        var apiPeopleQuery = $"{BoardEndpoint}/members?{ApiAuth}";
        var apiCommentsQuery = $"{BoardEndpoint}/actions?filter=commentCard&fields=id,data,date,idMemberCreator&{ApiAuth}";
        var apiAssignmentsQuery = $"{BoardEndpoint}/cards?fields=id,idMembers&{ApiAuth}";

        //List<CardList> cardLists = await GetListsAsync();
        //List<Card> cards = await GetCardsAsync();
        //List<Person> people = await GetPeopleAsync();
        //List<Comment> comments = await GetCommentsAsync();
        //List<Assignment> assignments = await GetAssignmentsAsync();

        var listTask = GetThingsAsync<CardList, CardListDto>(apiListsQuery);
        var cardTask = GetThingsAsync<Card, CardDto>(apiCardsQuery);
        var personTask = GetThingsAsync<Person, PersonDto>(apiPeopleQuery);
        var commentTask = GetThingsAsync<Comment, CommentDto>(apiCommentsQuery);
        var assignmentTask = GetAssignmentsAsync(apiAssignmentsQuery);

        var listTasks = new List<Task> { listTask, cardTask, personTask, commentTask, assignmentTask };

        var t = Task.WhenAll(listTasks);
        await t;

        return new DatabaseImage(cardTask.Result, personTask.Result, commentTask.Result, assignmentTask.Result, listTask.Result);
    }

    //private async Task<List<CardList>> GetListsAsync()
    //{
    //    var apiQuery = $"{BoardEndpoint}/lists?{ApiAuth}";
    //    return await GetThingsAsync<CardList, CardListDto>(apiQuery);
    //}

    //private async Task<List<Card>> GetCardsAsync()
    //{
    //    var apiQuery = $"{BoardEndpoint}/cards?{ApiAuth}";
    //    return await GetThingsAsync<Card, CardDto>(apiQuery);
    //}

    //private async Task<List<Person>> GetPeopleAsync()
    //{
    //    var apiQuery = $"{BoardEndpoint}/members?{ApiAuth}";
    //    return await GetThingsAsync<Person, PersonDto>(apiQuery);
    //}

    #region Get methods
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

    private async Task<List<Comment>> GetCommentsAsync(string query)
    {
        var task = _client.GetStreamAsync(query);
        var dtos = await JsonSerializer.DeserializeAsync<List<CommentDto>>(await task) ?? new List<CommentDto>();
        var result = new List<Comment>();
        var temp = new List<Comment>();
        foreach (var dto in dtos)
        {
            temp.Add(_mapper.Map<CommentDto, Comment>(dto));
        }

        //Only keep the most recent comment for each card
        ILookup<string, Comment> test = temp.ToLookup(c => c.CardId, c => c);
        foreach (var commentsInCard in test)
        {
            Comment newestComment = commentsInCard.OrderByDescending(c => c.CreatedAt).First();
            //commentsInCard.Aggregate((Comment newest, Comment next) => next.CreatedAt > newest.CreatedAt ? next : newest);
            result.Add(newestComment);
        }

        return result;
    }

    private async Task<List<Assignment>> GetAssignmentsAsync(string query)
    {
        var task = _client.GetStreamAsync(query);
        var cards = await JsonSerializer.DeserializeAsync<List<NestedCard>>(await task) ?? new List<NestedCard>();
        var result = new List<Assignment>();
        foreach (var card in cards)
        {
            foreach (var assigneeId in card.AssigneesIds)
            {
                var assignment = new Assignment()
                {
                    CardId = card.Id,
                    PersonId = assigneeId
                };
                result.Add(assignment);
            }
        }

        return result;
    }
    #endregion

    public async Task PutDataToApiAsync(MergingResults merge)
    {
        await PutCardListsAsync(merge.CardLists.Remote);
        //await PutCommentsAsync();
    }

    #region Set methods
    private async Task PutCardListsAsync(MergeCUD<CardList> data)
    {
        //Create
        foreach (var c in data.Created)
        {
            //TODO
        }
        //Update
        foreach (var c in data.Updated)
        {
            var apiUpdateQuery = $"{_options.BaseURL}/lists/{c.Id}?name={c.Name}&{ApiAuth}";
            var response = await _client.PutAsync(apiUpdateQuery, null);
            if (!response.IsSuccessStatusCode)
            {
                //TODO log
                var test = response.StatusCode;
            }
        }
        //Delete
        foreach (var c in data.Deleted)
        {
            var apiDeleteQuery = $"{_options.BaseURL}/lists/{c.Id}/closed?{ApiAuth}";
            var response = await _client.PostAsync(apiDeleteQuery, null);
            if (!response.IsSuccessStatusCode)
            {
                //TODO log
                var test = response.StatusCode;
            }
        }
    }

    private async Task PutCommentsAsync()
    {

        var cardId = "62503467d0ca8631655c48f3";
        //var commentId = "62559a3ff3e3238ceaecb06d";
        var newText = "Test 2: the return";

        var apiCreateQuery = $"{_options.BaseURL}/cards/{cardId}/actions/comments?text={newText}&{ApiAuth}";
        //
        //var apiUpdateQuery = $"{_options.BaseURL}/cards/{cardId}/actions/{commentId}/comments?text={newText}&{ApiAuth}";
        var task = _client.PostAsync(apiCreateQuery, null);
        var test = await task;

        var two = 1 + 1;
    }
    #endregion

}


