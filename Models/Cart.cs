namespace E_Commerce.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string UserId { get; set; }

        public User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
