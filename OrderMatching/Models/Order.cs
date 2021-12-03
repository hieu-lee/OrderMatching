using System;

namespace OrderMatching.Models
{
    public class Order : IComparable<Order>
    {
        public string Id = Guid.NewGuid().ToString();
        public string StockId { get; set; }
        public string CustomerId { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }

        public int CompareTo(Order other)
        {
            return Price.CompareTo(other.Price);
        }

        public override string ToString()
        {
            return $"{CustomerId} wants to buy/sell {Quantity} {StockId} at price {Price}";
        }
    }
}
