using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKStrata
{
    public interface IPayment
    {
        bool AuthorisePayment(string customerName, decimal paymentAmount, bool testReturnValue = true);
    }
}
