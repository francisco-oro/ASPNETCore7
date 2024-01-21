var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouter();
app.UseEndpoints();
app.MapGet("/", () => "Hello World!");

app.Run();
