using dotnetRinha.Entities;

namespace dotnetRinha.Service.Interfaces
{
    public interface IPaymentProcessorService
    {
        Task<bool> ProcessPaymentAsync(PaymentRequest request);
    }
}
