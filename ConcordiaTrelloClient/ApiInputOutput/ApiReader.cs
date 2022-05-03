namespace ConcordiaTrelloClient.ApiInputOutput;

using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Dto;
using Dto.NestedProperties;
using ConcordiaLib.Domain;
using ConcordiaLib.Collections;

public class ApiReader
{
    private readonly HttpClient _httpClient;
    private readonly ApiClient _client;

    public ApiReader(ApiClient parent)
    {
        _client = parent;
        _httpClient = _client.httpClientFactory.CreateClient("TrelloApi");
    }

    public async Task<DatabaseImage> GetDataFromApiAsync()
    {
        var apiListsQuery = $"{_client.BoardEndpoint}/lists?{_client.options.ApiAuth}";
        var apiCardsQuery = $"{_client.BoardEndpoint}/cards?{_client.options.ApiAuth}";
        var apiPeopleQuery = $"{_client.BoardEndpoint}/members?{_client.options.ApiAuth}";
        var apiCommentsQuery = $"{_client.BoardEndpoint}/actions?filter=commentCard&fields=id,data,date,idMemberCreator&{_client.options.ApiAuth}";
        var apiAssignmentsQuery = $"{_client.BoardEndpoint}/cards?fields=id,idMembers&{_client.options.ApiAuth}";

        //Commented for testing

        var listTask = GetThingsAsync<CardList, CardListDto>(apiListsQuery);
        var cardTask = GetThingsAsync<Card, CardDto>(apiCardsQuery);
        var personTask = GetThingsAsync<Person, PersonDto>(apiPeopleQuery);
        var commentTask = GetThingsAsync<Comment, CommentDto>(apiCommentsQuery);
        var assignmentTask = GetAssignmentsAsync(apiAssignmentsQuery);

        var listTasks = new List<Task> { listTask, cardTask, personTask, commentTask, assignmentTask };

        var t = Task.WhenAll(listTasks);

        await t;

        return new DatabaseImage(cardTask.Result, personTask.Result, commentTask.Result, assignmentTask.Result, listTask.Result);

        /*
        //Testing code
        var listR = await GetThingsAsync<CardList, CardListDto>(apiListsQuery);
        var cardR = await GetThingsAsync<Card, CardDto>(apiCardsQuery);
        var personR = await GetThingsAsync<Person, PersonDto>(apiPeopleQuery);
        var commentR = await GetThingsAsync<Comment, CommentDto>(apiCommentsQuery);
        var assignmentR = await GetAssignmentsAsync(apiAssignmentsQuery);

        return new DatabaseImage(cardR, personR, commentR, assignmentR, listR);
        */
    }

    #region Get methods

    private async Task<List<Tresult>> GetThingsAsync<Tresult, Tdto>(string ApiQuery)
    {

        var result = new List<Tresult>();
        var task = _httpClient.GetStreamAsync(ApiQuery);

        var jsonResult = await task;

        var dtos = await JsonSerializer.DeserializeAsync<List<Tdto>>(jsonResult) ?? new List<Tdto>();
        foreach (var dto in dtos)
        {
            result.Add(_client.mapper.Map<Tdto, Tresult>(dto));
        }

        return result;
    }



    private async Task<List<Assignment>> GetAssignmentsAsync(string query)
    {
        var task = _httpClient.GetStreamAsync(query);
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
}

