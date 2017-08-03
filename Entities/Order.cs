using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NKStrata.Models
{
    public class Order
    {
        public Order()
        {
            OrderLines = new List<OrderLine>();
        }

        public int ID { get; set; }
        public string CustomerName { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }
        public decimal Amount { get; set; }

        public List<OrderLine> OrderLines { get; set; }

        public void AddProductToOrder(Product product)
        {
            OrderLine orderLine = new OrderLine() { OrderID = this.ID, Product = product };
            OrderLines.Add(orderLine);            
        }

        public void AddProductCollectionToOrder(List<Product> products)
        {
            products.ForEach(p => AddProductToOrder(p));            
        }
    }
}