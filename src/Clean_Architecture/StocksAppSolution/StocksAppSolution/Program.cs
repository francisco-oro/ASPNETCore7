using Rotativa.AspNetCore;
using StocksApp.UI.MiddleWare;
using StocksApp.UI.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(configuration: builder.Configuration);

var app = builder.Build();

if (builder.Environment.IsEnvironment("Test").Equals(false))
{
    RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseExceptionHandlingMiddleware();
}



app.UseHttpLogging();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();

// make the auto-generated Program accessible programmatically 
public partial class Program
{

}