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
        // Time Complexity: O(max(NlogN, MlogM)) (O(NlogM) with sorted inputs)
        // Space Complexity: O(max(N,M))
        static List<Order> BuyOrdersTest;
        static List<Order> SellOrdersTest;

        static OrderExecutionResult OrderExecution(List<Order> BuyOrders, List<Order> SellOrders)
        {
            var StockId = BuyOrders[0].StockId;
            BuyOrders.Sort();
            SellOrders.Sort();
            var n = BuyOrders.Count;
            var m = SellOrders.Count;
            var i = n - 1;
            var j = m - 1;
            var Found = true;
            var Transactions = new List<Transaction>();
            var BalanceChanges = new Dictionary<string, double>();
            while (Found && i >= 0 && j >= 0)
            {
                if (SellOrders[j].Price > BuyOrders[i].Price)
                {
                    Found = false;
                    var l = 0;
                    var r = j - 1;
                    while (l <= r)
                    {
                        var mid = l + ((r - l) >> 1);
                        if (SellOrders[mid].Price <= BuyOrders[i].Price && SellOrders[mid + 1].Price > BuyOrders[i].Price)
                        {
                            Found = true;
                            j = mid;
                            break;
                        }
                        else if (SellOrders[mid].Price <= BuyOrders[i].Price)
                        {
                            l = mid + 1;
                        }
                        else
                        {
                            r = mid - 1;
                        }
                    }
                    if (!Found)
                    {
                        j = -1;
                    }
                }
                if (j == -1)
                {
                    break;
                }
                else
                {
                    while (BuyOrders[i].Quantity > 0 && j >= 0)
                    {
                        var q = (BuyOrders[i].Quantity <= SellOrders[j].Quantity)?BuyOrders[i].Quantity : SellOrders[j].Quantity;
                        var p = SellOrders[j].Price;
                        BuyOrders[i].Quantity -= q;
                        SellOrders[j].Quantity -= q;
                        Transactions.Add(new()
                        {
                            BuyerId = BuyOrders[i].CustomerId,
                            SellerId = SellOrders[j].CustomerId,
                            Price = p,
                            Quantity = q,
                            StockId = StockId
                        });
                        if (!BalanceChanges.ContainsKey(BuyOrders[i].CustomerId))
                        {
                            BalanceChanges[BuyOrders[i].CustomerId] = 0;
                        }
                        if (!BalanceChanges.ContainsKey(SellOrders[j].CustomerId))
                        {
                            BalanceChanges[SellOrders[j].CustomerId] = 0;
                        }
                        var c = Math.Round(p * q, 2);
                        BalanceChanges[BuyOrders[i].CustomerId] -= c;
                        BalanceChanges[SellOrders[j].CustomerId] += c;
                        if (SellOrders[j].Quantity == 0)
                        {
                            j--;
                        }
                    }
                    i--;
                }
            }
            var BuyOrderLeft = new List<Order>();
            var SellOrderLeft = new List<Order>();
            for (int k = 0; k < i + 1; k++)
            {
                BuyOrderLeft.Add(BuyOrders[k]);
            }
            foreach (var order in SellOrders)
            {
                if (order.Quantity > 0)
                {
                    SellOrderLeft.Add(order);
                }
            }
            return new()
            {
                BalanceChanges = BalanceChanges,
                BuyOrdersLeft = BuyOrderLeft,
                SellOrdersLeft = SellOrderLeft,
                Transactions = Transactions
            };
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

            var res = OrderExecution(BuyerOrders, SellerOrders);
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
        public void TestPerformance()
        {
            var m = 50000;
            var n = 50000;
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
                    CustomerId = $"Plh{i}",
                    StockId = "BTC",
                    Price = Math.Round(p * 200, 2),
                    Quantity = (uint)Math.Ceiling(q * 20)
                };
                SellOrdersTest.Add(order);
            }
            OrderExecution(BuyOrdersTest, SellOrdersTest);
        }

        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Program>();
        }
    }
}
