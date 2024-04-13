using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using ServiceContracts;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Rotativa.AspNetCore;
using Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(
    (HostBuilderContext context, IServiceProvider Services, LoggerConfiguration loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration) // Read configuration settings from built-in IConfiguration
            .ReadFrom.Services(Services); // Read out current app services and make them available to serilog
    });

// it adds controllers and views as services
builder.Services.AddControllersWithViews(options =>
{
    //options.Filters.Add<ResponseHeaderActionFilter>();
    options.Filters.Add(new ResponseHeaderActionFilter(null, "My-Key-From-Global", "My-Value-From-Global"))
});

//add services into IoC container
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPeopleService, PeopleService>();
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields =
        HttpLoggingFields.RequestProperties |
        HttpLoggingFields.ResponsePropertiesAndHeaders;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseSerilogRequestLogging();

// Crete application pipeline
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpLogging();

//app.Logger.LogDebug("debug-message");
//app.Logger.LogInformation("information-message");
//app.Logger.LogWarning("warning-message");
//app.Logger.LogError("error-message");
//app.Logger.LogCritical("critical-message");

if (builder.Environment.IsEnvironment("Test") == false)
{
    RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program //make the auto-generated Program accessible programatically 
{

}