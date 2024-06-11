using CitiesManager.Core.Entities;
using CitiesManager.Core.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.Infrastructure.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid> 
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected ApplicationDbContext()
        {
        }

        public virtual DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(new City() { CityID = Guid.Parse("3FA48B29-D34D-4280-8EFE-E374F58054E6"), CityName = "New York" });
            modelBuilder.Entity<City>().HasData(new City() { CityID = Guid.Parse("0B0ECB18-A053-4E04-A583-9587BAE7D28E"), CityName = "London" });
        }
    }
}
