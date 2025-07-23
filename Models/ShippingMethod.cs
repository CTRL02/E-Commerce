using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class ShippingMethod
    {
        [Key]
        public int ShippingMethodId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
