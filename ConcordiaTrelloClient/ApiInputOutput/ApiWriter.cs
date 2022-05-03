using ConcordiaLib.Collections;
using ConcordiaLib.Domain;
using Microsoft.Extensions.Logging;

namespace ConcordiaTrelloClient.ApiInputOutput;

public class ApiWriter
{
    private readonly HttpClient _httpClient;
    private readonly ApiClient _client;

    public ApiWriter(ApiClient parent)
    {
        _client = parent;
        _httpClient = _client.httpClientFactory.CreateClient("TrelloApi");
    }

    public async Task PutDataToApiAsync(MergingResults merge)
    {
        if (!merge.CardLists.Remote.IsEmpty()) await PutCardListsAsync(merge.CardLists.Remote);
        if (!merge.People.Remote.IsEmpty()) await PutPeopleAsync(merge.People.Remote);
        if (!merge.Cards.Remote.IsEmpty()) await PutCardsAsync(merge.Cards.Remote);
        if (!merge.Comments.Remote.IsEmpty()) await PutCommentsAsync(merge.Comments.Remote);
        if (!merge.Assignments.Remote.IsEmpty()) await PutAssignmentsAsync(merge.Assignments.Remote);
    }

    #region Set methods
#pragma warning disable CS1998 //Suppress Async warning (method should not run anyways)
    private async Task PutCardListsAsync(MergeCUD<CardList> data)
    {
        //Should never happen
        _client.logger.LogError($"Invalid data: {data}");
        throw new InvalidOperationException("Merge error: changes to card lists on remote");
    }

    private async Task PutPeopleAsync(MergeCUD<Person> data)
    {
        //Should never happen
        _client.logger.LogError($"Invalid data: {data}");
        throw new InvalidOperationException("Merge error: changes to people on remote");
    }

    private async Task PutCardsAsync(MergeCUD<Card> data)
    {
        //Create
        if (data.Created.Any())
        {
            //Should never happen
            _client.logger.LogError($"Invalid data: {data.Created}");
            throw new InvalidOperationException($"Merge error: attempted to create card on remote");
        }
        //Update
        foreach (var c in data.Updated)
        {
            var apiUpdateQuery = $"{_client.options.BaseURL}/cards/{c.Id}?idList={c.CardListId}&{_client.options.ApiAuth}";
            var response = await _httpClient.PutAsync(apiUpdateQuery, null);
            if (!response.IsSuccessStatusCode)
            {
                //TODO log
                var test = response.StatusCode;
            }
        }
        //Delete
        if (data.Deleted.Any())
        {
            //Should never happen
            _client.logger.LogError($"Invalid data: {data.Deleted}");
            throw new InvalidOperationException($"Merge error: attempted to delete card on remote");
        }
    }

    private async Task PutCommentsAsync(MergeCUD<Comment> data)
    {
        //Create
        foreach (var c in data.Created)
        {
            var apiCreateQuery = $"{_client.options.BaseURL}/cards/{c.CardId}/actions/comments?text={c.Text}&{_client.options.ApiAuth}";
            var response = await _httpClient.PostAsync(apiCreateQuery, null);
            if (!response.IsSuccessStatusCode)
            {
                _client.logger.LogWarning($"Comment creation failed on remote for commentId {c.Id} with status code {response.StatusCode}");
            }
        }
        //Update
        if (data.Updated.Any())
        {
            //Should never happen
            _client.logger.LogError($"Invalid data: {data.Updated}");
            throw new InvalidOperationException($"Merge error: attempted to update comment on remote");
        }
        //Delete
        foreach (var c in data.Deleted)
        {
            var apiDeleteQuery = $"{_client.options.BaseURL}/actions/{c.Id}?{_client.options.ApiAuth}";
            var response = await _httpClient.DeleteAsync(apiDeleteQuery);
            if (!response.IsSuccessStatusCode)
            {
                _client.logger.LogWarning($"Comment deletion failed on remote for commentId {c.Id} with status code {response.StatusCode}");
            }
        }
    }

    private async Task PutAssignmentsAsync(MergeCUD<Assignment> data)
    {
        //Should never happen
        _client.logger.LogError($"Invalid data: {data}");
        throw new InvalidOperationException($"Merge error: changes to assignments on remote");
    }
#pragma warning restore CS1998
    #endregion

}
