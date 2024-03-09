using ServiceContracts.CustomValidators;
using System.ComponentModel.DataAnnotations;


namespace ServiceContracts.DTO
{
    public class SellOrderRequest
    {
        [Required(ErrorMessage = "{0} is required")]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string? StockName { get; set; }

        [MinimumYearValidator(2000)]
        public DateTime? DateAndTimeOfOffer { get; set; }

        [Range(1, 100000, ErrorMessage = "Value should be between 1 and 100000")]
        public int? Quantity { get; set; }

        [Range(1, 100000, ErrorMessage = "Value should be between 1 and 100000")]
        public double? Price { get; set; }
    }
}
