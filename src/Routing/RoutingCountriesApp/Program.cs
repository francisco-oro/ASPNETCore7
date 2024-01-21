using System.Diagnostics.CodeAnalysis;
using RoutingCountriesApp;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("countries", async context =>
    {
        Dictionary<int, string> countries = Countries.GetAll();
        foreach (KeyValuePair<int, string> country in countries)
        {
            await context.Response.WriteAsync($"{country.Key} - {country.Value}\n");
        }
    });

    endpoints.MapGet("countries/{countryID:int:range(1,100)}", async context =>
    {
        int countryId = Convert.ToInt32(context.Request.RouteValues["countryID"]);
        if (countryId > 100)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("The CountryID should be between 1 and 100");
        }

        string? countryName = Countries.GetCountryById(countryId);

        if (countryName != null)
        {
            await context.Response.WriteAsync($"{countryName}");
        }
        else
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("[No country]");
        }


    });
});

app.Run();
