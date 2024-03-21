using ServiceContracts;
using Services;
using StocksApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IFinnhubService,FinnhubService>();
builder.Services.AddSingleton<IStocksService, StocksService>();

var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
