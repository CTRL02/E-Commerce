using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System.Collections.Generic;

namespace E_Commerce.Models
{
    public class User : IdentityUser
    {
        public DateTime RegistrationDate { get; set; }

        public Cart cart { get; set; }

        public int CartId {  get; set; }

        // Navigation properties
        public ICollection<ShippingAddress> Addresses { get; set; }
        public Cart ShoppingCart { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }

        public ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}
