using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Com.Exchange.Api.Models
{
    public class ApiConfig
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string ApiEndpointOverride { get; set; }
    }
}
