using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class PeopleDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("People");

            //Seed to Countries 
            modelBuilder.Entity<Country>().HasData(new Country()
            {
                CountryID = Guid.NewGuid(), CountryName = "Sample"
            });
        }
    }
}
