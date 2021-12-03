using OrderMatching.Models;
using System;
using System.Collections.Generic;

namespace OrderMatching
{
    public class MatchingAlgorithms
    {
        public static OrderExecutionResult MatchingAlgorithm1(List<Order> BuyOrders, List<Order> SellOrders)
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
                while (j >= 0 && SellOrders[j].Price > BuyOrders[i].Price)
                {
                    j--;
                }
                if (j == -1)
                {
                    break;
                }
                else
                {
                    while (BuyOrders[i].Quantity > 0 && j >= 0)
                    {
                        var q = (BuyOrders[i].Quantity <= SellOrders[j].Quantity) ? BuyOrders[i].Quantity : SellOrders[j].Quantity;
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


        public static OrderExecutionResult MatchingAlgorithm2(List<Order> BuyOrders, List<Order> SellOrders)
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
                        var q = (BuyOrders[i].Quantity <= SellOrders[j].Quantity) ? BuyOrders[i].Quantity : SellOrders[j].Quantity;
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
    }
}
