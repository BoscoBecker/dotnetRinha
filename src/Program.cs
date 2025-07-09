using dotnetRinha.Service.HealthCheck;
using dotnetRinha.Service.Interfaces;
using dotnetRinha.Service.Log;
using dotnetRinha.Service.Payment;
using StackExchange.Redis;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseUrls("http://0.0.0.0:8080");
        builder.Services.AddControllers();
        builder.Services.AddSingleton<IPaymentProcessorService, PaymentProcessorService>();
        builder.Services.AddSingleton<IPaymentSummaryService, PaymentSummaryService>();
        builder.Services.AddSingleton<VerifyHealthEndpoint>();
        
        var redisConnectionString = "redis:6379,abortConnect=false";        
        builder.Services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(redisConnectionString)
        );
        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
        builder.Services.AddSingleton<IPaymentLogService, RedisPaymentLogService>();

        builder.Services.AddHttpClient("default", client =>
        {
            client.BaseAddress = new Uri("http://backend-1:8080/");
        });
        
        builder.Services.AddHttpClient("fallback", client => {
            client.BaseAddress = new Uri("http://backend-2:8080/");
        });

        var app = builder.Build();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}