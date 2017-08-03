using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NKStrata;
using NKStrata.Controllers;
using NKStrata.Models;
using System.Web.Http.Results;

namespace NKStrata.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        private readonly IShoppingService _shoppingService;

        public ValuesControllerTest()
        {
            _shoppingService = new ShoppingService();
        }

        [TestInitialize]
        public void TestSetup()
        {
            CreateTestData();
        }

        [TestMethod]
        public void Authenticate_Valid_Customer()
        {
            ValuesController controller = new ValuesController(_shoppingService);
            var incomingRequest = new Login() { CustomerName = "Tony Stark", CustomerPassword = "BlackWidow" };

            IHttpActionResult result = controller.AuthenticateCustomer(incomingRequest);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Authenticate_Invalid_Customer()
        {
            ValuesController controller = new ValuesController(_shoppingService);
            var incomingRequest = new Login() { CustomerName = "IronMan", CustomerPassword = "BlackWidow" };

            IHttpActionResult result = controller.AuthenticateCustomer(incomingRequest);
            StatusCodeResult statusCodeResult = result as StatusCodeResult;

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public void Authenticate_Valid_Customer_Incorrect_Password()
        {
            ValuesController controller = new ValuesController(_shoppingService);
            var incomingRequest = new Login() { CustomerName = "Tony Stark", CustomerPassword = "12345" };

            IHttpActionResult result = controller.AuthenticateCustomer(incomingRequest);
            StatusCodeResult statusCodeResult = result as StatusCodeResult;

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public void Add_Product_To_ShoppingCart()
        {
            ValuesController controller = new ValuesController(_shoppingService);
            var incomingRequest = new Product() { ProductCode = "ABC123", Quantity = 2, UnitPrice = 1.99M };

            IHttpActionResult result = controller.AddToCart("Tony Stark", incomingRequest);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Confirm_Order()
        {
            ValuesController controller = new ValuesController(_shoppingService);
            var productToAdd = new Product() { ProductCode = "ABC123", Quantity = 2, UnitPrice = 1.99M };
            IHttpActionResult result = controller.AddToCart("Tony Stark", productToAdd);
            productToAdd = new Product() { ProductCode = "QQ22", Quantity = 1, UnitPrice = 15.99M };
            result = controller.AddToCart("Tony Stark", productToAdd);

            IHttpActionResult orderResult = controller.ConfirmOrder("Tony Stark");

            Assert.IsInstanceOfType(orderResult, typeof(OkResult));
        }



        [TestCleanup]
        public void TestTearDown()
        {
            _shoppingService.Customers.Clear();
            _shoppingService.Orders.Clear();
        }

        private void CreateTestData()
        {
            var salt = AuthenticationHelper.GenerateSalt();
            var password = AuthenticationHelper.GenerateHashedPassword("password1", salt);
            Customer customer = new Customer() { Name = "John Smith", Address = "London", Email = "jsmith@hotmail.com", PasswordSalt = salt, Password = password };
            _shoppingService.Customers.Add(customer);
            salt = AuthenticationHelper.GenerateSalt();
            password = AuthenticationHelper.GenerateHashedPassword("BlackWidow", salt);
            customer = new Customer() { Name = "Tony Stark", Address = "New York", Email = "ironman@aol.com", PasswordSalt = salt, Password = password };
            _shoppingService.Customers.Add(customer);
        }
    }
}
