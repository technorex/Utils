using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TechnoRex.Utils.Encryption
{
    public static class AES
    {
        public static class AES256
        {
            public static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes, byte[] saltBytes = null)
            {
                byte[] encryptedBytes = null;

                if (saltBytes == null) saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                            cs.Close();
                        }
                        encryptedBytes = ms.ToArray();
                    }
                }

                return encryptedBytes;
            }
            public static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] saltBytes = null)
            {
                byte[] decryptedBytes = null;

                if (saltBytes == null) saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                    }
                }

                return decryptedBytes;
            }

            public static string EncryptText(string input, string password, byte[] saltBytes = null)
            {
                // Get the bytes of the string
                byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes, saltBytes);

                string result = Convert.ToBase64String(bytesEncrypted);

                return result;
            }
            public static string DecryptText(string input, string password, byte[] saltBytes = null)
            {
                // Get the bytes of the string
                byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes, saltBytes);

                string result = Encoding.UTF8.GetString(bytesDecrypted);

                return result;
            }


            public static void EncryptFile(string filePathSource, string encryptedFilePathtDest, string password, byte[] saltBytes = null)
            {

                byte[] bytesToBeEncrypted = File.ReadAllBytes(filePathSource);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                byte[] bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes, saltBytes);
                File.WriteAllBytes(encryptedFilePathtDest, bytesEncrypted);
            }
            public static void DecryptFile(string sourceEncryptedFilePath, string destDecryptedFilePath, string password, byte[] saltBytes = null)
            {
                byte[] bytesToBeDecrypted = File.ReadAllBytes(sourceEncryptedFilePath);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes, saltBytes);
                File.WriteAllBytes(destDecryptedFilePath, bytesDecrypted);
            }

        }
    }
}
