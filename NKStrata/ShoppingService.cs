using NKStrata.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Caching;

namespace NKStrata
{
    public class ShoppingService : IShoppingService
    {
        private const int SILVERMINSPEND = 500;
        private const int GOLDMINSPEND = 800;

        public ShoppingService()
        {
            GetCustomers();
            GetOrders();
            GetPaymentProvider();
        }
        
        public List<Customer> Customers { get; set; }
        public List<Order> Orders { get; set; }
        public IPayment PaymentProvider { get; set; }

        public Customer GetCustomerByName(string customerName)
        {
            return Customers.Find(c => c.Name == customerName);
        }

        public void ProcessCustomerType(Customer customer)
        {
            var customerOrders = Orders.Where(o => o.CustomerName == customer.Name && o.Date > DateTime.Now.AddYears(-1));
            var totalSpend = customerOrders.Sum(x => x.Amount);

            if (totalSpend > GOLDMINSPEND)
                customer.CustomerType = CustomerType.Gold;
            else if (totalSpend > SILVERMINSPEND)
                customer.CustomerType = CustomerType.Silver;
            else
                customer.CustomerType = CustomerType.Standard;
        }

        public Order CreateOrder(Customer customer, ShoppingCart shoppingCart)
        {
            Order order = new Order() { ID = GetNextOrderID(), Address = customer.Address, CustomerName = customer.Name, Date = DateTime.Now };
            order.AddProductCollectionToOrder(shoppingCart.Products);
            CalculateOrderAmount(customer.CustomerType, order);
            Orders.Add(order);
            ProcessCustomerType(customer);

            MemoryCache.Default.Add("orders", Orders, new CacheItemPolicy());

            return order;
        }

        public void CalculateOrderAmount(CustomerType customerType, Order order)
        {
            var totalAmount = order.OrderLines.Sum(o => o.Product.Quantity * o.Product.UnitPrice);
            var discount = GetDiscount(customerType);
            order.Amount = totalAmount * discount;
        }

        public bool PayForOrder(Order order)
        {
            return PaymentProvider.AuthorisePayment(order.CustomerName, order.Amount);
        }

        public void SendCustomerConfirmationMessage(Customer customer)
        {
            //send email customer customer.Email property
        }

        public void ContactCourier(Order order)
        {
            var courier = CourierFactory.GetCourier();
            courier.SendOrderDetails(order);
        }


        private decimal GetDiscount(CustomerType customerType)
        {
            return (decimal)(100 - (int)customerType) / 100;
        }

        private int GetNextOrderID()
        {
            if (Orders != null && Orders.Count > 0)
                return Orders.Max(o => o.ID) + 1;
            else
                return 1;
        }

        private void GetCustomers()
        {
            var customers = MemoryCache.Default.Get("customers");
            if (customers != null)
            {
                Customers = customers as List<Customer>;
            }
            else
            {
                Customers = new List<Customer>();
                CreateTestCustomers();
            }
        }

        private void GetOrders()
        {
            var orders = MemoryCache.Default.Get("orders");
            if (orders != null)
                Orders = orders as List<Order>;
            else
                Orders = new List<Order>();
        }

        private void GetPaymentProvider()
        {
            PaymentProvider = PaymentProviderFactory.GetPaymentProvider();
        }

        private void CreateTestCustomers()
        {
            var salt = AuthenticationHelper.GenerateSalt();
            var password = AuthenticationHelper.GenerateHashedPassword("password2", salt);
            Customer customer = new Customer() { Name = "John Jones", Address = "London", Email = "jj@hotmail.com", PasswordSalt = salt, Password = password };
            Customers.Add(customer);
            salt = AuthenticationHelper.GenerateSalt();
            password = AuthenticationHelper.GenerateHashedPassword("Shield", salt);
            customer = new Customer() { Name = "Steve Rogers", Address = "New York", Email = "cap@aol.com", PasswordSalt = salt, Password = password };
            Customers.Add(customer);

            MemoryCache.Default.Add("customers", Customers, new CacheItemPolicy());
        }
    }
}