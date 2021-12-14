using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.WebSockets;
using System.Text.Json;
using O9d.Json.Formatting;
using System.Net.Http.Json;

namespace Crypto.Com.Exchange.Api.Base
{
    public partial class ExchangeClient
    {
        private string _apiKey;
        private string _apiSecret;
        private byte[] _apiSecretBytes;
        private string _apiEndpoint;
        private HttpClient _httpClient;
        private JsonSerializerOptions _jsonOptions;
        private Dictionary<string, (DateTime, decimal)> _throttle { get; set; }

        public ExchangeClient(string apiKey, string apiSecret, string endpoint)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _apiEndpoint = endpoint;            
            _apiSecretBytes = Encoding.UTF8.GetBytes(_apiSecret);
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_apiEndpoint);
            _jsonOptions = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy(),
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };
            InitThrottleDictionary();
        }


        protected string GetSignature(string requestUri, BaseRequest request)
        {
            var paramString = "";

            if (request != null && request.Params.Any())
            {
                // Ensure the params are alphabetically sorted by key
                paramString = string.Join("", request.Params.Keys.OrderBy(key => key).Select(key => key + request.Params[key]));
            }

            var sigPayload = Encoding.UTF8.GetBytes(request.Method + request.Id + _apiKey + paramString + request.Nonce);
            var hash = new HMACSHA256(_apiSecretBytes);
            var computedHash = hash.ComputeHash(sigPayload);
            return BitConverter.ToString(computedHash).Replace("-", "");
        }


        protected async Task<T> GetAsync<T>(string requestUri, string? requestParams = null)
        {
            ThrottleRequest(requestUri);

            if (!string.IsNullOrEmpty(requestParams))
                requestUri += requestParams;

            var res = await _httpClient.GetAsync(requestUri);
            var content = await res.Content.ReadAsStreamAsync();
            var baseResponse = await JsonSerializer.DeserializeAsync<BaseResponse<T>>(content, _jsonOptions);

            HandleResponseCodes(baseResponse);

            return baseResponse.Result;
        }

        protected async Task<T> PostAsync<T>(string requestUri, BaseRequest? request = null)
        {
            ThrottleRequest(requestUri);

            if(request == null)
            {
                request = new BaseRequest
                {
                    Method = requestUri
                };
            }

            request.ApiKey = _apiKey;
            request.Nonce = GetNonce();
            request.Sig = GetSignature(requestUri, request);

            var res = await _httpClient.PostAsync(requestUri, JsonContent.Create(request, null, _jsonOptions));
            var content = await res.Content.ReadAsStringAsync();
            var baseResponse = JsonSerializer.Deserialize <BaseResponse<T>>(content, _jsonOptions);

            HandleResponseCodes(baseResponse);

            return baseResponse.Result;
        }

        public static long GetNonce()
        {
            return (long)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalMilliseconds - 5;
        }

        private void InitThrottleDictionary()
        {
            // Endpoint, Last Requested, ms between calls
            _throttle = new Dictionary<string, (DateTime, decimal)>()
            {
                { "public/get-book", (DateTime.MinValue, 10) },
                { "public/get-ticker", (DateTime.MinValue, 10) },
                { "public/get-trades", (DateTime.MinValue, 10) },

                { "private/create-order", (DateTime.MinValue, 100/15) },
                { "private/cancel-order", (DateTime.MinValue, 100/15) },
                { "private/cancel-all-orders", (DateTime.MinValue, 100/15) },
                { "private/margin/create-order", (DateTime.MinValue, 100/15) },
                { "private/margin/cancel-order", (DateTime.MinValue, 100/15) },
                { "private/margin/cancel-all-orders", (DateTime.MinValue, 100/15) },

                { "private/get-order-detail", (DateTime.MinValue, 100/30) },
                { "private/margin/get-order-detail", (DateTime.MinValue, 100/30) },

                { "private/get-trades", (DateTime.MinValue, 1000) },
                { "private/margin/get-trades", (DateTime.MinValue, 1000) },

                { "private/get-order-history", (DateTime.MinValue, 1000) },
                { "private/margin/get-order-history", (DateTime.MinValue, 1000) },

                // NOTE: every endpoint should be covered explicitly for maximum effectiveness.
                { "private", (DateTime.MinValue, 100/3) },
                { "public", (DateTime.MinValue, 10) },
            };
        }

        protected void ThrottleRequest(string requestUri)
        {
            var throttle = _throttle.ContainsKey(requestUri)
                                ? _throttle[requestUri]
                                : (requestUri.StartsWith("public")
                                    ? _throttle["public"]
                                    : _throttle["private"]);

            var nextAccess = throttle.Item1.AddMilliseconds((double)throttle.Item2);

            while (nextAccess > DateTime.UtcNow)
            { 
                Thread.Sleep(1); 
            }

            _throttle[requestUri] = (DateTime.UtcNow, throttle.Item2);
        }

        protected void HandleResponseCodes<T>(BaseResponse<T> response)
        {
            if (response == null)  throw new Exception($"response returned null");

            switch (response.Code)
            {
                case 0:
                    // Success
                    break;

                case 100001:
                    throw new Exception(response.ErrorMessage, new Exception("Server error, Likely malformed request. Check ContentType is JSON."));

                case 10001:
                    throw new Exception(response.Message, new Exception("Malformed Request"));

                case 10002:
                    throw new Exception(response.Message, new Exception("Not authenticated, or key/signature incorrect"));

                case 10003:
                    throw new Exception(response.Message, new Exception("IP address not whitelisted"));

                case 10004:
                    throw new Exception(response.Message, new Exception("Missing required fields"));

                case 10005:
                    throw new Exception(response.Message, new Exception("Disallowed based on user tier"));

                case 10006:
                    throw new Exception(response.Message, new Exception("Requests have exceeded rate limits"));

                case 10007:
                    throw new Exception(response.Message, new Exception("Nonce value differs by more than 30 seconds from server"));

                case 10008:
                    throw new Exception(response.Message, new Exception("Invalid method specified"));

                case 10009:
                    throw new Exception(response.Message, new Exception("Invalid date range"));

                case 20001:
                    throw new Exception(response.Message, new Exception("Duplicated record"));

                case 20002:
                    throw new Exception(response.Message, new Exception("Insufficient balance"));

                case 30003:
                    throw new Exception(response.Message, new Exception("Invalid instrument_name specified"));

                case 30004:
                    throw new Exception(response.Message, new Exception("Invalid side specified"));

                case 30005:
                    throw new Exception(response.Message, new Exception("Invalid type specified"));
               
                case 30006:
                    throw new Exception(response.Message, new Exception("Price is lower than the minimum"));
                
                case 30007:
                    throw new Exception(response.Message, new Exception("Price is higher than the maximum"));
                
                case 30008:
                    throw new Exception(response.Message, new Exception("Quantity is lower than the minimum"));
                
                case 30009:
                    throw new Exception(response.Message, new Exception("Quantity is higher than the maximum"));
                
                case 30010:
                    throw new Exception(response.Message, new Exception("Required argument is blank or missing"));

                case 30013:
                    throw new Exception(response.Message, new Exception("Too many decimal places for Price"));

                case 30014:
                    throw new Exception(response.Message, new Exception("Too many decimal places for Quantity"));

                case 30016:
                    throw new Exception(response.Message, new Exception("The notional amount is less than the minimum"));

                case 30017:
                    throw new Exception(response.Message, new Exception("The notional amount exceeds the maximum"));

                case 30023:
                    throw new Exception(response.Message, new Exception("Amount is lower than the minimum"));

                case 30024:
                    throw new Exception(response.Message, new Exception("Amount is higher than the maximum"));

                case 30025:
                    throw new Exception(response.Message, new Exception("Amount precision exceeds the maximum"));

                case 40001:
                    throw new Exception(response.Message, new Exception("Operation has failed due to your account's status. Please try again later."));

                case 40002:
                    throw new Exception(response.Message, new Exception("Transfer has failed due to holding an active loan. Please repay your loan and try again later."));

                case 40003:
                    throw new Exception(response.Message, new Exception("Currency is not same as loan currency of active loan"));

                case 40004:
                    throw new Exception(response.Message, new Exception("Only supporting full repayment of all margin loans"));

                case 40005:
                    throw new Exception(response.Message, new Exception("No active loan"));

                case 40006:
                    throw new Exception(response.Message, new Exception("Borrow has been suspended. Please try again later."));

                case 40007:
                    throw new Exception(response.Message, new Exception("Placing new order has been suspended. Please try again later."));

                case 50001:
                    throw new Exception(response.Message, new Exception("Please ensure your credit line is maintained and try again later."));

                default:
                    throw new Exception(response.Message, new Exception("Exception not recognised"));
            }
        }
    }
}
