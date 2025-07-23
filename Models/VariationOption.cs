using Microsoft.Identity.Client;

namespace E_Commerce.Models
{
    public class VariationOption
    {
        public int Id { get; set; }
        public int VariationId { get; set; }
        public string Value { get; set; }

        public Variation Variation { get; set; }

        public ICollection<ProductConfiguration> ProductConfigurations { get; set; }

    }
}
