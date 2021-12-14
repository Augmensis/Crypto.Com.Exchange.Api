using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Com.Exchange.Api.Base
{
    public class BaseRequest
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public Dictionary<string, string> Params { get; set; } = new Dictionary<string, string>();
        public string? ApiKey { get; set; }
        public string? Sig { get; set; }
        public long Nonce { get; set; }
    }
}
