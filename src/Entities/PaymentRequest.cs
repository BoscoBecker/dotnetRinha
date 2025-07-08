using System.ComponentModel.DataAnnotations;

namespace dotnetRinha.Entities
{
    public class PaymentRequest
    {
        [Required]
        public Guid CorrelationId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime RequestedAt { get; set; }
    }

    public class PaymentResponse
    {
        public string Message { get; set; } = "payment processed successfully";
    }
}
