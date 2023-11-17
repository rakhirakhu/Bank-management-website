using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Bank_Management.DAL
{
    public class HashPassword
    {
        public string Hash(string password)
        {
            // Create an instance of the SHA-256 hashing algorithm
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute the hash value of the password
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                // Return the hashed password as a string
                return builder.ToString();
            }
        }
    }
}