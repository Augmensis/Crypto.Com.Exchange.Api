using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Com.Exchange.Api.Models
{
    public class OrderBook
    {

        // Price, Quantity, Number of orders
        public List<double[]> Bids { get; set; }
        public List<double[]> Asks { get; set; }
        public long T { get; set; }

        public List<BookPrice> BidPrices { get {
                var prices = new List<BookPrice>();
                foreach (var price in Bids)
                    prices.Add(new BookPrice(price[0], price[1], price[2], T));
                return prices;
            } 
        }

        public List<BookPrice> AskPrices
        {
            get
            {
                var prices = new List<BookPrice>();
                foreach (var price in Asks)
                    prices.Add(new BookPrice(price[0], price[1], price[2], T));
                return prices;
            }
        }
    }

    public class BookPrice 
    { 
        public BookPrice() { }
        public BookPrice(double price, double quantity, double orders, long timestamp) 
        {
            Price = price;
            Quantity = quantity;
            NumberOfOrders = orders;
            TimeStamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(timestamp);
        }

        public double Price { get; set; }
        public double Quantity { get; set; }
        public double NumberOfOrders { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
