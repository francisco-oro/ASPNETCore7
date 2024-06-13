using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Core.Services;
using CitiesManager.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));
}).AddXmlSerializerFormatters();

builder.Services.AddTransient<IJwtService, JwtService>();

builder.Services.AddApiVersioning(config =>
{
    config.ApiVersionReader = new UrlSegmentApiVersionReader();
    //config.ApiVersionReader = new HeaderApiVersionReader(); //Reads version number from request header called "api-version". Eg: api-version: 1.0
    
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;

});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

//Swagger
builder.Services.AddEndpointsApiExplorer(); //Generates description for all endpoints
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Cities Web API", Version = "1.0"});
    options.SwaggerDoc("v2", new OpenApiInfo() { Title = "Cities Web API", Version = "2.0"});
}); //generates OpenAPI specification

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV"; //v1 
    setup.SubstituteApiVersionInUrl = true;
});

// CORS: localhost:4200, localhost:4100
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
            .WithHeaders("Authorization", "origin", "accept", "content-type")
            .WithMethods("GET", "POST", "PUT", "DELETE");
    });
    options.AddPolicy("4100Client" ,corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .WithOrigins(builder.Configuration.GetSection("AllowedOrigins2").Get<string[]>() ?? Array.Empty<string>())
            .WithHeaders("Authorization", "origin", "accept")
            .WithMethods("GET");
    });
});

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddUserStore<UserStore<ApplicationUser,ApplicationRole, ApplicationDbContext, Guid>>()
.AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger(); //creates endpoint for swagger.json
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");
}); //creates swagger UI for testing all web API endpoints / action methods

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
