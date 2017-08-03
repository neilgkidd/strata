using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NKStrata
{
    public class Payment : IPayment
    {
        public bool AuthorisePayment(string customerName, decimal paymentAmount, bool testReturnValue = true)
        {
            return testReturnValue;
        }
    }
}