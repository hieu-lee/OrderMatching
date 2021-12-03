using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using OrderMatching.Models;
using System;
using System.Collections.Generic;

namespace OrderMatching
{
    [MemoryDiagnoser]
    public class Program
    {
        // Matching algorithm for market maker with N buy orders and M sell orders
        // Time Complexity: O(max(NlogN, MlogM)) (O(M) with sorted inputs)
        // Space Complexity: O(max(N,M))
        List<Order> BuyOrdersTest;
        List<Order> SellOrdersTest;

        public Program()
        {
            var m = 1000000;
            var n = 1000000;
            var rand = new Random();
            BuyOrdersTest = new();
            SellOrdersTest = new();
            for (int i = 0; i < m; i++)
            {
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
                    StockId = "BTC",
                    Price = Math.Round(p * 200, 2),
                    Quantity = (uint)Math.Ceiling(q * 20)
                };
                BuyOrdersTest.Add(order);
            }
            for (int i = 0; i < n; i++)
            {
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
                    StockId = "BTC",
                    Price = Math.Round(p * 200, 2),
                    Quantity = (uint)Math.Ceiling(q * 20)
                };
                SellOrdersTest.Add(order);
            }
            BuyOrdersTest.Sort();
            SellOrdersTest.Sort();
        }

        static void TestAlter()
        {
            var rand = new Random();
            var BuyerOrders = new List<Order>();
            var SellerOrders = new List<Order>();
            for (int i = 0; i < 20; i++)
            {
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
                    CustomerId = $"Hieu{i}",
                    StockId = "BTC",
                    Price = Math.Round(p * 200, 2),
                    Quantity = (uint)Math.Ceiling(q * 20)
                };
                BuyerOrders.Add(order);
            }
            for (int i = 0; i < 20; i++)
            {
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
                    StockId = "BTC",
                    Price = Math.Round(p * 200, 2),
                    Quantity = (uint)Math.Ceiling(q * 20)
                };
                SellerOrders.Add(order);
            }

            foreach (var order in BuyerOrders)
            {
                Console.WriteLine(order);
            }

            Console.WriteLine("END BUY");

            foreach (var order in SellerOrders)
            {
                Console.WriteLine(order);
            }

            Console.WriteLine("END SELL");

            var res = MatchingAlgorithms.MatchingAlgorithm1(BuyerOrders, SellerOrders);
            foreach (var t in res.Transactions)
            {
                Console.WriteLine(t);
            }

            Console.WriteLine("END TRANSACTIONS");

            foreach (var order in res.BuyOrdersLeft)
            {
                Console.WriteLine(order);
            }

            Console.WriteLine("END BUYLEFT");

            foreach (var order in res.SellOrdersLeft)
            {
                Console.WriteLine(order);
            }

            Console.WriteLine("END SELLLEFT");
        }

        static void Test()
        {
            var rand = new Random();
            var BuyerOrders = new List<Order>();
            var SellerOrders = new List<Order>();
            for (int i = 0; i < 20; i++)
            {
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
                    StockId = "BTC",
                    Price = Math.Round(p * 200, 2),
                    Quantity = (uint)Math.Ceiling(q * 20)
                };
                BuyerOrders.Add(order);
            }
            for (int i = 0; i < 20; i++)
            {
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
                    StockId = "BTC",
                    Price = Math.Round(p * 200, 2),
                    Quantity = (uint)Math.Ceiling(q * 20)
                };
                SellerOrders.Add(order);
            }

            foreach (var order in BuyerOrders)
            {
                Console.WriteLine(order);
            }

            Console.WriteLine("END BUY");

            foreach (var order in SellerOrders)
            {
                Console.WriteLine(order);
            }

            Console.WriteLine("END SELL");

            var res = MatchingAlgorithms.MatchingAlgorithm2(BuyerOrders, SellerOrders);
            foreach (var t in res.Transactions)
            {
                Console.WriteLine(t);
            }

            Console.WriteLine("END TRANSACTIONS");

            foreach (var order in res.BuyOrdersLeft)
            {
                Console.WriteLine(order);
            }

            Console.WriteLine("END BUYLEFT");

            foreach (var order in res.SellOrdersLeft)
            {
                Console.WriteLine(order);
            }

            Console.WriteLine("END SELLLEFT");
        }

        [Benchmark]
        public void TestPerformanceMatchingAlgorithm1()
        {
            MatchingAlgorithms.MatchingAlgorithm1(BuyOrdersTest, SellOrdersTest);
        }

        [Benchmark]
        public void TestPerformanceMatchingAlgorithm2()
        {
            MatchingAlgorithms.MatchingAlgorithm2(BuyOrdersTest, SellOrdersTest);
        }

        [Benchmark]
        public void TestPerformanceMatchingAlgorithm1Sorted()
        {
            MatchingAlgorithms.MatchingAlgorithm1(BuyOrdersTest, SellOrdersTest, true);
        }

        [Benchmark]
        public void TestPerformanceMatchingAlgorithm2Sorted()
        {
            MatchingAlgorithms.MatchingAlgorithm2(BuyOrdersTest, SellOrdersTest, true);
        }

        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Program>();
            //TestAlter();
        }
    }
}
