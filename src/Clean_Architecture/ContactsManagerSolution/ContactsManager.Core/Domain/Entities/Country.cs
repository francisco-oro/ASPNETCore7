using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.Domain.Entities
{
    /// <summary>
    /// Domain Model for Country Details
    /// </summary>
    public class Country
    {
        [Key]
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

        public virtual ICollection<Person>? People { get; set; }
    }
}
