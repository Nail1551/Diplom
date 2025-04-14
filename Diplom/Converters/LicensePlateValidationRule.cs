using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Diplom.Converters
{
    public class LicensePlateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;
            if (string.IsNullOrWhiteSpace(input))
                return ValidationResult.ValidResult;

            if (!Regex.IsMatch(input, @"^[А-Я0-9]+$"))
                return new ValidationResult(false, "Только заглавные русские буквы и цифры");

            return ValidationResult.ValidResult;
        }
    }
}
