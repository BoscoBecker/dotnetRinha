using dotnetRinha.Entities;

namespace dotnetRinha.Service.Interfaces
{
    public interface IPaymentSummaryStore
    {
        public void Add(string source, decimal amount);
        public IEnumerable<PaymentLogEntry> Get(DateTime from, DateTime to);
    }
}
