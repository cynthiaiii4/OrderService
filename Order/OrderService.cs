using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace Order
{
    public class OrderService
    {
        private const string FilePath = @"C:\temp\xxx.csv";

        public ImmutableDictionary<string, int> GetTopBooksByOrders()
        {
            var orders = this.GetOrders();

            // only get orders of book
            var ordersOfBook =
                orders
                    .Where(x => x.Type == "Book")
                    .GroupBy(g => new { ProductName = g.ProductName })
                    .Select(s => new { ProductName = s.Key.ProductName, Price = s.Sum(x => x.Price) })
                    .OrderByDescending(o => o.Price)
                    .ToImmutableDictionary(d => d.ProductName, d => d.Price);

            return ordersOfBook;
        }

        
        protected virtual IEnumerable<Order> GetOrders()
        {
            // parse csv file to get orders
            var result = new List<Order>();

            // directly depend on File I/O
            using (var sr = new StreamReader(FilePath, Encoding.UTF8))
            {
                var rowCount = 0;

                while (sr.Peek() > -1)
                {
                    rowCount++;

                    var content = sr.ReadLine();

                    // Skip CSV header line
                    if (rowCount > 1)
                    {
                        var line = content?.Trim().Split(',');

                        result.Add(this.Mapping(line));
                    }
                }
            }

            return result;
        }

        private Order Mapping(IReadOnlyList<string> line)
        {
            var result = new Order
            {
                ProductName = line[0],
                Type = line[1],
                Price = Convert.ToInt32(line[2]),
                CustomerName = line[3]
            };

            return result;
        }
    }

    public class Order
    {
        public string ProductName { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public string CustomerName { get; set; }
    }
}
