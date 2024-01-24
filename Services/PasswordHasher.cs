using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Data.SqlTypes;
using System.Security.Cryptography;

namespace InsuranceManagement.Services
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password);
    }

    public class PasswordHasher : IPasswordHasher
    {
        private readonly IConfiguration configuration;

        public PasswordHasher(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string HashPassword(string password)
        {
            // Generate a random salt
            var saltString = configuration["HashSettings:Salt"];
            //System.Diagnostics.Debug.WriteLine("Salt str: " + saltString);

            byte[] salt = Convert.FromBase64String(saltString);

            // Create a new instance of the Rfc2898DeriveBytes class
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

            // Generate the hash value
            byte[] hash = pbkdf2.GetBytes(20);

            // Combine the salt and hash
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            //System.Diagnostics.Debug.WriteLine("PS str: " + Convert.ToBase64String(hashBytes));
            // Convert the byte array to a string and return it
            return Convert.ToBase64String(hashBytes);

        }
    }
}
