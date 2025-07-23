using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace E_Commerce.DTO
{
    public class SellerRegisterDTO
    {
        public string Username { get; set; }

        [CustomEmailValidation(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        [JsonIgnore]
        public string ClientUri { get; set; } = @"http://pazzify.runasp.net/Seller/Seller-ConFirm-Email";
    }
}
