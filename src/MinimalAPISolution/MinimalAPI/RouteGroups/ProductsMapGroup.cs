using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Models;

namespace MinimalAPI.RouteGroups;

public static class ProductsMapGroup
{
    public static RouteGroupBuilder ProductsAPI(this RouteGroupBuilder routeGroupBuilder)
    {
        List<Product> list =
        [
            new Product() { Id = 1, ProductName = "Smart Phone" },
            new Product() { Id = 2, ProductName = "Smart TV " }
        ];

// GET /products 
        routeGroupBuilder.MapGet("/", async (HttpContext context) =>
        {
            // 1, xxxxxxxx
            // 2, xxxxxxxx

            await context.Response.WriteAsync(JsonSerializer.Serialize(list));
        });

// GET /products/{id}
        routeGroupBuilder.MapGet("/{id:int}", async (HttpContext context, int id) =>
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

// POST /products/
        routeGroupBuilder.MapPost("/", async (HttpContext context, Product product) =>
        {
            list.Add(product);
            await context.Response.WriteAsync("Product added");
        });
        
        //PUT /products/{id}

        routeGroupBuilder.MapPut("/{id}", async (HttpContext context, int id, [FromBody] Product product) =>
        {
            Product? productFromCollection = list.FirstOrDefault(temp => temp.Id == id);
            if (productFromCollection == null)
            {
                context.Response.StatusCode = 400; //Bad request 
                await context.Response.WriteAsync("Incorrect product ID");
                return;
            }

            productFromCollection.ProductName = product.ProductName;

            await context.Response.WriteAsync("Product Updated");
        });

        
        //DELETE /products/{id}

        routeGroupBuilder.MapDelete("/{id}", async (HttpContext context, int id) =>
        {
            Product? productFromCollection = list.FirstOrDefault(temp => temp.Id == id);
            if (productFromCollection == null)
            {
                return Results.BadRequest(new { error = "Incorrect Product ID"});
            }

            list.Remove(productFromCollection);
            return Results.Ok(new { message = "Product deleted" });
        });
        return routeGroupBuilder;
        
    }
}