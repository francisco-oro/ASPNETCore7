using System.Text.Json;
using Microsoft.Data.SqlClient;
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

        public List<Person> sp_GetAllPeople()
        {
            return People.FromSqlRaw("EXECUTE [dbo].[GetAllPeople]").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            object[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@PersonID", person.PersonID),
                new SqlParameter("@Email", person.PersonName),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@CountryID", person.CountryID),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
            };

            return Database.ExecuteSqlRaw(
                "EXECUTE [dbo].[InsertPerson] @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters",
                parameters);
        }
    }
}
