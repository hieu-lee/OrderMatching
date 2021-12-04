namespace OrderMatching.Tests
{
    [MemoryDiagnoser]
    public class PerformanceTest
    {
        List<Order> BuyOrdersTest;
        List<Order> SellOrdersTest;

        public PerformanceTest()
        {
            var m = 100000;
            var n = 100000;
            var rand = new Random();
            BuyOrdersTest = new();
            SellOrdersTest = new();
            for (int i = 0; i < m; i++)
            {
                var j = rand.Next(10);
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
                var j = rand.Next(10);
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

        [Benchmark]
        public void RunTest()
        {
            MatchingAlgorithms.ExecuteOrders(BuyOrdersTest, SellOrdersTest);
        }

        public void TestSort()
        {
            BuyOrdersTest.Sort();
            SellOrdersTest.Sort();
        }

        public static void StartBenchmark()
        {
            Console.WriteLine("The program will benchmark the algorithm matching 200,000 orders (including buy and sell)");
            Console.WriteLine("Do you want to start? [Y/N]");
            var answer = Console.ReadLine();
            if (answer.ToUpper().Trim() == "Y")
            {
                var _ = BenchmarkRunner.Run<PerformanceTest>();
            }
        }
    }
}
