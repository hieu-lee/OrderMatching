namespace OrderMatching.Tests
{
    public class DemoTest
    {
        List<Order> BuyOrdersTest;
        List<Order> SellOrdersTest;

        public DemoTest()
        {
            var m = 27;
            var n = 27;
            var rand = new Random();
            BuyOrdersTest = new();
            SellOrdersTest = new();
            for (int i = 0; i < m; i++)
            {
                var j = rand.Next(3);
                var p = rand.NextDouble();
                while (p == 0)
                {
                    p = rand.NextDouble();
                }
                var q = rand.NextDouble();
                while (q == 0)
                {
                    q = rand.NextDouble();
                }
                var order = new Order()
                {
                    OrderType = OrderType.BUY,
                    CustomerId = $"Hieu{i}",
                    StockId = $"BTC{j}",
                    Price = Math.Round(p * 200, 2),
                    Quantity = (uint)Math.Ceiling(q * 20)
                };
                BuyOrdersTest.Add(order);
            }
            for (int i = 0; i < n; i++)
            {
                var j = rand.Next(3);
                var p = rand.NextDouble();
                while (p == 0)
                {
                    p = rand.NextDouble();
                }
                var q = rand.NextDouble();
                while (q == 0)
                {
                    q = rand.NextDouble();
                }
                var order = new Order()
                {
                    OrderType = OrderType.SELL,
                    CustomerId = $"Plh{i}",
                    StockId = $"BTC{j}",
                    Price = Math.Round(p * 200, 2),
                    Quantity = (uint)Math.Ceiling(q * 20)
                };
                SellOrdersTest.Add(order);
            }
        }

        public void RunDemo()
        {
            Console.WriteLine("Buy Orders:");
            foreach (var order in BuyOrdersTest)
            {
                Console.WriteLine(order);
            }
            Console.WriteLine("END BUY\n");

            Console.WriteLine("Sell Orders:");
            foreach (var order in SellOrdersTest)
            {
                Console.WriteLine(order);
            }
            Console.WriteLine("END SELL");
        
            var res = MatchingAlgorithms.ExecuteOrders(BuyOrdersTest, SellOrdersTest);

            Console.WriteLine("\nTransactions:");
            foreach (var t in res.Transactions)
            {
                Console.WriteLine(t);
            }
            Console.WriteLine("\nBalance Changes:");
            foreach (var customerId in res.BalanceChanges.Keys)
            {
                Console.WriteLine($"{customerId}: {res.BalanceChanges[customerId]}");
            }
            Console.WriteLine("\nBuy Orders Left:");
            foreach (var order in res.BuyOrdersLeft)
            {
                Console.WriteLine(order);
            }
            Console.WriteLine("\nSell Orders Left:");
            foreach (var order in res.SellOrdersLeft)
            {
                Console.WriteLine(order);
            }
        }
    }
}
