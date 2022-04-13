namespace ConcordiaLib.Abstract;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

public interface IApiClient
{
    Task<DatabaseImage> GetDataFromApiAsync();
}

