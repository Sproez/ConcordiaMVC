using ConcordiaLib.Abstract;
using ConcordiaLib.Utils;
using ConcordiaMVC.Options;
using ConcordiaSqlDatabase.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Options
builder.Services
    .AddOptions<MyMvcOptions>()
    .Bind(builder.Configuration.GetSection("MyMvcOptions"))
    .ValidateDataAnnotations();

//Card sorter config
builder.Services.AddSingleton<CardComparer>(p =>
{
    var completedListId = p.GetRequiredService<IOptions<MyMvcOptions>>().Value.CompletedListId;
    return new CardComparer(completedListId);
}
);

//Db config
builder.Services.AddScoped<IDbMiddleware, SQLDbMiddleware>();
builder.Services.AddDbContext<ConcordiaDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
