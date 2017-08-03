using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NKStrata.Models;
using System.Linq;

namespace NKStrata.Tests
{
    [TestClass]
    public class OrderTest
    {
        private IShoppingService _shoppingService;
        private Customer _customer;

        [TestInitialize]
        public void TestSetup()
        {
            _shoppingService = new ShoppingService();
            CreateTestData();

            _customer = _shoppingService.GetCustomerByName("Tony Stark");
            _customer.ShoppingCart.CustomerName = _customer.Name;
        }

        [TestMethod]
        public void Add_OrderLine_To_New_Order()
        {
            Order order = new Order();
            Product product = new Product() { ProductCode = "P1", Description = "Pear", Quantity = 2, UnitPrice = 1.23M };
            order.AddProductToOrder(product);

            Assert.AreEqual(1, order.OrderLines.Count);
        }

        [TestMethod]
        public void Add_OrderLines_From_Collection()
        {
            Order order = new Order();
            order.AddProductCollectionToOrder(_customer.ShoppingCart.Products);

            Assert.AreEqual(2, order.OrderLines.Count);
        }

        [TestMethod]
        public void Create_Order_From_Shopping_Cart()
        {
            _shoppingService.CreateOrder(_customer, _customer.ShoppingCart);
            var customerOrder = _shoppingService.Orders.Where(o => o.CustomerName == _customer.Name).FirstOrDefault();

            Assert.AreEqual(2, customerOrder.OrderLines.Count);
        }

        [TestMethod]
        public void Order_Total_Increases()
        {
            _customer.ShoppingCart.Products.Clear();
            Product product = new Product() { ProductCode = "P1", Description = "Pear", Quantity = 2, UnitPrice = 1.23M };
            _customer.ShoppingCart.Products.Add(product);
            _shoppingService.CreateOrder(_customer, _customer.ShoppingCart);
            var customerOrder = _shoppingService.Orders.Where(o => o.CustomerName == _customer.Name).FirstOrDefault();

            Assert.AreEqual(2.46M, customerOrder.Amount);
        }

        [TestMethod]
        public void Order_Total_Increases_Multiple_Lines()
        {
            _customer.ShoppingCart.Products.Clear();
            Product product = new Product() { ProductCode = "P1", Description = "Pear", Quantity = 2, UnitPrice = 1.23M };
            _customer.ShoppingCart.Products.Add(product);            
            product = new Product() { ProductCode = "M1", Description = "Melon", Quantity = 1, UnitPrice = 2.00M };
            _customer.ShoppingCart.Products.Add(product);
            _shoppingService.CreateOrder(_customer, _customer.ShoppingCart);
            var customerOrder = _shoppingService.Orders.Where(o => o.CustomerName == _customer.Name).FirstOrDefault();

            Assert.AreEqual(4.46M, customerOrder.Amount);
        }

        [TestMethod]
        public void Order_Total_Changes_Customer_Type()
        {
            _customer.ShoppingCart.Products.Clear();
            Product product = new Product() { ProductCode = "P1", Description = "Pear", Quantity = 2, UnitPrice = 271.23M };
            _customer.ShoppingCart.Products.Add(product);
            _shoppingService.CreateOrder(_customer, _customer.ShoppingCart);

            Assert.AreEqual(CustomerType.Silver, _customer.CustomerType);
        }

        [TestMethod]
        public void Order_Total_With_Silver_Discount()
        {
            _customer.CustomerType = CustomerType.Silver;
            _customer.ShoppingCart.Products.Clear();
            Product product = new Product() { ProductCode = "P1", Description = "Pear", Quantity = 2, UnitPrice = 5.00M };
            _customer.ShoppingCart.Products.Add(product);
            _shoppingService.CreateOrder(_customer, _customer.ShoppingCart);
            var customerOrder = _shoppingService.Orders.Where(o => o.CustomerName == _customer.Name).FirstOrDefault();

            Assert.AreEqual(9.80M, customerOrder.Amount);
        }

        [TestCleanup]
        public void TestTearDown()
        {
            _shoppingService.Customers.Clear();
            _shoppingService.Orders.Clear();
        }

        private void CreateTestData()
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            Product product = new Product() { ProductCode = "A1", Description = "Apple", Quantity = 1, UnitPrice = 17.99M };
            shoppingCart.Products.Add(product);
            product = new Product() { ProductCode = "B1", Description = "Banana", Quantity = 2, UnitPrice = 7.65M };
            shoppingCart.Products.Add(product);
            Customer customer = new Customer() { Name = "Tony Stark", Address = "New York", Email = "ironman@aol.com", ShoppingCart = shoppingCart };
            _shoppingService.Customers.Add(customer);
        }
    }
}
