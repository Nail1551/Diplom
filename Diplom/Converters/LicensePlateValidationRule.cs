using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;

namespace Diplom.Converters
{
    public class LicensePlateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            if (string.IsNullOrWhiteSpace(input))
                return ValidationResult.ValidResult;

            // Приводим к верхнему регистру
            input = input.ToUpper(new CultureInfo("ru-RU"));

            // Проверка: только заглавные русские буквы и цифры
            if (!Regex.IsMatch(input, @"^[А-Я0-9]+$"))
                return new ValidationResult(false, "Допустимы только заглавные русские буквы и цифры");

            return ValidationResult.ValidResult;
        }
    }

    // 🔁 Конвертер для автоматического изменения значения на заглавные
    public class UppercaseConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            return value is string str
                ? str.ToUpper(new CultureInfo("ru-RU"))
                : value;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            return value is string str
                ? str.ToUpper(new CultureInfo("ru-RU"))
                : value;
        }
    }
}
