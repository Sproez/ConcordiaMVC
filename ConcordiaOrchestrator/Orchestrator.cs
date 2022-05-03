using ConcordiaLib.Abstract;
using ConcordiaLib.Collections;
using ConcordiaMerger;
using ConcordiaOrchestrator.Options;
using ConcordiaPDFGenerator;
using ConcordiaSqlDatabase.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ConcordiaOrchestrator;

public class Orchestrator
{
    private readonly IApiClient _client;
    private readonly OrchestratorOptions _options;

    public Orchestrator(IApiClient client, IOptions<OrchestratorOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task Sync()
    {
        //SQL Client setup
        var dbOptions = new DbContextOptionsBuilder<ConcordiaDbContext>()
            .EnableSensitiveDataLogging(true)
            .UseSqlServer(_options.DefaultDatabase)
            .Options;

        //Get data from API
        DatabaseImage apiData = await _client.GetDataFromApiAsync();

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
        await _client.PutDataToApiAsync(result);


        //Create PDF report

        using (var dbContext = new ConcordiaDbContext(dbOptions))
        {
            var db = new SQLDbMiddleware(dbContext);

            var pdfGenerator = new PdfGenerator(db, _options.CompletedListId);
            await pdfGenerator.Test();
        }
    }

}