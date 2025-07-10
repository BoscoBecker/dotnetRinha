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
                var response = await httpClient.PostAsync("http://localhost:9999/payment", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
            })
            .WithWarmUpDuration(TimeSpan.FromSeconds(5)) 
            .WithLoadSimulations(
                NBomber.Contracts.LoadSimulation.NewInject(
                    _rate: 10,
                    _interval: TimeSpan.FromSeconds(1),
                    _during: TimeSpan.FromSeconds(50) 
                )
            );

            NBomberRunner
                .RegisterScenarios(step)
                .Run();

            Console.WriteLine("NBomber POST Test Completed.");
        }

    }
}