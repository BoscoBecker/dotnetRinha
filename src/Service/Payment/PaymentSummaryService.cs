using dotnetRinha.Entities;
using dotnetRinha.Service.HealthCheck;
using dotnetRinha.Service.Interfaces;

namespace dotnetRinha.Service.Payment
{
    public class PaymentSummaryService(IHttpClientFactory httpClientFactory) : IPaymentSummaryService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private bool _defaultHealthy = true;

        public async  Task<PaymentSummary> GetSummaryAsync()
        {
            var PaymentProcessor = new VerifyHealthEndpoint(_httpClientFactory);
            _defaultHealthy = await PaymentProcessor.CheckHealth("default");

            var client = _defaultHealthy ? _httpClientFactory.CreateClient("default") : _httpClientFactory.CreateClient("fallback");
                client.DefaultRequestHeaders.Add("X-Rinha-Token", "123");
            try
            {
                return await client.GetFromJsonAsync<PaymentSummary>("/admin/payments-summary");               
                
            }
            catch
            {
                return new PaymentSummary(); 
            }
            
        }
    }
}
