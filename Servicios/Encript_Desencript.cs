using System.Security.Cryptography;
using System.Text;

namespace API_QCode.Servicios
{
    public class Encript_Desencript
    {
        public static string EncriptarContrasena(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                using (var keyDerivationFunction = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes("YourSalt12345678"), 10000))
                {
                    aesAlg.Key = keyDerivationFunction.GetBytes(32);
                    aesAlg.IV = keyDerivationFunction.GetBytes(16);
                }

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }
        public static string DesencriptarContrasena(string cipherText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                using (var keyDerivationFunction = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes("YourSalt12345678"), 10000))
                {
                    aesAlg.Key = keyDerivationFunction.GetBytes(32);
                    aesAlg.IV = keyDerivationFunction.GetBytes(16);
                }

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
