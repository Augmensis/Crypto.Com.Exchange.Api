using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Com.Exchange.Api.Consts
{
    public class Endpoints
    {
        /// <summary>
        /// Production REST API V2 Endpoint
        /// </summary>
        public const string PRD_REST_V2_ENDPOINT_URL = "https://api.crypto.com/v2/";

        /// <summary>
        /// Production REST API V1 Endpoint
        /// </summary>
        public const string PRD_REST_V1_ENDPOINT_URL = "https://api.crypto.com/v1/";

        /// <summary>
        /// Sandbox REST API V2 Endpoint
        /// </summary>
        public const string SBX_REST_V2_ENDPOINT_URL = "https://api.crypto.com/v2/";

        /// <summary>
        /// Sandbox REST API V1 Endpoint
        /// </summary>
        public const string SBX_REST_V1_ENDPOINT_URL = "https://api.crypto.com/v1/";


        public const string SBX_WSS_V2_USER_ENDPOINT_URL = "wss://uat-stream.3ona.co/v2/user";
        public const string SBX_WSS_V2_MARKET_ENDPOINT_URL = "wss://uat-stream.3ona.co/v2/market";

        public const string PRD_WSS_V2_USER_ENDPOINT_URL = "wss://stream.crypto.com/v2/user";
        public const string PRD_WSS_V2_MARKET_ENDPOINT_URL = "wss://stream.crypto.com/v2/market";


    }
}
