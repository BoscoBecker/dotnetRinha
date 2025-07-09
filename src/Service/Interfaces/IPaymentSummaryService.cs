using dotnetRinha.Entities;

namespace dotnetRinha.Service.Interfaces
{
    public interface IPaymentSummaryService
    {
        Task<PaymentSummary> GetSummaryAsync();
    }
}
