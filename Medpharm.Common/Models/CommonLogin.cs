using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Medpharm.Common
{
    
    public class CommonLogin
    {
        private static readonly byte[] SaltBytes = { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static readonly int Iterations = 1000;

        public static string Encrypt(string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Text or key cannot be null or empty.");
            }

            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
            var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }

        public static string Decrypt(string encryptedText, string key)
        {
            if (string.IsNullOrEmpty(encryptedText) || string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Encrypted text or key cannot be null or empty.");
            }

            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
            var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            using (var AES = new RijndaelManaged())
            {
                var key = new Rfc2898DeriveBytes(passwordBytes, SaltBytes, Iterations);
                AES.KeySize = 256;
                AES.BlockSize = 128;
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);
                AES.Mode = CipherMode.CBC;

                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            using (var AES = new RijndaelManaged())
            {
                var key = new Rfc2898DeriveBytes(passwordBytes, SaltBytes, Iterations);
                AES.KeySize = 256;
                AES.BlockSize = 128;
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);
                AES.Mode = CipherMode.CBC;

                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }
    }
}