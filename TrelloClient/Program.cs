using ConcordiaLib.Abstract;
using ConcordiaOrchestrator;
using ConcordiaOrchestrator.Options;
using ConcordiaTrelloClient;
using ConcordiaTrelloClient.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scheduler;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IScheduler, SchedulerInstance>();
                services.AddSingleton<Orchestrator>();
                services.AddHttpClient();
                services.AddHttpClient<IApiClient, ApiClient>();
                services.AddSingleton<ApiOptions>();

                services.AddOptions<OrchestratorOptions>()
                        .Bind(context.Configuration.GetSection("OrchestratorOptions"));
            }
       //TODO figure out how to handle scoped services (make a factory)
       /*
       .AddDbContext<ConcordiaDbContext>(
            options => options.EnableSensitiveDataLogging(true)
            .UseSqlServer(@"Server=DESKTOP-617MC8M\SQLEXPRESS;Database=Concordia;Trusted_Connection=True")
            )
       */
       );

using IHost host = builder.Build();
var scheduler = host.Services.GetRequiredService<IScheduler>();

//Test run
await scheduler.TestRun();
Console.WriteLine("Sync done");

//Scheduled run every 30 seconds
//await scheduler.ScheduleAndRun(DateTime.Now, new TimeSpan(0, 0, 30), new TimeSpan(0, 0, 10));
//Console.WriteLine("This line will never be reached");