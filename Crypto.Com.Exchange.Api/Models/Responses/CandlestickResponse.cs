using Crypto.Com.Exchange.Api.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Com.Exchange.Api.Models.Responses
{
    public class CandlestickResponse
    {
        public string InstrumentName { get; set; }
        public string Interval { get; set; }
        public List<Candlestick> Data { get; set; }
    }
}
