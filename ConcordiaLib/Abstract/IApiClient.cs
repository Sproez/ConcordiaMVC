namespace ConcordiaLib.Abstract;

using System.Threading.Tasks;
using Collections;

public interface IApiClient
{
    Task<DatabaseImage> GetDataFromApiAsync();

    Task PutDataToApiAsync(MergingResults merge);
}

