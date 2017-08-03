namespace NKStrata.Models
{
    public enum CustomerType
    {
        Standard = 0,
        Silver = 2,
        Gold = 3
    }

    public class Customer
    {
        public string Name { get; set; }
        public CustomerType CustomerType { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public ShoppingCart ShoppingCart
        {
            get
            {
                if (shoppingCart == null)
                    shoppingCart = new ShoppingCart();

                return shoppingCart;
            }
            set
            {
                shoppingCart = value;
            }
        }

        private ShoppingCart shoppingCart;
    }
}