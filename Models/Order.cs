namespace E_Commerce.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public int PaymentMethodId { get; set; }
        public int ShippingAddressId { get; set; }
        public int ShippingMethodId { get; set; }
        public decimal OrderTotal { get; set; }
        public int OrderStatusId { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public User User { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ICollection<OrderLine> OrderLines { get; set; }

    }
}
