using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Helper.Security
{
    public class RsaEncrypt
    {
        public string Encrypt(string key, string data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("lost key ");

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(key);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(data), false);
            string rs = ToHexString(cipherbytes);
            rs = Convert.ToBase64String(cipherbytes, Base64FormattingOptions.None);
            return rs;
        }

        public string Decrypt(string key, string data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("lost key ");
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);
            byte[] bte = Convert.FromBase64String(data);// HexToBytes(data);
            byte[] cipherbytes = rsa.Decrypt(bte, false);
            return Encoding.UTF8.GetString(cipherbytes);
        }

        public static string[] GenerateKeys(ref string privateKey, ref string publicKey)
        {
            string[] sKeys = new String[2];
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            privateKey = sKeys[0] = rsa.ToXmlString(true);//PRIVATE
            publicKey = sKeys[1] = rsa.ToXmlString(false);//PUBLIC
            return sKeys;
        }

        public string GetSignature(string OriginalString, string PrivateKey)
        {
            if (string.IsNullOrEmpty(OriginalString))
            {
                throw new ArgumentNullException();
            }
            RSACryptoServiceProvider _rsaCrypto = new RSACryptoServiceProvider();
            _rsaCrypto.FromXmlString(PrivateKey);
            byte[] originalData = Encoding.GetEncoding("utf-8").GetBytes(OriginalString);
            byte[] singData = _rsaCrypto.SignData(originalData, new SHA1CryptoServiceProvider());
            var SignatureString = Convert.ToBase64String(singData);
            return SignatureString;
        }

        public bool VerifySignature(string OriginalString, string SignatureString, string PublicKey)
        {
            if (string.IsNullOrEmpty(OriginalString))
            {
                throw new ArgumentNullException();
            }
            RSACryptoServiceProvider _rsaCrypto = new RSACryptoServiceProvider();
            _rsaCrypto.FromXmlString(PublicKey);
            byte[] originalData = Encoding.GetEncoding("utf-8").GetBytes(OriginalString);
            byte[] signatureData = Convert.FromBase64String(SignatureString);
            var isVerify = _rsaCrypto.VerifyData(originalData, new SHA1CryptoServiceProvider(), signatureData);
            return isVerify;
        }

        private byte[] encryptor(string publicKey, byte[] OriginalData)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            if (OriginalData == null || OriginalData.Length <= 0)
            {
                throw new NotSupportedException();
            }

            rsa.FromXmlString(publicKey);

            var encryptData = rsa.Encrypt(OriginalData, false);
            return encryptData;
        }

        private byte[] decryptor(string PrivateKey, byte[] EncryptDada)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            if (EncryptDada == null || EncryptDada.Length <= 0)
            {
                throw new NotSupportedException();
            }

            rsa.FromXmlString(PrivateKey);
            var decrtpyData = rsa.Decrypt(EncryptDada, false);
            return decrtpyData;
        }

        private string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "   
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }

            return hexString;
        }

        public byte[] HexToBytes(string src)
        {
            int l = src.Length / 2;
            String str;
            byte[] ret = new byte[l];

            for (int i = 0; i < l; i++)
            {
                str = src.Substring(i * 2, 2);
                ret[i] = Convert.ToByte(str, 16);
            }
            return ret;
        }

    }
}
