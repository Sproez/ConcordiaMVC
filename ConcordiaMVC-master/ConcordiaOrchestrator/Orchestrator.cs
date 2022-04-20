using ConcordiaLib.Abstract;
using ConcordiaLib.Collections;
using ConcordiaMerger;
using ConcordiaSqlDatabase.Data;
using ConcordiaTrelloClient;
using ConcordiaTrelloClient.Options;
using Microsoft.EntityFrameworkCore;

namespace ConcordiaOrchestrator;

public static class Orchestrator
{

    public static async Task Sync()
    {
        //API Client setup
        var options = new ApiOptions();
        IApiClient trelloClient = new ApiClient(options);

        //SQL Client setup
        var dbOptions = new DbContextOptionsBuilder<ConcordiaDbContext>()
            .EnableSensitiveDataLogging(true)
            .UseSqlServer(@"Server=DESKTOP-617MC8M\SQLEXPRESS;Database=Concordia;Trusted_Connection=True") //Use SINGLE slashes ONLY!!!
            .Options;

        //Get data from API
        DatabaseImage apiData = await trelloClient.GetDataFromApiAsync();
        //Get data from DB
        DatabaseImage dbData;

        using (var dbContext = new ConcordiaDbContext(dbOptions))
        {
            var db = new SQLDbMiddleware(dbContext);

            dbData = await db.GetDatabaseData();
        }

        //Merge
        var merger = new Merger(dbData, apiData);
        var result = merger.Merge();

        //Write data to DB
        
        using (var dbContext = new ConcordiaDbContext(dbOptions))
        {
            var db = new SQLDbMiddleware(dbContext);

            await db.UpdateData(result);
        }

        //Put data to API
        await trelloClient.PutDataToApiAsync(result);

    }

}