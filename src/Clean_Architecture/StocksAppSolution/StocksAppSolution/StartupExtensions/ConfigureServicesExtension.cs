using Microsoft.EntityFrameworkCore;
using StocksApp.Core.Domain.RepositoryContracts;
using StocksApp.Core.Domain.RepositoryContracts.ServiceContracts;
using StocksApp.Core.ServiceContracts;
using StocksApp.Core.Services;
using StocksApp.Infrastructure.DbContext;
using StocksApp.Infrastructure.Repositories;

namespace StocksApp.UI.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpLogging(o => { });
            services.AddControllersWithViews();
            services.AddHttpClient();

            services.AddScoped<IFinnhubRespository, FinnhubRepository>();
            services.AddScoped<IStocksRepository, StocksRepository>();
            services.AddScoped<IFinnhubSearchStocksService, FinnhubSearchStocksService>();
            services.AddScoped<IFinnhubStockPriceQuoteService, FinnhubStockPriceQuoteService>();
            services.AddScoped<IFinnhubStocksService, FinnhubStocksService>();
            services.AddScoped<IFinnhubCompanyProfileService, FinnhubCompanyProfileService>();
            services.AddScoped<IStocksService, StocksService>();

            services.Configure<TradingOptions>(configuration.GetSection("TradingOptions"));
            //Db context
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
