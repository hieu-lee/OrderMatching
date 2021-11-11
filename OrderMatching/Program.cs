using System;
using System.Collections.Generic;

namespace OrderMatching
{
    class Order : IComparable<Order>
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
    }

    class Transaction
    {
        public string Id = Guid.NewGuid().ToString();
        public DateTime Time = DateTime.Now.ToUniversalTime();
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public double Price { get; set; }
        public uint Quantity { get; set; }
        public string StockId { get; set; }
    }

    class OrderExecutionResult
    {
        public Dictionary<string, double> BalanceChanges { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<Order> BuyOrdersLeft { get; set; }
        public List<Order> SellOrdersLeft { get; set; }
    }

    class Program
    {
        // Matching algorithm for market maker with N buy orders and M sell orders
        // Time Complexity: O(max(NlogN, MlogM)) (O(NlogM) with sorted inputs)
        // Space Complexity: O(max(N,M))
        static OrderExecutionResult OrderExecution(List<Order> BuyOrders, List<Order> SellOrders)
        {
            BuyOrders.Sort();
            SellOrders.Sort();
            var n = BuyOrders.Count;
            var m = SellOrders.Count;
            var i = n - 1;
            var Right = m - 1;
            var Res = new Dictionary<string, double>();
            var Transactions = new List<Transaction>();
            var Found = true;
            var BuyOrdersLeft = new List<Order>();
            var SellOrdersLeft = new List<Order>();
            while (Found && i >= 0)
            {
                var l = 0;
                var r = Right;
                Found = false;
                while (l <= r)
                {
                    var mid = l + ((r - l) >> 1);
                    if ((SellOrders[mid].Price <= BuyOrders[i].Price) && (mid == Right || SellOrders[mid + 1].Price > BuyOrders[i].Price))
                    {
                        var q = Math.Min(SellOrders[mid].Quantity, BuyOrders[i].Quantity);
                        SellOrders[mid].Quantity -= q;
                        BuyOrders[i].Quantity -= q;
                        var c = q * SellOrders[mid].Price;
                        if (Res.ContainsKey(SellOrders[mid].CustomerId))
                        {
                            Res[SellOrders[mid].CustomerId] += c;
                        }
                        else
                        {
                            Res[SellOrders[mid].CustomerId] = c;
                        }
                        if (Res.ContainsKey(BuyOrders[i].CustomerId))
                        {
                            Res[BuyOrders[i].CustomerId] -= c;
                        }
                        else
                        {
                            Res[BuyOrders[i].CustomerId] = -c;
                        }
                        Transactions.Add(new()
                        {
                            BuyerId = BuyOrders[i].CustomerId,
                            SellerId = SellOrders[mid].CustomerId,
                            Price = SellOrders[mid].Price, Quantity = q
                        });
                        if (SellOrders[mid].Quantity == 0)
                        {
                            Right = mid - 1;
                        }
                        else
                        {
                            i--;
                        }
                        Found = true;
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
            }

            for (int j = 0; j < n; j++)
            {
                if (BuyOrders[j].Quantity > 0)
                {
                    BuyOrdersLeft.Add(BuyOrders[j]);
                }
                else
                {
                    break;
                }
            }

            for (int j = 0; j < m; j++)
            {
                if (SellOrders[j].Quantity > 0)
                {
                    SellOrdersLeft.Add(SellOrders[j]);
                }
            }

            return new()
            {
                BalanceChanges = Res,
                Transactions = Transactions,
                BuyOrdersLeft = BuyOrdersLeft,
                SellOrdersLeft = SellOrdersLeft
            };
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
