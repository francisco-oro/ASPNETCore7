using MinimalAPI.RouteGroups;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var mapGroup = app.MapGroup("/products").ProductsAPI();


app.Run();