namespace E_Commerce.Models
{
    public class Promotion
    {
        public int PromotionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal DiscountRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<PromotionCategory> PromotionCategories { get; set; }
    }
}
