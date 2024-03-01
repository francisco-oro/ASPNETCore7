var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.Map("/", async context =>
    {
        app.Configuration.
    })
})


app.MapControllers();

app.Run();
