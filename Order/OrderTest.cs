using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Order
{
    
    public class OrderTest
    {
        private void SalesShouldBe(Dictionary<string, int> expect, ImmutableDictionary<string, int> source)
        {
            Assert.AreEqual(expect, source);
        }

        [Test]
        public void NormalData()
        {
            var salesReport = new StubOrderService();
            var orders = new List<Order>() {
                new Order{ProductName="A",Type="Book",Price=100,CustomerName="AA" },
                new Order{ProductName="B",Type="Book",Price=200,CustomerName="BB" },
                new Order{ProductName="C",Type="Book",Price=500,CustomerName="CC" }
            };
            salesReport.SetOrders(orders);
            var expect = new Dictionary<string,int>() {
                {"C",500},
                {"B",200},
                {"A",100}
            };
            SalesShouldBe(expect, salesReport.GetTopBooksByOrders());
        }

        [Test]
        public void SameProducts()
        {
            var salesReport = new StubOrderService();
            var orders = new List<Order>() {
                new Order{ProductName="A",Type="Book",Price=100,CustomerName="AA" },
                new Order{ProductName="A",Type="Book",Price=200,CustomerName="BB" },
                new Order{ProductName="C",Type="Book",Price=500,CustomerName="CC" }
            };
            salesReport.SetOrders(orders);
            var expect = new Dictionary<string, int>() {
                {"C",500},
                {"A",300}
                
            };
            SalesShouldBe(expect, salesReport.GetTopBooksByOrders());
        }

        [Test]
        public void WrongType()
        {
            var salesReport = new StubOrderService();
            var orders = new List<Order>() {
                new Order{ProductName="A",Type="Book",Price=100,CustomerName="AA" },
                new Order{ProductName="B",Type="CD",Price=200,CustomerName="BB" },
                new Order{ProductName="C",Type="Book",Price=500,CustomerName="CC" }
            };
            salesReport.SetOrders(orders);
            var expect = new Dictionary<string, int>() {
                {"C",500},
                {"A",100}
            };
            SalesShouldBe(expect, salesReport.GetTopBooksByOrders());
        }

        [Test]
        public void SameCustomers()
        {
            var salesReport = new StubOrderService();
            var orders = new List<Order>() {
                new Order{ProductName="A",Type="Book",Price=100,CustomerName="AA" },
                new Order{ProductName="B",Type="CD",Price=200,CustomerName="AA" },
                new Order{ProductName="C",Type="Book",Price=500,CustomerName="CC" }
            };
            salesReport.SetOrders(orders);
            var expect = new Dictionary<string, int>() {
                {"A",100},
                {"C",500}
            };
            SalesShouldBe(expect, salesReport.GetTopBooksByOrders());
        }

    }

    public class StubOrderService : OrderService {

        private List<Order> _order = new List<Order>();
        internal void SetOrders(List<Order> orders)
        {
            this._order = orders;
        }

        protected override IEnumerable<Order> GetOrders()
        {
            return this._order;
        }
    }


}