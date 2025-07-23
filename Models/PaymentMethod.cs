namespace E_Commerce.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public string UserId { get; set; }
        public int PaymentTypeId { get; set; }
        public string Provider { get; set; }
        public string AccountNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsDefault { get; set; }
        public User User { get; set; }
        public Payment PaymentType { get; set; }

        public ICollection<Order> orders { get; set; }


    }
}
