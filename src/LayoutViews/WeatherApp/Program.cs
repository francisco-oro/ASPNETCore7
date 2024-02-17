var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseStatusCodePages();
app.UseStatusCodePagesWithRedirects("/ErrorPage/{0}");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();


app.Run();
