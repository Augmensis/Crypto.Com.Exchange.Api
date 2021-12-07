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
        public async Task<BaseResponse<InstrumentsReponse>> GetInstruments()
        {
            return await GetAsync<BaseResponse<InstrumentsReponse>>("public/get-instruments");
        }

        public async Task<BaseResponse<BookResponse>> GetBook(string instrumentName, int? depth)
        {
            if(string.IsNullOrEmpty(instrumentName) || string.IsNullOrWhiteSpace(instrumentName))
                throw new ArgumentNullException(nameof(instrumentName));

            if(depth > 150) 
                throw new ArgumentOutOfRangeException(nameof(depth), "Value exceeds Max depth value of 150");

            var builder = new StringBuilder("public/get-book");
            builder.Append($"?instrument_name={instrumentName}");
            if (depth != null)
                builder.Append($"&depth={depth}");

            return await GetAsync<BaseResponse<BookResponse>>(builder.ToString());
        }

        public async Task<BaseResponse<CandlestickResponse>> GetCandlestick(string instrumentName, enCandlestickPeriod period)
        {
            if (string.IsNullOrEmpty(instrumentName) || string.IsNullOrWhiteSpace(instrumentName))
                throw new ArgumentNullException(nameof(instrumentName));

            return await GetAsync<BaseResponse<CandlestickResponse>>($"public/get-candlestick?instrument_name={instrumentName}&timeframe={period.GetDescription()}");
        }
    }
}
