using dotnetRinha.Entities;

namespace dotnetRinha.Service.Interfaces
{
    public interface IPaymentLogService
    {
       public Task LogAsync(string source, decimal amount);
<<<<<<< HEAD
       public Task<bool> ExistsOrInsertCorrelationIdAsync(Guid correlationId);
       public Task<List<PaymentLogEntry>> GetLogsAsync(DateTime from, DateTime to);       
=======
        public Task<List<PaymentLogEntry>> GetLogsAsync(DateTime from, DateTime to);
>>>>>>> 91ad82d9f53d2746d3a0620b5ffaddda4a3d61fb
    }
}
