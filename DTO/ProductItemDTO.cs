namespace E_Commerce.DTO
{
    public class ProductItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ProductImage { get; set; }
        public double AverageRating { get; set; }
    }
}
