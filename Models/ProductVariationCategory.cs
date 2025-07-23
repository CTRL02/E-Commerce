namespace E_Commerce.Models
{
    public class ProductVariationCategory
    {
        public int CategoryId { get; set; }
        public int VariationId { get; set; }

        public Category Category { get; set; }
        public Variation Variation { get; set; }
    }
}
