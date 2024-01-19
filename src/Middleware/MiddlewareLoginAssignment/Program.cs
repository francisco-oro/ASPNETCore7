using MiddlewareLoginAssignment.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseAuthenticationMiddleware();

app.Run();
