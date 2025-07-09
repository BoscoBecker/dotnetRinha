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
        public RedisPaymentLogService(IConnectionMultiplexer redis) => _redis = redis.GetDatabase();
        public async Task<List<PaymentLogEntry>> GetLogsAsync(DateTime from, DateTime to)
        {
            var start = from > DateTime.MinValue ? from : DateTime.UtcNow.AddDays(-30);
            var end = to > DateTime.MinValue ? to : DateTime.UtcNow;

            var fromEpoch = new DateTimeOffset(start).ToUnixTimeSeconds();
            var toEpoch = new DateTimeOffset(end).ToUnixTimeSeconds();

            var entries = await _redis.SortedSetRangeByScoreAsync(Key, fromEpoch, toEpoch);
            var logs = new List<PaymentLogEntry>();
            
            foreach (var entry in entries)
            {
                var logEntry = JsonSerializer.Deserialize<PaymentLogEntry>(entry);
                if (logEntry != null) 
                    logs.Add(logEntry);
            }
            return logs;
        }


        public async Task LogAsync(string source, decimal amount)
        {
            var entry = new PaymentLogEntry
            {
                Source = source,
                Timestamp = DateTime.UtcNow,
                Amount = amount
            };

            string json = JsonSerializer.Serialize(entry);
            var score = new DateTimeOffset(entry.Timestamp).ToUnixTimeSeconds();
            await _redis.SortedSetAddAsync(Key, json, score);
        }

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
    }
}
