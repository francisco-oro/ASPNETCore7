using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using ContactsManager.Infrastructure.DbContext;
using ContactsManager.Infrastructure.Repositories;
using ContactsManager.UI.Filters.ActionFilters;
using CRUDExample.Filters.ActionFilters;
using Microsoft.EntityFrameworkCore;

namespace ContactsManager.UI.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ResponseHeaderActionFilter>();
            //it adds controllers and views as services
            services.AddControllersWithViews(options =>
            {
                //////options.Filters.Add<ResponseHeaderActionFilter>(5);

                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

                options.Filters.Add(new ResponseHeaderActionFilter(logger)
                {
                    Key = "My-Key-From-Global",
                    Value = "My-Value-From-Global",
                    Order = 2
                });
            });
             
            //add services into IoC container
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IPeopleRepository, PeopleRepository>();

            services.AddScoped<ICountriesGetterService, CountriesGetterService>();
            services.AddScoped<ICountriesUploaderService, CountriesUploaderService>();
            services.AddScoped<ICountriesAdderService, CountriesAdderService>();
            services.AddScoped<IPeopleDeleterService, PeopleDeleterService>();
            services.AddScoped<IPeopleGetterService, PeopleGetterServiceWithFewExcelFields>();
            services.AddScoped<PeopleGetterService, PeopleGetterService>();
            services.AddScoped<IPeopleAdderService, PeopleAdderService>();
            services.AddScoped<IPeopleSorterService, PeopleSorterService>();
            services.AddScoped<IPeopleUpdaterService, PeopleUpdaterService>();


            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<PeopleListActionFilter>();

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            return services;
        }
    }
}
