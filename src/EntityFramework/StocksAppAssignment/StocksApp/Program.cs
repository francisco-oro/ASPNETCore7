using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using StocksApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IFinnhubService,FinnhubService>();
builder.Services.AddScoped<IStocksService, StocksService>();

//Db context
builder.Services.AddDbContext<StockMarketDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
