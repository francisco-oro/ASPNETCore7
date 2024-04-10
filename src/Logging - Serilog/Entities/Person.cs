using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{
    /// <summary>
    /// Person domain model class
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        [StringLength(40)] //nvarchar(40)
        public string? PersonName { get; set; }

        [StringLength(40)] //nvarchar(40)
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)] //nvarchar(10)
        public string? Gender { get; set; }

        //uniqueidentifier
        public Guid? CountryID { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        //bit
        public bool ReceiveNewsLetters { get; set; }

        public string? TIN { get; set; }

        [ForeignKey("CountryID")]
        public Country? Country { get; set; }

        public override string ToString()
        {
            return $"Person ID:{PersonID}, Person Name: {PersonName}, Email: {Email}, Date of Birth: {DateOfBirth?.ToString("MM/dd/yyyy")}, Gender: {Gender}, CountryID: {CountryID}, Country: {Country?.CountryName}, Address: {Address}, Receive news letters: {ReceiveNewsLetters}"; 
        }
    }
}
