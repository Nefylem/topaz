using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Topaz.Application.Common.Command
{
    public class EncryptionControl
    {
        private readonly string initVector = "rga50vx11790xyuij"; //Default password - change for an actual release
        private readonly string encryptionString = "U$3Th3F0rc3Luk3";
        private readonly int keysize = 256;

        public string Encrypt(string text)
        {

            var initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            var plainTextBytes = Encoding.UTF8.GetBytes(text);
            var password = new PasswordDeriveBytes(encryptionString, null);
            var keyBytes = password.GetBytes(keysize / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            var cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public string Decrypt(string text)
        {
            var initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            var cipherTextBytes = Convert.FromBase64String(text);
            var password = new PasswordDeriveBytes(encryptionString, null);
            var keyBytes = password.GetBytes(keysize / 8);
            var symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];
            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }
}
