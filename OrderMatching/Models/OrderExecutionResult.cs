using System.Collections.Generic;

namespace OrderMatching.Models
{
    public class OrderExecutionResult
    {
        public Dictionary<string, double> BalanceChanges { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<Order> BuyOrdersLeft { get; set; }
        public List<Order> SellOrdersLeft { get; set; }
    }
}
