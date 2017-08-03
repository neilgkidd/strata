using System.Configuration;

namespace NKStrata
{
    public class CourierFactory
    {
        public static ICourier GetCourier()
        {
            ICourier courier;
            var configValue = ConfigurationManager.AppSettings["Courier"];

            if (configValue != null && configValue.ToString() != string.Empty)
            {
                switch (configValue.ToString())
                {
                    case "post":
                        courier = new PostCourier();
                        break;
                    default:
                        courier = new PostCourier();
                        break;
                }
            }
            else
            {
                courier = new PostCourier();
            }

            return courier;
        }
    }
}