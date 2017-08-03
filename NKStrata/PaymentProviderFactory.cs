using System.Configuration;

namespace NKStrata
{
    public static class PaymentProviderFactory
    {
        public static IPayment GetPaymentProvider()
        {
            IPayment newPaymentProvider;
            var configValue = ConfigurationManager.AppSettings["PaymentProvider"];

            if (configValue != null && configValue.ToString() != string.Empty)
            {
                switch (configValue.ToString())
                {
                    case "standard":
                        newPaymentProvider = new Payment();
                        break;
                    default:
                        newPaymentProvider = new Payment();
                        break;
                }
            }
            else
            {
                newPaymentProvider = new Payment();
            }

            return newPaymentProvider;
        }
    }
}