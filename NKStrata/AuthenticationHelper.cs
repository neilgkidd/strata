using NKStrata.Models;
using System.Linq;
using System.Security.Cryptography;

namespace NKStrata
{
    public static class AuthenticationHelper
    {
        private const int SALTByteSize = 24;
        private const int HASHByteSize = 24;
        private const int HASHIterations = 1001;

        public static byte[] GenerateSalt()
        {
            byte[] saltValue = new byte[SALTByteSize];
            using (RNGCryptoServiceProvider saltGenerator = new RNGCryptoServiceProvider())
            {
                saltGenerator.GetBytes(saltValue);
            }

            return saltValue;
        }

        public static byte[] GenerateHashedPassword(string password, byte[] saltValue)
        {
            byte[] hashedPassword;

            using (Rfc2898DeriveBytes hashGenerator = new Rfc2898DeriveBytes(password, saltValue))
            {
                hashGenerator.IterationCount = HASHIterations;
                hashedPassword = hashGenerator.GetBytes(HASHByteSize);
            }

            return hashedPassword;
        }

        public static bool IsCustomerPasswordValid(Customer customer, string password)
        {
            byte[] hashedPassword = AuthenticationHelper.GenerateHashedPassword(password, customer.PasswordSalt);

            return hashedPassword.SequenceEqual(customer.Password);
        }
    }
}