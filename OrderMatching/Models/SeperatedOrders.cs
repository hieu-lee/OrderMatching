namespace OrderMatching.Models
{
    public class SeperatedOrders
    {
        public List<Order> BuyOrders { get; set; } = new();
        public List<Order> SellOrders { get; set; } = new();
    }
}
