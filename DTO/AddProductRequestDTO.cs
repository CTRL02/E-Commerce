namespace E_Commerce.DTO
{
    public class AddProductRequestDTO
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int QtyInStock { get; set; }
        public string Description { get; set; }
        public string ProductImage { get; set; }
        public List<ProductVariationDTO> Variations { get; set; }
    }

    public class ProductVariationDTO
    {
        public int VariationId { get; set; }
        public int VariationOptionId { get; set; }
    }
}
