using ConcordiaLib.Abstract;
using ConcordiaMVC.Options;
using ConcordiaSqlDatabase.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Options
builder.Services
    .AddOptions<MyMvcOptions>()
    .Bind(builder.Configuration.GetSection("MyMvcOptions"))
    .ValidateDataAnnotations();

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
