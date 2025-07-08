using dotnetRinha.Entities;

namespace dotnetRinha.Service
{
    public class PaymentProcessorService : IPaymentProcessorService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private DateTime _lastHealthCheckDefault = DateTime.MinValue;
        private bool _defaultHealthy = true;

        public PaymentProcessorService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> ProcessPaymentAsync(PaymentRequest request)
        {
            var verify = new Verify(_httpClientFactory);
            var now = DateTime.UtcNow;
            if ((now - _lastHealthCheckDefault).TotalSeconds >= 5)
            {
                _defaultHealthy = await verify.CheckHealth("default");
                _lastHealthCheckDefault = now;
            }

            var client = _defaultHealthy ? _httpClientFactory.CreateClient("default") : _httpClientFactory.CreateClient("fallback");
            var response = await client.PostAsJsonAsync("/payments", request);
            return response.IsSuccessStatusCode;
        }

    }

}
