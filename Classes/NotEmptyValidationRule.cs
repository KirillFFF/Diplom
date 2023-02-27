using System.Globalization;
using System.Windows.Controls;

namespace ProductsClient.Classes
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult(false, "Значение не может быть пустым");
            else
                return new ValidationResult(true, null);
        }
    }
}
