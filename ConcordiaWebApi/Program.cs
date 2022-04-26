using ConcordiaLib.Abstract;
using ConcordiaSqlDatabase.Data;
using ConcordiaWebApi.Options;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Options
builder.Services
    .AddOptions<WebApiOptions>()
    .Bind(builder.Configuration.GetSection("WebApiOptions"))
    .ValidateDataAnnotations();

//Db config
builder.Services.AddScoped<IDbMiddleware, SQLDbMiddleware>();
builder.Services.AddDbContext<ConcordiaDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
