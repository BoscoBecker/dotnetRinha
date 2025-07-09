using dotnetRinha.Entities;
using dotnetRinha.Service.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace dotnetRinha.Service.Log
{
    public class RedisPaymentLogService : IPaymentLogService
    {
        private readonly IDatabase _redis;
        private const string Key = "payments:logs";
<<<<<<< HEAD
        public RedisPaymentLogService(IConnectionMultiplexer redis) => _redis = redis.GetDatabase();        
=======
        public RedisPaymentLogService(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }
>>>>>>> 91ad82d9f53d2746d3a0620b5ffaddda4a3d61fb
        public Task<List<PaymentLogEntry>> GetLogsAsync(DateTime from, DateTime to)
        {
            var logs = new List<PaymentLogEntry>();
            var end = DateTime.UtcNow;
<<<<<<< HEAD
            var start = end.AddDays(-30);
=======
            var start = end.AddDays(-30); // Default to last 30 days
>>>>>>> 91ad82d9f53d2746d3a0620b5ffaddda4a3d61fb
            if (from > DateTime.MinValue && to > DateTime.MinValue)
            {
                start = from;
                end = to;
            }
            var entries = _redis.ListRange(Key, 0, -1);
            foreach (var entry in entries)
            {
                var logEntry = JsonSerializer.Deserialize<PaymentLogEntry>(entry);
                if (logEntry != null && logEntry.Timestamp >= start && logEntry.Timestamp <= end)
                {
                    logs.Add(logEntry);
                }
            }
            return Task.FromResult(logs);
        }

        public async Task LogAsync(string source, decimal amount)
        {
            var entry = new PaymentLogEntry
            {
                Source = source,
                Timestamp = DateTime.UtcNow,
                Amount = amount
            };
            var json = JsonSerializer.Serialize(entry);
            await _redis.ListRightPushAsync(Key, json);

        }
<<<<<<< HEAD
        public async Task<bool> ExistsOrInsertCorrelationIdAsync(Guid correlationId)
        {
            if (correlationId == Guid.Empty)
                return false;

            var key = $"correlation:{correlationId}";
            bool inserted = await _redis.StringSetAsync(
                key,
                "1",
                expiry: TimeSpan.FromMinutes(10),
                when: When.NotExists);
            return !inserted;
        }
      
=======
>>>>>>> 91ad82d9f53d2746d3a0620b5ffaddda4a3d61fb
    }
}
