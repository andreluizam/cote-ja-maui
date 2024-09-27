using Newtonsoft.Json;
using System;

namespace Cota_aqui.Model
{
    public class Cotacao
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("codein")]
        public string CodeIn { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("high")]
        public decimal High { get; set; }

        [JsonProperty("low")]
        public decimal Low { get; set; }

        [JsonProperty("varBid")]
        public decimal VarBid { get; set; }

        [JsonProperty("pctChange")]
        public decimal PctChange { get; set; }

        [JsonProperty("bid")]
        public decimal Bid { get; set; }

        [JsonProperty("ask")]
        public decimal Ask { get; set; }

        [JsonProperty("timestamp")]
        public float Timestamp { get; set; }

        [JsonProperty("create_date")]
        public DateTime CreateDate { get; set; }
    }
}
