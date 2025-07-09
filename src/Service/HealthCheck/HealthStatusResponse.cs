namespace dotnetRinha.Service.HealthCheck
{
    public class HealthStatusResponse
    {
        public bool Failing { get; set; }
        public int MinResponseTime { get; set; }
    }
}