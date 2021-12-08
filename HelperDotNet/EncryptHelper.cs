using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HelperDotNet
{
    public class EncryptHelper
    {
        public byte[] iv;
        public EncryptHelper()
        {
            
        }

        //Decrypt byte[]
        public byte[] Decrypt(byte[] data, byte[] key)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = key;
            alg.IV = iv;

            using (CryptoStream cs = new CryptoStream
                (ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
            }

            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }

        //Encrypt byte[]
        public byte[] Encrypt(byte[] data, byte[] key)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = key;
            alg.IV = iv;

            using (CryptoStream cs = new CryptoStream
                (ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
            }

            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }

        //Get encrypt key
        public byte[] GetKey()
        {
            Random r = new Random();
            byte[] array = new byte[16];
            r.NextBytes(array);
            return array;
        }
    }
}
