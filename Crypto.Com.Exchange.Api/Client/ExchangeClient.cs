using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using O9d.Json.Formatting;

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
                PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy()
            };
        }


        protected string GetSignature<T>(BaseRequest<T> request)
        {
            var paramString = string.Empty;

            if (request.Params != null)
            {
                var paramsDictionary = request.Params.GetType()
                                                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                                .OrderBy(p => p.Name)
                                                .ToDictionary(prop => prop.Name, prop => (string?)prop.GetValue(request.Params, null));

                // Ensure the params are alphabetically sorted by key
                paramString = string.Join("", paramsDictionary.Keys.OrderBy(key => key).Select(key => key + paramsDictionary[key]));
            }

            var SigPayload = Encoding.UTF8.GetBytes(request.Method.ToString() + request.Id + _apiKey + paramString + request.Nonce);
            var hash = new HMACSHA256(_apiSecretBytes);
            var ComputedHash = hash.ComputeHash(SigPayload);

            return BitConverter.ToString(ComputedHash);
        }


        protected async Task<T> GetAsync<T>(string requestUri)
        {
            var res = await _httpClient.GetAsync(requestUri);
            var content = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        // TODO - Add request throttling
    }
}
