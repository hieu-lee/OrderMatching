namespace OrderMatching.Models
{
    public class Order : IComparable<Order>
    {
        public string Id = Guid.NewGuid().ToString();
        public OrderType OrderType { get; set; }
        public string StockId { get; set; }
        public string CustomerId { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Order;
            return Id == other.Id;
        }

        public int CompareTo(Order other)
        {
            return Price.CompareTo(other.Price);
        }

        public override string ToString()
        {
            if (OrderType == OrderType.BUY)
            {
                return $"{CustomerId} wants to buy {Quantity} {StockId} at price {Price}";
            }
            return $"{CustomerId} wants to sell {Quantity} {StockId} at price {Price}";
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
