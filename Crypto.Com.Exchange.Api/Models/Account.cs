using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Com.Exchange.Api.Models
{
    public class Account
    {
        public double Balance { get; set; }
        public double Available { get; set; }
        public double Order { get; set; }
        public double Stake { get; set; }
        public string Currency { get; set; }
    }
}
