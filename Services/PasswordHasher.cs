using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;

namespace SocialWorkouts.Services
{
    public static class PasswordHasher
    {

        public static string? Hash(string? password)
        {
            if (password is null) return null;
            //Make a 16 byte salt from a random number
            byte[] salt = new byte[16];
            using (var rngCsp = RandomNumberGenerator.Create())
            {
                rngCsp.GetNonZeroBytes(salt);
            }


            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            //
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string hashedPw = Convert.ToBase64String(hashBytes);
            return hashedPw;
        }
        public static bool CompHash(string? password, string? hashedPw)
        {
            if (password == null || hashedPw == null) return false;
            byte[] hashBytes = Convert.FromBase64String(hashedPw);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }

            return true;
        }
    }

}