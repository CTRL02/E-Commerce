using Microsoft.Build.Evaluation;

namespace E_Commerce.Models
{
    public class OrderLine
    {
        public int OrderLineId { get; set; }
        public int ProductItemId { get; set; }
        public int OrderId { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }

        public ProductItem ProductItem { get; set; }
        public Order Order { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
