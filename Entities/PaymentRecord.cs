namespace dotnetRinha.Entities
{
    public class PaymentRecord
    {
        public decimal Amount { get; set; }
        public string  Processor { get; set; } = "default";

    }

}
