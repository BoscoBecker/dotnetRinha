using System.Text.Json;

namespace dotnetRinha.Service.HealthCheck
{
    public class VerifyHealthEndpoint(IHttpClientFactory httpClientFactory)
    {        
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private DateTime _lastHealthCheckDefault = DateTime.MinValue;
        private bool _defaultHealthy = false;

        public async Task<bool> CheckHealth(string clientName)
        {
            try
            {
                var now = DateTime.UtcNow;
                if ((now - _lastHealthCheckDefault).TotalSeconds >= 5)
                {
                    var client = _httpClientFactory.CreateClient(clientName);
                    var response = await client.GetAsync("/payments/service-health");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var healthStatus = JsonSerializer.Deserialize<HealthStatusResponse>(content, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        _defaultHealthy = healthStatus is { Failing: false };
                    }
                    else                    
                        _defaultHealthy = false;                    
                    _lastHealthCheckDefault = now;
                }
                return _defaultHealthy;
            }
            catch
            {
                return false;
            }
        }
    }
}
