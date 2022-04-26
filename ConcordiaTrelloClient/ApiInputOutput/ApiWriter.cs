using ConcordiaLib.Collections;
using ConcordiaLib.Domain;

namespace ConcordiaTrelloClient.ApiInputOutput;

public class ApiWriter
{
    private readonly ApiClient _client;

    public ApiWriter(ApiClient parent)
    {
        _client = parent;
    }

    public async Task PutDataToApiAsync(MergingResults merge)
    {
        await PutCardListsAsync(merge.CardLists.Remote);
        await PutPeopleAsync(merge.People.Remote);
        await PutCardsAsync(merge.Cards.Remote);
        await PutCommentsAsync(merge.Comments.Remote);
        await PutAssignmentsAsync(merge.Assignments.Remote);
    }

    #region Set methods
    private async Task PutCardListsAsync(MergeCUD<CardList> data)
    {
        //Create
        foreach (var c in data.Created)
        {
            var apiCreateQuery = $"{_client.options.BaseURL}/lists?name={c.Name}&idBoard={_client.options.ConcordiaBoardID}&{_client.options.ApiAuth}";
            var response = await _client.httpClient.PostAsync(apiCreateQuery, null);
            if (!response.IsSuccessStatusCode)
            {
                //TODO log
                var test = response.StatusCode;
            }
        }
        //Update
        foreach (var c in data.Updated)
        {
            var apiUpdateQuery = $"{_client.options.BaseURL}/lists/{c.Id}?name={c.Name}&{_client.options.ApiAuth}";
            var response = await _client.httpClient.PutAsync(apiUpdateQuery, null);
            if (!response.IsSuccessStatusCode)
            {
                //TODO log
                var test = response.StatusCode;
            }
        }
        //Delete
        foreach (var c in data.Deleted)
        {
            var apiDeleteQuery = $"{_client.options.BaseURL}/lists/{c.Id}/closed?{_client.options.ApiAuth}";
            var response = await _client.httpClient.PostAsync(apiDeleteQuery, null);
            if (!response.IsSuccessStatusCode)
            {
                //TODO log
                var test = response.StatusCode;
            }
        }
    }

    private async Task PutPeopleAsync(MergeCUD<Person> data)
    {
        //Create
        foreach (var p in data.Created)
        {
            //Should not happen
            //TODO log
            Console.WriteLine("WARNING: " + p);
        }
        //Update
        foreach (var p in data.Updated)
        {
            //Should not happen
            //TODO log
            Console.WriteLine("WARNING: " + p);
        }
        //Delete
        foreach (var p in data.Deleted)
        {
            //Should not happen
            //TODO log
            Console.WriteLine("WARNING: " + p);
        }
    }

    private async Task PutCardsAsync(MergeCUD<Card> data)
    {
        //Create
        foreach (var c in data.Created)
        {
            //Should not happen
            //TODO log
            Console.WriteLine("WARNING: " + c);
        }
        //Update
        foreach (var c in data.Updated)
        {
            var apiUpdateQuery = $"{_client.options.BaseURL}/cards/{c.Id}?idList={c.CardListId}&{_client.options.ApiAuth}";
            var response = await _client.httpClient.PutAsync(apiUpdateQuery, null);
            if (!response.IsSuccessStatusCode)
            {
                //TODO log
                var test = response.StatusCode;
            }
        }
        //Delete
        foreach (var c in data.Deleted)
        {
            //Should not happen
            //TODO log
            Console.WriteLine("WARNING: " + c);
        }
    }

    private async Task PutCommentsAsync(MergeCUD<Comment> data)
    {
        //Create
        foreach (var c in data.Created)
        {
            var apiCreateQuery = $"{_client.options.BaseURL}/cards/{c.CardId}/actions/comments?text={c.Text}&{_client.options.ApiAuth}";
            var response = await _client.httpClient.PostAsync(apiCreateQuery, null);
            if (!response.IsSuccessStatusCode)
            {
                //TODO log
                var test = response.StatusCode;
            }
        }
        //Update
        foreach (var c in data.Updated)
        {
            //Should not happen
            //TODO log
            Console.WriteLine("WARNING: " + c);
        }
        //Delete
        foreach (var c in data.Deleted)
        {
            var apiDeleteQuery = $"{_client.options.BaseURL}/actions/{c.Id}?{_client.options.ApiAuth}";
            var response = await _client.httpClient.DeleteAsync(apiDeleteQuery);
            if (!response.IsSuccessStatusCode)
            {
                //TODO log
                var test = response.StatusCode;
            }
        }
    }

    private async Task PutAssignmentsAsync(MergeCUD<Assignment> data)
    {
        //Create
        foreach (var a in data.Created)
        {
            //Should not happen
            //TODO log
            Console.WriteLine("WARNING: " + a);
        }
        //Update
        foreach (var a in data.Updated)
        {
            //Should not happen
            //TODO log
            Console.WriteLine("WARNING: " + a);
        }
        //Delete
        foreach (var a in data.Deleted)
        {
            //Should not happen
            //TODO log
            Console.WriteLine("WARNING: " + a);
        }
    }
    #endregion

}
