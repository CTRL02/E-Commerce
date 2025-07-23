using System.Text.Json.Serialization;

namespace E_Commerce.DTO
{
    public class UserResendEmailDTO
    {
        public string Email { get; set; }

        [JsonIgnore]
        public string ClientUri { get; set; } = @"http://pazzify.runasp.net/User/EmailConfirmation";
    }
}