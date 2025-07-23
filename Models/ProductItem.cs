namespace E_Commerce.Models
{
    public class ProductItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string SKU { get; set; }
        public int QtyInStock { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ProductImage { get; set; }
        public string SellerId { get; set; }

        public Product Product { get; set; }
        public Seller Seller { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<OrderLine> OrderLines { get; set; }
        public ICollection<ProductConfiguration> ProductConfigurations { get; set; }
    }
}
