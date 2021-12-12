using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Crypto.Com.Exchange.Api.Models
{
    public class Trade
    {
        [JsonPropertyName("i")]
        public string InstrumentName { get; set; }

        [JsonPropertyName("p")]
        public decimal? TradePrice { get; set; }

        [JsonPropertyName("q")]
        public decimal? TradeQuantity { get; set; }

        [JsonPropertyName("s")]
        public string Side { get; set; }

        [JsonPropertyName("d")]
        public long? TradeId { get; set; }

        [JsonPropertyName("dataTime")]
        public long? DataTime { get; set; }

        [JsonPropertyName("t")]
        public long T { get; set; }

        public DateTime Timestamp => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(T);
    }
}
