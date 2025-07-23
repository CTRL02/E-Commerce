using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace E_Commerce.DTO
{
    public class RegisterDTO
    {
        public string Username { get; set; }

        [CustomEmailValidation(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        [JsonIgnore]
        public string ClientUri { get; set; } = @"http://pazzify.runasp.net/User/EmailConfirmation";
    }

    public class CustomEmailValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string email)
            {
                // Define a regex pattern for validating email addresses
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

                if (Regex.IsMatch(email, emailPattern))
                {
                    return ValidationResult.Success; // Email is valid
                }
            }

            // Return a validation error if email is invalid
            return new ValidationResult(ErrorMessage ?? "Invalid email address.");
        }
    }
}
