using SocialMediaLinks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<SocialMediaLinksOptions>(builder.Configuration.GetSection("SocialMediaLinks"));
var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
