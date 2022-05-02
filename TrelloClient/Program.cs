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
                services.AddSingleton<IApiClient, ApiClient>();
                services.AddSingleton<ApiOptions>();

                services.AddOptions<OrchestratorOptions>()
                        .Bind(context.Configuration.GetSection("OrchestratorOptions"));
            }
       //TODO figure out how to handle scoped services
       /*
       .AddDbContext<ConcordiaDbContext>(
            options => options.EnableSensitiveDataLogging(true)
            .UseSqlServer(@"Server=DESKTOP-617MC8M\SQLEXPRESS;Database=Concordia;Trusted_Connection=True")
            )
       */
       );

using IHost host = builder.Build();

//Test run

var scheduler = host.Services.GetRequiredService<IScheduler>();
//await scheduler.TestRun();
await scheduler.ScheduleAndRun(DateTime.Now, new TimeSpan(30));
Console.WriteLine("Sync done");

//