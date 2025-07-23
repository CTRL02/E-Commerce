using Microsoft.Build.Evaluation;

namespace E_Commerce.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int ProductItemId { get; set; }
        public int Qty { get; set; }

        public Cart Cart { get; set; }
        public ProductItem ProductItem { get; set; }
    }
}
