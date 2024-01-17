using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) =>
{
    StreamReader reader = new StreamReader(context.Request.Body);
    string body = await reader.ReadToEndAsync();

    Dictionary<string, StringValues> queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);

    if (queryDictionary.ContainsKey("firstName"))
    {
        foreach (string value in queryDictionary["firstName"])
        {
            await context.Response.WriteAsync(value);   
        }
    }
});

app.Run();