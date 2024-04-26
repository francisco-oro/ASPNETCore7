using Microsoft.EntityFrameworkCore;
using OrdersWebAPI.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddHttpLogging(o => { });
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();
app.UseHttpLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
