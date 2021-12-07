using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Com.Exchange.Api.Base
{
    public class BaseRequest<T>
    {
        public int Id { get; set; }
        public HttpMethod Method { get; set; } = HttpMethod.Get;
        public T? Params { get; set; }
        public string? ApiKey { get; set; }
        public string? Sig { get; set; }
        public long Nonce { get; set; }
    }
}
