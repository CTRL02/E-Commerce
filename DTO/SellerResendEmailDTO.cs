using System.Text.Json.Serialization;

namespace E_Commerce.DTO
{
    public class SellerResendEmailDTO
    {
        public string Email { get; set; }

        [JsonIgnore]
        public string ClientUri { get; set; } = @"http://pazzify.runasp.net/Seller/Seller-ConFirm-Email";
    }
}
