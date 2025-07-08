using dotnetRinha.Entities;

namespace dotnetRinha.Service
{
    public interface IPaymentProcessorService
    {
        Task<bool> ProcessPaymentAsync(PaymentRequest request);
    }
}
