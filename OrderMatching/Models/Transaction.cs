namespace OrderMatching.Models
{
    public class Transaction
    {
        public string Id = Guid.NewGuid().ToString();
        public DateTime Time = DateTime.Now.ToUniversalTime();
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public double Price { get; set; }
        public uint Quantity { get; set; }
        public string StockId { get; set; }
        public override string ToString()
        {
            return $"{BuyerId} buy {Quantity} BTC at price {Price} from {SellerId}";
        }
    }
}
