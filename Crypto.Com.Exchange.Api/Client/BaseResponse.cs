using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Com.Exchange.Api.Base
{
    public class BaseResponse<T>
    {
        /// <summary>
        /// Original request identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Method invoked
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Result object
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// 0 for success, see below for full list
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// (optional) For server or error messages
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// (optional) Original request as a string, for error cases
        /// </summary>
        public string Original { get; set; }
    }
}
