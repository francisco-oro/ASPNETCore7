﻿using ContactsManager.Core.Domain.Entities;

namespace ContactsManager.Core.DTO
{
    /// <summary>
    /// DTO class that is used as return type for most of CountriesService methods
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

        // It compares the current object to another object of CurrentResponse type and returns true, if both values are the same;
        // Otherwise returns false
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }

            CountryResponse country_to_compare = obj as CountryResponse;

            return this.CountryID == country_to_compare.CountryID && 
                   this.CountryName == country_to_compare.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class CountryExtensions
    {
        //Converts from Country object to CountryResponse object
        public static CountryResponse? ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName,
            };
        }
    }
}