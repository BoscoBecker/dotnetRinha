﻿using System.Text.Json.Serialization;

namespace dotnetRinha.Entities
{
    public class PaymentSummary
    {
        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("totalRequests")]
        public int TotalRequests { get; set; }

        [JsonPropertyName("totalFee")]
        public decimal TotalFee { get; set; }

        [JsonPropertyName("feePerTransaction")]
        public decimal FeePerTransaction { get; set; }

    }
}
