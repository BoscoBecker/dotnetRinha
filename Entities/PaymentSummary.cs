using System.Text.Json.Serialization;

namespace dotnetRinha.Entities
{
    public class PaymentSummary
    {
        public decimal Total { get; set; }
        public int Count { get; set; }
        [JsonPropertyName("byProcessor")]
        public Dictionary<string, ProcessorSummary> ByProcessor { get; set; } = [];

    }
}
