using Crypto.Com.Exchange.Api.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Com.Exchange.Api.Common.Models
{
    public class Instrument
    {
        public string InstrumentName { get; set; }
        public string QuoteCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public int PriceDecimals { get; set; }
        public int QuantityDecimals { get; set; }
        public bool MarginTradingEnabled { get; set; }
        public bool MarginTradingEnabled5X { get; set; }
        public bool MarginTradingEnabled10X { get; set; }
        public string MaxQuantity { get; set; }
        public string MinQuantity { get; set; }
    }
}
