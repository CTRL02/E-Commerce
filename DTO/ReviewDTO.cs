using E_Commerce.Models;

namespace E_Commerce.DTO
{
   public class ReviewDTO
   {
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public int ProductItemId { get; set; }
        public int OrderLineId { get; set; }
        public OrderLine OrderLine { get; set; }

    }

}
