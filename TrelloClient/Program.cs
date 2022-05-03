using System.Net.Http.Headers;
using ConcordiaLib.Abstract;
using ConcordiaOrchestrator;
using ConcordiaOrchestrator.Options;
using ConcordiaPDFGenerator;
using ConcordiaSqlDatabase.Data;
using ConcordiaTrelloClient;
using ConcordiaTrelloClient.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scheduler;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IScheduler, SchedulerInstance>();
                services.AddSingleton<Orchestrator>();
                services.AddHttpClient("TrelloApi", client =>
                 {
                     client.DefaultRequestHeaders.Accept.Clear();
                     client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                     client.DefaultRequestHeaders.Add("User-Agent", "Concordia Trello Client");
                 }
                );
                services.AddSingleton<IApiClient, ApiClient>();
                services.AddSingleton<ApiOptions>();

                services.AddOptions<OrchestratorOptions>()
                        .Bind(context.Configuration.GetSection("OrchestratorOptions"));

                services.AddDbContextFactory<ConcordiaDbContext>(
                    options =>
                    options.UseSqlServer(context.Configuration.GetSection("OrchestratorOptions")["DefaultDatabase"])
                );
            }
       );

using IHost host = builder.Build();
var scheduler = host.Services.GetRequiredService<IScheduler>();

//Test run
await scheduler.TestRun();
Console.WriteLine("Sync done");

//Scheduled run every 30 seconds
//await scheduler.ScheduleAndRun(DateTime.Now, new TimeSpan(0, 0, 30), new TimeSpan(0, 0, 10));
//Console.WriteLine("This line will never be reached");