using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class PeopleDbContext : DbContext
    {
        public PeopleDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("People");

            //Seed to Countries 
            string countriesJson = File.ReadAllText("countries.json");
            List<Country>? countries =  JsonSerializer.Deserialize<List<Country>>(countriesJson);

            if (countries != null)
                foreach (Country country in countries)
                {
                    modelBuilder.Entity<Country>().HasData(country);
                }

            //Seed to People 
            string peopleJson = File.ReadAllText("people.json");
            List<Person>? people = JsonSerializer.Deserialize<List<Person>>(peopleJson);

            if (people != null)
                foreach (Person person in people)
                {
                    modelBuilder.Entity<Person>().HasData(person);
                }

        }
    }
}
