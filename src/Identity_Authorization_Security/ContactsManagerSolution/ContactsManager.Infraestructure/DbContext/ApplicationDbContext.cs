using System.Text.Json;
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ContactsManager.Infrastructure.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("People");

            //Seed to Countries 
            string countriesJson = File.ReadAllText("countries.json");
            List<Country>? countries = JsonSerializer.Deserialize<List<Country>>(countriesJson);

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

            //Fluent API 
            modelBuilder.Entity<Person>().Property(temp => temp.TIN)
                .HasColumnName("TaxIdentificationNumber")
                .HasColumnType("varchar(8)")
                .HasDefaultValue("ABC12345");

            //modelBuilder.Entity<Person>()
            //    .HasIndex(temp => temp.TIN).IsUnique();
            modelBuilder
                .Entity<Person>()
                .ToTable(b => b.HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8"));

            //Table Relations
            //modelBuilder.Entity<Person>(entity =>
            //{
            //    entity.HasOne<Country>(person => person.Country)
            //        .WithMany(country => country.People)
            //        .HasForeignKey(person => person.CountryID);
            //});
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
