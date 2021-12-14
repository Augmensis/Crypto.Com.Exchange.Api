using Crypto.Com.Exchange.Api.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http.Formatting;
using Crypto.Com.Exchange.Api.Models.Responses;
using System.Text.Json;
using Crypto.Com.Exchange.Api.Models;
using Crypto.Com.Exchange.Api.Consts;
using Crypto.Com.Exchange.Api.Extensions;

namespace Crypto.Com.Exchange.Api.Base
{
    public partial class ExchangeClient
    { 

        public async Task<SpotAccountsResponse> GetAccountSummary(string? currency = null)
        {
            var request = new BaseRequest
            {
                Method = "private/get-account-summary"
            };

            if (!string.IsNullOrEmpty(currency) && !string.IsNullOrWhiteSpace(currency))
                request.Params.Add("currency", currency);

            return await PostAsync<SpotAccountsResponse>("private/get-account-summary", request);
        }

        
    }
}
