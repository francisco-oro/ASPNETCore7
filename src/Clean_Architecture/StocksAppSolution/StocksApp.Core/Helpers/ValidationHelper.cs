
using System.ComponentModel.DataAnnotations;

namespace StocksApp.Core.Helpers
{
    internal class ValidationHelper
    {
        internal static void ModelValidation(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> errors = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, validationContext, errors, true);

            if (!isValid)
            {
                throw new ArgumentException(errors.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
