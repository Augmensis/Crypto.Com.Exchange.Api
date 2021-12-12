using Crypto.Com.Exchange.Api.Base;
using Crypto.Com.Exchange.Api.Consts;
using Crypto.Com.Exchange.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = Host.CreateDefaultBuilder(args)
               .ConfigureHostConfiguration(opt => 
               {
                   opt.AddUserSecrets("035e018d-ffc0-40cb-aafe-4ace55ad7eb7", true);     
               })
               .ConfigureServices(svc => { 
                    // Add your services, etc here                    
               })
               .Build();

// Get ApiConfig from user secrets or appsettings.json (overrides user secrets)
IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
var apiConfig = config.GetSection("ApiConfig").Get<ApiConfig>();

// Initialise the client
var client = new ExchangeClient(apiConfig.ApiKey, apiConfig.ApiSecret, Endpoints.SBX_REST_V2_ENDPOINT_URL);

// Get available instruments
var res = await client.GetInstruments();

foreach (var coinPair in res.Instruments.Take(5))
{
    Console.WriteLine($"{coinPair.InstrumentName} {coinPair.QuoteCurrency}/{coinPair.BaseCurrency}");

    // Get the current 5 levels of this instrument's order book
    var bookRes = await client.GetBook(coinPair.InstrumentName, 5);
    var minBid = bookRes.Book.BidPrices.MinBy(x => x.Price);
    var maxAsk = bookRes.Book.AskPrices.MinBy(x => x.Price);
    Console.WriteLine($"Lowest Bid {minBid.Price} @ {minBid.Quantity} # {minBid.NumberOfOrders} - {minBid.TimeStamp}");
    Console.WriteLine($"Highest Ask {maxAsk.Price} @ {maxAsk.Quantity} # {maxAsk.NumberOfOrders} - {maxAsk.TimeStamp}");

    // Get the candlesticks
    var candlesticks = await client.GetCandlestick(coinPair.InstrumentName, enCandlestickPeriod.OneHour);
    foreach (var data in candlesticks.Data.Take(5))
    {
        Console.WriteLine($"O:{data.Open} H:{data.High} L: C:{data.Close} T:{data.Timestamp}");
    }
}


// Get ticker
var tickers = await client.GetTicker();
foreach (var ticker in tickers.Data.Take(5))
{
    Console.WriteLine($"{ticker.InstrumentName}: BID:{ticker.Bid} ASK:{ticker.Ask}");
}

// Get trades
var trades = await client.GetTrades();
foreach (var trade in trades.Data.Take(5))
{
    Console.WriteLine($"{trade.InstrumentName}: {trade.TradePrice} @ {trade.TradeQuantity}");
}


await host.RunAsync();