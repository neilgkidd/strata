using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NKStrata.Models;

namespace NKStrata.Tests
{
    [TestClass]
    public class CustomerTest
    {
        private IShoppingService _shoppingService;

        [TestInitialize]
        public void TestSetup()
        {
            _shoppingService = new ShoppingService();
            CreateTestData();
        }

        [TestMethod]
        public void Customer_List_Has_Data()
        {            
            Assert.IsTrue(_shoppingService.Customers.Count > 0);
        }

        [TestMethod]
        public void Get_Valid_Customer_By_Name()
        {
            var testName = "Tony Stark";
            var customer = _shoppingService.GetCustomerByName(testName);
            Assert.IsNotNull(customer);
        }

        [TestMethod]
        public void Get_Invalid_Customer_By_Name()
        {
            var testName = "Bruce Banner";
            var customer = _shoppingService.GetCustomerByName(testName);
            Assert.IsNull(customer);
        }

        [TestMethod]
        public void Authenticate_Customer()
        {
            var testName = "Tony Stark";
            var customer = _shoppingService.GetCustomerByName(testName);
            Assert.AreEqual(true, AuthenticationHelper.IsCustomerPasswordValid(customer, "BlackWidow"));
        }

        [TestMethod]
        public void Authenticate_Customer_Fail()
        {
            var testName = "Tony Stark";
            var customer = _shoppingService.GetCustomerByName(testName);
            Assert.AreEqual(false, AuthenticationHelper.IsCustomerPasswordValid(customer, "password"));
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
