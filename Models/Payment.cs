namespace E_Commerce.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public ICollection<PaymentMethod> UserPaymentMethods { get; set; }
    }
}
