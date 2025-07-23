using Microsoft.Build.Evaluation;

namespace E_Commerce.Models
{
    public class ProductConfiguration
    {
        public int ProductConfigurationId { get; set; }
        public int ProductItemId { get; set; }
        public int VariationOptionId { get; set; }

        public ProductItem ProductItem { get; set; }
        public VariationOption VariationOption { get; set; }
    }
}
