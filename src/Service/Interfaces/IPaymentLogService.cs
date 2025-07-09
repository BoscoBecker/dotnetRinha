using dotnetRinha.Entities;

namespace dotnetRinha.Service.Interfaces
{
    public interface IPaymentLogService
    {
       public Task LogAsync(string source, decimal amount);
        public Task<List<PaymentLogEntry>> GetLogsAsync(DateTime from, DateTime to);
    }
}
