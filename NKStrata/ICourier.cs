using NKStrata.Models;

namespace NKStrata
{
    public interface ICourier
    {
        void SendOrderDetails(Order order);
    }
}
