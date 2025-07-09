namespace dotnetRinha.Entities
{
    public class PaymentLogEntry
    {
        public string Source { get; set; } = ""; // "default" ou "fallback"
        public DateTime Timestamp { get; set; }
        public decimal Amount { get; set; }
    }
}
