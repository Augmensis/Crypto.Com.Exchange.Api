using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Crypto.Com.Exchange.Api.Models
{
    public class Ticker
    {
        [JsonPropertyName("i")]
        public string InstrumentName { get; set; }

        [JsonPropertyName("b")]
        public decimal? Bid { get; set; }

        [JsonPropertyName("k")]
        public decimal? Ask { get; set; }

        [JsonPropertyName("a")]
        public decimal? LastTrade { get; set; }

        [JsonPropertyName("v")]
        public decimal? Volume { get; set; }

        [JsonPropertyName("h")]
        public decimal? HighestTrade { get; set; }

        [JsonPropertyName("l")]
        public decimal? LowestTrade { get; set; }

        [JsonPropertyName("c")]
        public decimal? PriceChange { get; set; }

        [JsonPropertyName("t")]
        public long T { get; set; }

        public DateTime Timestamp => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(T);
    }
}
