using dotnetRinha.Service;
using Polly;
using Polly.Extensions.Http;

internal class Program
{
    private static IAsyncPolicy<HttpResponseMessage> RetryPolicy => 
        HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(300 * retryAttempt));


    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddSingleton<IPaymentProcessorService, PaymentProcessorService>();
        builder.Services.AddSingleton<IPaymentSummaryService, PaymentSummaryService>();
        builder.Services.AddSingleton<Verify>();

        builder.Services.AddHttpClient("default", client => {
            client.BaseAddress = new Uri("http://localhost:8001/");
        }).AddPolicyHandler(RetryPolicy);
        
        builder.Services.AddHttpClient("fallback", client => {
            client.BaseAddress = new Uri("http://localhost:8002/");
        }).AddPolicyHandler(RetryPolicy);

        var app = builder.Build();
        if (app.Environment.IsDevelopment()) app.MapOpenApi();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }


}