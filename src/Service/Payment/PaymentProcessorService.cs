using dotnetRinha.Entities;
using dotnetRinha.Service.HealthCheck;
using dotnetRinha.Service.Interfaces;
using dotnetRinha.Service.Log;

namespace dotnetRinha.Service.Payment
{
    public class PaymentProcessorService(IHttpClientFactory httpClientFactory, IPaymentLogService paymentLogService) : IPaymentProcessorService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IPaymentLogService _paymentLogService = paymentLogService;
        private bool _defaultHealthy = true;

        public async Task<bool> ProcessPaymentAsync(PaymentRequest request)
        {
            if (await _paymentLogService.ExistsOrInsertCorrelationIdAsync(request.CorrelationId)) return false;

            var PaymentProcessor = new VerifyHealthEndpoint(_httpClientFactory);
                _defaultHealthy = await PaymentProcessor.CheckHealth("default");
            var client = _defaultHealthy ? _httpClientFactory.CreateClient("default") : _httpClientFactory.CreateClient("fallback");
            var response = await client.PostAsJsonAsync("/payments", request);

            if (response != null)
            {
                var clientName = _defaultHealthy ? "default" : "fallback";
                await _paymentLogService.LogAsync(clientName, request.Amount);
            } else return false;

            return response.IsSuccessStatusCode;
        }
    }
}
