using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Review
    {
        [Key]
        public int UserReviewId { get; set; }
        public string UserId { get; set; }
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public int OrderLineId { get; set; }
        public User User { get; set; }
        public OrderLine OrderLine { get; set; }

    }
}
