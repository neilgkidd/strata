using NKStrata.Models;
using System.Collections.Generic;

namespace NKStrata
{
    public interface IShoppingService
    {
        List<Customer> Customers { get; set; }
        List<Order> Orders { get; set; }
        IPayment PaymentProvider { get; set; }
        Customer GetCustomerByName(string customerName);
        void ProcessCustomerType(Customer customer);
        Order CreateOrder(Customer customer, ShoppingCart shoppingCart);
        void CalculateOrderAmount(CustomerType customerType, Order order);
        bool PayForOrder(Order order);
        void SendCustomerConfirmationMessage(Customer customer);
        void ContactCourier(Order order);
    }
}
