using System.Reflection;
using MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Product> list = new List<Product>()
{
    new Product() { Id = 1, ProductName = "Smart Phone"},
    new Product() { Id = 2, ProductName = "Smart TV "}
};
// GET /products 
app.MapGet("/products", async (HttpContext context) =>
{
    var content = string.Join("\n", list.Select(temp => temp.ToString()));
    // 1, xxxxxxxx
    // 2, xxxxxxxx

    await context.Response.WriteAsync(content);
});

app.MapPost("/", async (HttpContext context, Product product) =>
{
    list.Add(product); 
    await context.Response.WriteAsync("Product added");
});

app.Run();