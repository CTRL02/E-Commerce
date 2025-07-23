namespace E_Commerce.Models
{

    public class Variation
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public Category Category { get; set; }
        public ICollection<VariationOption> VariationOptions { get; set; }

        public ICollection<ProductVariationCategory> productVariationCategories { get; set; }

    }

}
