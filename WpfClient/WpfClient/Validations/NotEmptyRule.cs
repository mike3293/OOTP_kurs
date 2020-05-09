using System.Globalization;
using System.Windows.Controls;

namespace WpfClient.Validations
{
    public class NotEmptyRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {
                string input = value as string;

                if (input.Length > 0)
                {
                    return new ValidationResult(true, null);
                }
            }

            return new ValidationResult(false, "Field required");
        }
    }
}
