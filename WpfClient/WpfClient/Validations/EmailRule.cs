using DataAnnotations = System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Windows.Controls;

namespace WpfClient.Validations
{
    public class EmailRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {
                string input = value as string;

                if (ValidateEmail(input))
                {
                    return new ValidationResult(true, null);
                }
            }

            return new ValidationResult(false, "Email is not valid");
        }

        private bool ValidateEmail(string email)
        {
            var attribute = new DataAnnotations.EmailAddressAttribute();

            return attribute.IsValid(email);
        }
    }
}
