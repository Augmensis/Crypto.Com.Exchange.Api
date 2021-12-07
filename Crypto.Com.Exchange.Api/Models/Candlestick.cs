using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Crypto.Com.Exchange.Api.Models
{
    public class Candlestick
    {
        [JsonPropertyName("o")]
        public double Open { get; set; }

        [JsonPropertyName("h")]
        public double High { get; set; }

        [JsonPropertyName("l")]
        public double Low { get; set; }

        [JsonPropertyName("c")]
        public double Close { get; set; }

        [JsonPropertyName("v")]
        public double Volume { get; set; }

        [JsonPropertyName("t")]
        public long T { get; set; }

        public DateTime Timestamp => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(T);
    }
}
