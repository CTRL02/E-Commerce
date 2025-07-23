namespace E_Commerce.Models
{
    public class PromotionCategory
    {
        public int CategoryId { get; set; }
        public int PromotionId { get; set; }

        public Category Category { get; set; }
        public Promotion Promotion { get; set; }
    }
}
