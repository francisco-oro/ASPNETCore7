using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StocksApp.Infrastructure.DbContext;

namespace StocksApp.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                ServiceDescriptor? serviceDescriptor = services.SingleOrDefault(temp =>
                    temp.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));


                if (serviceDescriptor != null)
                {
                    services.Remove(serviceDescriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DatabaseForTesting");
                });
                
            });
        }
    }
}
