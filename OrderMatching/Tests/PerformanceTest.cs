namespace OrderMatching.Tests
{
    [MemoryDiagnoser]
    public class PerformanceTest
    {
        List<Order> BuyOrdersTest;
        List<Order> SellOrdersTest;
        List<Order> BuyOrdersTestSort;
        List<Order> SellOrdersTestSort;

        public PerformanceTest()
        {
            var m = 150000;
            var n = 150000;
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
            Order[] tmp1 = new Order[m];
            Order[] tmp2 = new Order[n];
            BuyOrdersTest.CopyTo(tmp1, 0);
            SellOrdersTest.CopyTo(tmp2, 0);
            BuyOrdersTestSort = new(tmp1);
            SellOrdersTestSort = new(tmp2);
        }

        [Benchmark]
        public void TheAlgorithm()
        {
            MatchingAlgorithms.ExecuteOrders(BuyOrdersTest, SellOrdersTest);
        }

        [Benchmark]
        public void SortingPartOfTheAlgorithm()
        {
            BuyOrdersTestSort.Sort();
            SellOrdersTestSort.Sort();
        }

        public static void StartBenchmark()
        {
            Console.WriteLine("The program will benchmark the algorithm matching 300,000 orders (including buy and sell)");
            Console.WriteLine("Do you want to start? [Y/N]");
            var answer = Console.ReadLine();
            if (answer.ToUpper().Trim() == "Y")
            {
                var _ = BenchmarkRunner.Run<PerformanceTest>();
            }
        }
    }
}
