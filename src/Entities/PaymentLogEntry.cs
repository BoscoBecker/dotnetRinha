namespace dotnetRinha.Entities
{
    public class PaymentLogEntry
    {
        public string Source { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public decimal Amount { get; set; }
    }
}
