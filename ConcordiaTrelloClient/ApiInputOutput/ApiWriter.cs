using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var apiUpdateQuery = $"{_client.options.BaseURL}/lists/{c.Id}?name={c.Name}&{_client.ApiAuth}";
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
            var apiDeleteQuery = $"{_client.options.BaseURL}/lists/{c.Id}/closed?{_client.ApiAuth}";
            var response = await _client.httpClient.PostAsync(apiDeleteQuery, null);
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
            var apiDeleteQuery = $"{_client.options.BaseURL}/actions/{c.Id}?{_client.ApiAuth}";
            var response = await _client.httpClient.DeleteAsync(apiDeleteQuery);
            if (!response.IsSuccessStatusCode)
            {
                //TODO log
                var test = response.StatusCode;
            }
        }
    }
    #endregion

}
