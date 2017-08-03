using NKStrata.Models;
using System.Net;
using System.Web.Http;

namespace NKStrata.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly IShoppingService _shoppingService;

        public ValuesController(IShoppingService shoppingService)
        {
            _shoppingService = shoppingService;
        }

        [HttpPost]
        [Route("customer/authenticate")]
        public IHttpActionResult AuthenticateCustomer([FromBody]Login login)
        {
            var customer = _shoppingService.GetCustomerByName(login.CustomerName);

            if (customer != null)
            {
                if (AuthenticationHelper.IsCustomerPasswordValid(customer, login.CustomerPassword))
                {
                    return Ok(); //a more complete solution would utlise a public/private key pair and return the public key to the calling application
                }
            }

            return StatusCode(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        [Route("shopping/add/{customerName}")]
        public IHttpActionResult AddToCart(string customerName, [FromBody]Product product)
        {
            var customer = _shoppingService.GetCustomerByName(customerName);

            if (customer != null)
            {
                customer.ShoppingCart.AddProduct(product);
                return Ok();
            }
            else
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost]
        [Route("shopping/confirm")]
        public IHttpActionResult ConfirmOrder([FromBody]string customerName)
        {
            var customer = _shoppingService.GetCustomerByName(customerName);

            if (customer != null)
            {
                var order = _shoppingService.CreateOrder(customer, customer.ShoppingCart);

                bool paymentSuccess = _shoppingService.PayForOrder(order);

                if (paymentSuccess)
                {
                    _shoppingService.SendCustomerConfirmationMessage(customer);
                    _shoppingService.ContactCourier(order);
                }
                else
                {
                    return BadRequest("payment was unsuccessful");
                }

                return Ok();
            }
            else
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
