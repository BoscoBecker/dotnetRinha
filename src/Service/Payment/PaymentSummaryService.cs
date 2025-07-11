using dotnetRinha.Entities;
using dotnetRinha.Service.HealthCheck;
using dotnetRinha.Service.Interfaces;

public class PaymentSummaryService(IHttpClientFactory httpClientFactory, VerifyHealthEndpoint healthChecker) : IPaymentSummaryService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly VerifyHealthEndpoint _healthChecker = healthChecker;

    public async Task<PaymentSummary> GetSummaryAsync()
    {
        try
        {
            var healthy = await _healthChecker.CheckHealth("default");
            var clientName = healthy ? "default" : "fallback";
            var client = _httpClientFactory.CreateClient(clientName);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("X-Rinha-Token", "123");

            return await client.GetFromJsonAsync<PaymentSummary>("/admin/payments-summary")
                ?? new PaymentSummary();
        }
        catch (Exception)
        {
            return new PaymentSummary();
        }
    }
}
