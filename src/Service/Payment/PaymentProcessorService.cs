using dotnetRinha.Entities;
using dotnetRinha.Service.HealthCheck;
using dotnetRinha.Service.Interfaces;


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
            var paymentProcessor = new VerifyHealthEndpoint(_httpClientFactory);
            var clientName = _defaultHealthy == true ? "default" : "fallback";
            var client = _httpClientFactory.CreateClient(clientName);

            _defaultHealthy = await paymentProcessor.CheckHealth("default");
            HttpResponseMessage? response = null;
            try
            {
                response = await client.PostAsJsonAsync("/payments", request);

                if ((!response.IsSuccessStatusCode || response == null) && clientName == "default")
                {
                    var fallbackClient = _httpClientFactory.CreateClient("fallback");
                    response = await fallbackClient.PostAsJsonAsync("/payments", request);

                    if (response.IsSuccessStatusCode)
                        clientName = "fallback";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"POST to {clientName} failed: {ex.Message}");

                if (clientName == "default")
                {
                    var fallbackClient = _httpClientFactory.CreateClient("fallback");
                    response = await fallbackClient.PostAsJsonAsync("/payments", request);

                    if (response.IsSuccessStatusCode)
                        clientName = "fallback";
                }
            }

            if (response != null && response.IsSuccessStatusCode)
            {
                await _paymentLogService.LogAsync(clientName, request.Amount);
                return true;
            }

            return false;
        }

    }
}

