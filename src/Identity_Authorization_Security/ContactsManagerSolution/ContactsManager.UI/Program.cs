using ContactsManager.UI.StartupExtensions;
using CRUDExample.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) => {

    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration) //read configuration settings from built-in IConfiguration
    .ReadFrom.Services(services); //read out current app's services and make them available to serilog
});

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();

//create application pipeline
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

app.UseSerilogRequestLogging();
app.UseHttpLogging();

if (builder.Environment.IsEnvironment("Test") == false)
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

app.UseStaticFiles();

app.UseRouting(); //Identifying action method based route
app.UseAuthentication(); //Reading Identity cookie
app.UseAuthorization(); //Validates access permissions of the user
app.MapControllers(); //Execute the filter pipeline (action + filters)

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default")
});
app.Run();

public partial class Program { } //make the auto-generated Program accessible programmatically