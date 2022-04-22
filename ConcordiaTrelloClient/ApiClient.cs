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
    public readonly ApiOptions options;
    public readonly IMapper mapper;
    public readonly HttpClient httpClient;

    public readonly string BoardEndpoint;
    public readonly string ApiAuth;

    public ApiClient(ApiOptions options)
    {
        //Options config
        this.options = options;
        BoardEndpoint = $"{this.options.BaseURL}/boards/{this.options.ConcordiaBoardID}";
        ApiAuth = $"key={this.options.ApiKey}&token={this.options.ApiToken}";

        //Automapper setup
        var pResolver = new PriorityResolver(this.options);

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
        this.mapper = mapper;

        //HTTP client setup and config
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Concordia Trello Client");
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

    #region Get methods
    private async Task<List<Tresult>> GetThingsAsync<Tresult, Tdto>(string ApiQuery)
    {
        var result = new List<Tresult>();
        var task = httpClient.GetStreamAsync(ApiQuery);
        var dtos = await JsonSerializer.DeserializeAsync<List<Tdto>>(await task) ?? new List<Tdto>();
        foreach (var dto in dtos)
        {
            result.Add(mapper.Map<Tdto, Tresult>(dto));
        }
        return result;
    }

    private async Task<List<Assignment>> GetAssignmentsAsync(string query)
    {
        var task = httpClient.GetStreamAsync(query);
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
        await PutCommentsAsync(merge.Comments.Remote);
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
            var apiUpdateQuery = $"{options.BaseURL}/lists/{c.Id}?name={c.Name}&{ApiAuth}";
            var response = await httpClient.PutAsync(apiUpdateQuery, null);
            if (!response.IsSuccessStatusCode)
            {
                //TODO log
                var test = response.StatusCode;
            }
        }
        //Delete
        foreach (var c in data.Deleted)
        {
            var apiDeleteQuery = $"{options.BaseURL}/lists/{c.Id}/closed?{ApiAuth}";
            var response = await httpClient.PostAsync(apiDeleteQuery, null);
            if (!response.IsSuccessStatusCode)
            {
                //TODO log
                var test = response.StatusCode;
            }
        }
    }

    private async Task PutCommentsAsync(MergeCUD<Comment> data)
    {
        //Create
        foreach (var c in data.Created)
        {
            //TODO
        }
        //Update
        foreach (var c in data.Updated)
        {
            //TODO
        }
        //Delete
        foreach (var c in data.Deleted)
        {
            var apiDeleteQuery = $"{options.BaseURL}/actions/{c.Id}?{ApiAuth}";
            var response = await httpClient.DeleteAsync(apiDeleteQuery);
            if (!response.IsSuccessStatusCode)
            {
                //TODO log
                var test = response.StatusCode;
            }
        }
    }
    #endregion

}


