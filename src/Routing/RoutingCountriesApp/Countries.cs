namespace RoutingCountriesApp
{
    public class Countries
    {
        private static readonly Dictionary<int, string> _countries = new Dictionary<int, string>()
        {
            {1, "United States"},
            {2, "Canada"},
            {3, "United Kingdom"},
            {4, "India"},
            {5, "Japan"},
        };

        public static string? GetCountryById(int id)
        {
            if(_countries.TryGetValue(id, out string country))
            {
                return country;
            }
            return null;
        }

        public static Dictionary<int, string> GetAll()
        {
            return _countries; 
        }
    }
}
