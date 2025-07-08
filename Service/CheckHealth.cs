using System.Net.Http;

namespace dotnetRinha.Service
{
    public class Verify
    {
        
        private readonly IHttpClientFactory _httpClientFactory;
        public Verify(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> CheckHealth(string clientName)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(clientName);
                var response = await client.GetAsync("/payments/service-health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
