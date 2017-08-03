using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NKStrata.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace NKStrata.Tests
{
    [TestClass]
    public class ShoppingCartTest
    {
        private IShoppingService _shoppingService;
        private ShoppingCart _shoppingCart;

        [TestInitialize]
        public void TestSetup()
        {
            _shoppingService = new ShoppingService();
            CreateTestData();

            Customer customer = _shoppingService.GetCustomerByName("John Smith");
            _shoppingCart = customer.ShoppingCart;
            _shoppingCart.CustomerName = customer.Name;
        }
        
        [TestMethod]
        public void Add_To_ShoppingCart()
        {
            Product product = new Product() { ProductCode = "B1", Description = "Banana", Quantity = 2, UnitPrice = 7.65M };
            _shoppingCart.AddProduct(product);
            Assert.AreEqual(2, _shoppingCart.Products.Count);
        }

        [TestMethod]
        public void Find_Valid_Product_By_Code()
        {
            string productCode = "A1";
            var product = _shoppingCart.GetProductByCode(productCode);
            Assert.IsNotNull(product);
        }

        [TestMethod]
        public void Find_Invalid_Product_By_Code()
        {
            string productCode = "apple";
            var product = _shoppingCart.GetProductByCode(productCode);
            Assert.IsNull(product);
        }

        [TestMethod]
        public void Remove_Product_From_Cart()
        {
            string productCode = "A1";
            var product = _shoppingCart.GetProductByCode(productCode);

            _shoppingCart.RemoveProduct(product);
            product = _shoppingCart.GetProductByCode(productCode);

            Assert.IsNull(product);
        }

        [TestMethod]
        public void Edit_Product_In_Cart()
        {
            string productCode = "A1";
            var product = _shoppingCart.GetProductByCode(productCode);
            product.Quantity = 2;

            _shoppingCart.EditProduct(product);
            var updatedProduct = _shoppingCart.GetProductByCode(productCode);

            Assert.AreEqual(2, updatedProduct.Quantity);
        }

        [TestMethod]
        public void Disallow_Quantity_Less_Than_0()
        {
            var results = new List<ValidationResult>();
            Product product = new Product() { ProductCode = "M1", Description = "Melon", Quantity = -2, UnitPrice = 6.66M };
            var validationContext = new ValidationContext(product);
            var success = Validator.TryValidateObject(product, validationContext, results, true);

            Assert.AreEqual(false, success);
        }

        [TestMethod]
        public void Add_Another_Existing_Product_Same_Item_Count()
        {
            Product product = new Product() { ProductCode = "B1", Description = "Banana", Quantity = 2, UnitPrice = 7.65M };
            _shoppingCart.AddProduct(product);
            _shoppingCart.AddProduct(product);
            Assert.AreEqual(2, _shoppingCart.Products.Count);
        }

        [TestMethod]
        public void Add_Another_Existing_Product_Quantity_Increases()
        {
            Product product = new Product() { ProductCode = "B1", Description = "Banana", Quantity = 2, UnitPrice = 7.65M };
            _shoppingCart.AddProduct(product);
            _shoppingCart.AddProduct(product);
            Assert.AreEqual(4, _shoppingCart.Products.Find(p => p.ProductCode == "B1").Quantity);
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
            Customer customer = new Customer() { Name = "John Smith", Address = "London", Email = "jsmith@hotmail.com", ShoppingCart = shoppingCart };
            _shoppingService.Customers.Add(customer);
        }
    }
}
