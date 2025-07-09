using System.Text;
using System.Text.Json;
using NBomber.CSharp;

namespace LoadTestingBomber
{
    public partial class Program
    {
        private static void Main()
        {
            Console.WriteLine("NBomber POST Test Started...");
            using var httpClient = new HttpClient();
            var step = Scenario.Create("post_payment", async context =>
            {
                var correlationId = Guid.NewGuid();
                var amount = Random.Shared.NextDouble() * 1000;
                var requestedAt = DateTime.UtcNow.ToString("o");
                var payload = new
                {
                    correlationId,
                    amount = Math.Round(amount, 2),
                    requestedAt
                };
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("http://localhost:8001/payments", content);
                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();

            }).WithWarmUpDuration(TimeSpan.FromSeconds(5))
              .WithLoadSimulations(NBomber.Contracts.LoadSimulation.NewKeepConstant(_copies: 500, _during: TimeSpan.FromSeconds(60)));
            NBomberRunner
           .RegisterScenarios(step)
           .Run();
            Console.WriteLine("NBomber POST Test Completed.");
        }
    }
}