namespace E_Commerce.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
        public string CategoryName { get; set; }

        public Category ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<ProductVariationCategory> ProductVariationCategories { get; set; }
        public ICollection<PromotionCategory> PromotionCategories { get; set; }
    }
}
