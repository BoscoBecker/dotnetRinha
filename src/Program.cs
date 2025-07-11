using dotnetRinha.Service.HealthCheck;
using dotnetRinha.Service.Interfaces;
using dotnetRinha.Service.Payment;
using dotnetRinha.Service.Payment.Log;
using StackExchange.Redis;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseUrls("http://0.0.0.0:8080");
        builder.Services.AddControllers();
        builder.Services.AddScoped<IPaymentProcessorService, PaymentProcessorService>();
        builder.Services.AddScoped<IPaymentSummaryService, PaymentSummaryService>();
        builder.Services.AddScoped<IPaymentLogService, PaymentRedisLogService>();
        builder.Services.AddSingleton<VerifyHealthEndpoint>();
        var redisConnectionString = "redis:6379,abortConnect=false";
        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
        builder.Services.AddHttpClient("default", client => client.BaseAddress = new Uri("http://payment-processor-default:8080"));                
        builder.Services.AddHttpClient("fallback", client => client.BaseAddress = new Uri("http://payment-processor-fallback:8080"));
        var app = builder.Build();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}