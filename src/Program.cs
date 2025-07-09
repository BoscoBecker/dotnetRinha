using dotnetRinha.Service.HealthCheck;
using dotnetRinha.Service.Interfaces;
using dotnetRinha.Service.Log;
using dotnetRinha.Service.Payment;
using StackExchange.Redis;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddSingleton<IPaymentProcessorService, PaymentProcessorService>();
        builder.Services.AddSingleton<IPaymentSummaryService, PaymentSummaryService>();
        builder.Services.AddSingleton<VerifyHealthEndpoint>();
        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
        builder.Services.AddSingleton<IPaymentLogService, RedisPaymentLogService>();

        builder.Services.AddHttpClient("default", client =>
        {
            client.BaseAddress = new Uri("http://localhost:8001/");
        });
        
        builder.Services.AddHttpClient("fallback", client => {
            client.BaseAddress = new Uri("http://localhost:8002/");
        });

        var app = builder.Build();
        if (app.Environment.IsDevelopment()) app.MapOpenApi();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}