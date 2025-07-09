using dotnetRinha.Entities;

namespace dotnetRinha.Service.Interfaces
{
    public interface IPaymentLogService
    {
       public Task LogAsync(string source, decimal amount);
<<<<<<<<< Temporary merge branch 1
        public Task<List<PaymentLogEntry>> GetLogsAsync(DateTime from, DateTime to);
=========
       public Task<bool> ExistsOrInsertCorrelationIdAsync(Guid correlationId);
       public Task<List<PaymentLogEntry>> GetLogsAsync(DateTime from, DateTime to);       
>>>>>>>>> Temporary merge branch 2
    }
}
