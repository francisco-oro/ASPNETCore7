using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Product> list = new List<Product>()
{
    new Product() { Id = 1, ProductName = "Smart Phone"},
    new Product() { Id = 2, ProductName = "Smart TV "}
};

var mapGroup = app.MapGroup("/products");
// GET /products 
mapGroup.MapGet("/products", async (HttpContext context) =>
{
    // 1, xxxxxxxx
    // 2, xxxxxxxx

    await context.Response.WriteAsync(JsonSerializer.Serialize(list));
});

// GET /products/{id}
mapGroup.MapGet("/products/{id:int}", async (HttpContext context, int id) =>
{
    Product? product = list.FirstOrDefault(temp => temp.Id == id);
    if (product == null)
    {
        context.Response.StatusCode = 400; //Bad request 
        await context.Response.WriteAsync("Incorrect product ID");
        return;
    }
    await context.Response.WriteAsync(JsonSerializer.Serialize(product));
}); 

mapGroup.MapPost("/products", async (HttpContext context, Product product) =>
{
    list.Add(product); 
    await context.Response.WriteAsync("Product added");
});

app.Run();