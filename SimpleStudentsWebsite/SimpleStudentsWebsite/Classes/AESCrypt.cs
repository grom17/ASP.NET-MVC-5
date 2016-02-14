using System;
using System.Security.Cryptography;
using System.IO;

namespace SimpleStudentsWebsite.Classes
{
    public class AESCrypt
    {
        public static string EncryptString(string StringToEncrypt, string Key)
        {
            try
            {
                if (string.IsNullOrEmpty(StringToEncrypt))
                {
                    return StringToEncrypt;
                }
                var aes = new AesManaged();

                aes.Key = StringToByte(Key, 32); // convert to 24 characters - 192 bits
                aes.IV = StringToByte(Key, 16);
                byte[] key = aes.Key;
                byte[] IV = aes.IV;

                byte[] decrypted = StringToByte(StringToEncrypt); ;
                byte[] encrypted = null;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(key, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(decrypted, 0, decrypted.Length);
                        cs.FlushFinalBlock();
                    }
                    encrypted = ms.ToArray();
                }

                return ByteToString(encrypted);
            }
            catch (Exception)
            {
                //Logger.LogError("AESCrypt.EncryptString", ex);
                return StringToEncrypt;
            }
        }

        public static string DescryptString(string StringToDecrypt, string Key)
        {
            try
            {
                string std = StringToDecrypt;
                if (string.IsNullOrEmpty(StringToDecrypt))
                {
                    return StringToDecrypt;
                }
                var aes = new AesManaged();

                aes.Key = StringToByte(Key, 32); // convert to 24 characters - 192 bits
                aes.IV = StringToByte(Key, 16);
                byte[] key = aes.Key;
                byte[] IV = aes.IV;

                byte[] encrypted = HexStringToByte(std);
                string decrypted = null;

                // Now decrypt the previously encrypted message using the decryptor
                using (MemoryStream ms = new MemoryStream(encrypted))
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(key, IV), CryptoStreamMode.Read))
                    {
                        decrypted = ByteToString(cs);
                    }
                }

                return decrypted;
            }
            catch (Exception)
            {
                //Logger.LogError("AESCrypt.DescryptString", ex);
                return StringToDecrypt;
            }
        }
        protected static byte[] HexStringToByte(string StringToConvert)
        {

            char[] CharArray = StringToConvert.ToCharArray();
            byte[] ByteArray = new byte[CharArray.Length / 2];
            for (int i = 0, j = 0; i < CharArray.Length; i += 2, j++)
            {
                ByteArray[j] = Convert.ToByte("" + CharArray[i] + CharArray[i + 1], 16);
            }
            return ByteArray;
        }

        protected static byte[] StringToByte(string StringToConvert)
        {

            char[] CharArray = StringToConvert.ToCharArray();
            byte[] ByteArray = new byte[CharArray.Length];
            for (int i = 0; i < CharArray.Length; i++)
            {
                ByteArray[i] = Convert.ToByte(CharArray[i]);
            }
            return ByteArray;
        }

        protected static byte[] StringToByte(string StringToConvert, int length)
        {

            char[] CharArray = StringToConvert.ToCharArray();
            byte[] ByteArray = new byte[length];
            int y = 0;
            for (int i = 0; i < ByteArray.Length; i++)
            {
                if (y >= CharArray.Length)
                {
                    y = 0;
                }
                ByteArray[i] = Convert.ToByte(CharArray[y]);
                y++;
            }
            return ByteArray;
        }

        protected static string ByteToString(CryptoStream buff)
        {
            string sbinary = "";
            int b = 0;
            do
            {
                b = buff.ReadByte();
                if (b != -1) sbinary += ((char)b);

            } while (b != -1);
            return (sbinary);
        }

        protected static string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }
    }
}
