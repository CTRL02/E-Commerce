using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class ShippingAddress
    {
        [Key]
        public int UserAddressId { get; set; }
        public string UserId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public int CountryId { get; set; }
        public bool IsDefault { get; set; }
        public User User { get; set; }
        public Country Country { get; set; }

        public ICollection<Order> orders { get; set; }
    }
}
