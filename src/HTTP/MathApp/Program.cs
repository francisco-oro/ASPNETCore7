using MathApp;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) =>
{
    StreamReader reader = new StreamReader(context.Request.Body);
    string body = await reader.ReadToEndAsync();

    Dictionary<string, StringValues> queryDictionary = QueryHelpers.ParseQuery(body);
    
    if(
    queryDictionary.TryGetValue("firstNumber", out StringValues firstNumber) &&
    queryDictionary.TryGetValue("secondNumber", out StringValues secondNumber) &&
    queryDictionary.TryGetValue("operation", out StringValues operation)
    )
    {
        try
        {
            int num1 = Int32.Parse(firstNumber[0]);
            int num2 = Int32.Parse(secondNumber[0]);
            string op = operation[0];

            context.Response.Headers["Content-Type"] = "text/html";
            await context.Response.WriteAsync(Operation.Parse(num1, num2, op).ToString());
        }
        catch (ArgumentException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync($"Invalid input for 'operation");
        }
        catch (FormatException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync($"Invalid input for 'numbers'");
        }
    }
    else
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync($"Invalid input for 'operation");
        await context.Response.WriteAsync($"Invalid input for 'numbers'");
    }
});

app.Run();