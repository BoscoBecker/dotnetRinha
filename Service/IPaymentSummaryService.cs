using dotnetRinha.Entities;

namespace dotnetRinha.Service
{
    public interface IPaymentSummaryService
    {
        Task<PaymentSummary> GetSummaryAsync();
    }
}
