using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.EndpointFilters;
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

            return Results.Ok(JsonSerializer.Serialize(list));
        });

// GET /products/{id}
        routeGroupBuilder.MapGet("/{id:int}", async (HttpContext context, int id) =>
        {
            Product? product = list.FirstOrDefault(temp => temp.Id == id);
            if (product == null)
            {
                return Results.BadRequest("Incorrect product ID");
            }

            return Results.Ok(JsonSerializer.Serialize(product));
        });


// POST /products/
        routeGroupBuilder.MapPost("/", async (HttpContext context, Product product) =>
        {
            list.Add(product);
            return Results.NoContent();
        })        
            .AddEndpointFilter<CustomEndpointFilter>()
            .AddEndpointFilter(async (context, @delegate) =>
        {
            Product? product = context.Arguments.OfType<Product>().FirstOrDefault();

            if (product == null)
            {
                return Results.BadRequest("Product details are not found in the request");
            }

            var validationContext = new ValidationContext(product);
            List<ValidationResult> errors = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(product, validationContext, errors, true);
            if (!isValid)
            {
                return Results.BadRequest(errors.FirstOrDefault()?.ErrorMessage);
            }

            var result = await @delegate(context);  // invokes the subsequent filter or endpoint after logic
            
            // After logic here 
            return result;
        });;
        
        //PUT /products/{id}

        routeGroupBuilder.MapPut("/{id}", async (HttpContext context, int id, [FromBody] Product product) =>
        {
            Product? productFromCollection = list.FirstOrDefault(temp => temp.Id == id);
            if (productFromCollection == null)
            {
                return Results.BadRequest("Incorrect product ID");
            }

            productFromCollection.ProductName = product.ProductName;

            return Results.Ok("Product Updated");
        });

        
        //DELETE /products/{id}

        routeGroupBuilder.MapDelete("/{id}", async (HttpContext context, int id) =>
        {
            Product? productFromCollection = list.FirstOrDefault(temp => temp.Id == id);
            if (productFromCollection == null)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>()
                {
                    { "Id", new string[] {"Incorrect Product ID"} }
                });
            }

            list.Remove(productFromCollection);
            return Results.Ok(new { message = "Product deleted" });
        });
        return routeGroupBuilder;
        
    }
}