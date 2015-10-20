using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace fxpa
{
    class Crypta : ICrypta
    {
        public Crypta()
        {
        }

        static string localKey ="~*@^%&$#";

        string bfKey;

        public string BfKey
        {
            get { return bfKey; }
            set { bfKey = value; }
        }

        public virtual string localDecrypt(String source)
        {
            if (!string.IsNullOrEmpty(localKey))
            {
                BlowfishEasy bfes = new BlowfishEasy(localKey);
                return bfes.DecryptString(source);
            }
            else
            {
                return source;
            }
        }

        public virtual string localEncrypt(String source)
        {
            if (!string.IsNullOrEmpty(localKey))
            {
                BlowfishEasy bfes = new BlowfishEasy(localKey);
                return bfes.EncryptString(source);
            }
            else
            {
                return source;
            }
        }

        public virtual string decrypt(String source)
        {
            if (!string.IsNullOrEmpty(bfKey))
            {
                BlowfishEasy bfes = new BlowfishEasy(bfKey);
                return bfes.DecryptString(source);
            }
            else
            {
                return source;
            }
        }

        public virtual string encrypt(String source)
        {
            if (!string.IsNullOrEmpty(bfKey))
            {
                BlowfishEasy bfes = new BlowfishEasy(bfKey);
                return bfes.EncryptString(source);
            }
            else
            {
                return source;
            }
        }

        public virtual string specialEncrypt(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }
   }


    public class SecurityService
    {
        public static ICrypta GetCrypta()
        {
                ICrypta crypta=new Crypta();
                crypta.BfKey = DSClient.GetKey(typeof(ICrypta).Name);
                return crypta;
        }
    }
}
