namespace ConcordiaLib.Abstract;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Collections;

public interface IApiClient
{
    Task<DatabaseImage> GetDataFromApiAsync();

    Task PutDataToApiAsync(MergingResults merge);
}

