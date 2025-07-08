using dotnetRinha.Entities;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net.Http;

namespace dotnetRinha.Service
{
    public class PaymentSummaryService : IPaymentSummaryService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private DateTime _lastHealthCheckDefault = DateTime.MinValue;
        private bool _defaultHealthy = true;
        public PaymentSummaryService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async  Task<PaymentSummary> GetSummaryAsync()
        {
            var verify = new Verify(httpClientFactory);
            var now = DateTime.UtcNow;
            if ((now - _lastHealthCheckDefault).TotalSeconds >= 5)
            {
                _defaultHealthy = await verify.CheckHealth("default");
                _lastHealthCheckDefault = now;
            }

            var client = _defaultHealthy ? httpClientFactory.CreateClient("default") : httpClientFactory.CreateClient("fallback");
                client.DefaultRequestHeaders.Add("X-Rinha-Token", "123");
            try
            {
                var response = await client.GetFromJsonAsync<PaymentSummary>("/admin/payments-summary");
                return response ;
            }
            catch
            {
                return new PaymentSummary(); 
            }
            
        }
    }
}
