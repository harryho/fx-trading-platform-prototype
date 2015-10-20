/*
  Copyright 2001-2009 Markus Hahn 
  All rights reserved. See documentation for license details.
*/

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using fxpa.Properties;


namespace fxpa
{
    #region String Handling

    /// <summary>This class encrypts and decrypts strings the same way like it is
    /// done in the BlowfishJ package. Strings can be directly exchanged between these
    /// two platforms, given that both sides expect standard Unicode characters. Notice
    /// that this solution is less versatile than BlowfishSimple, especially since it
    /// lacks the ability to verify if keys/passwords do match.</summary>
    public class BlowfishEasy
    {
        static readonly RandomNumberGenerator _rng = new RNGCryptoServiceProvider();

        BlowfishCBC bfc;

        /// <summary>Creates a new BlowfishEasy instance. Notice that this ctor
        /// supports only the new way of how string are set up. There is no support
        /// for the old BlowfishJ key setup available right now (which had a
        /// design flaw by not using the full Unicode character space).</summary>
        /// <param name="password">The password used for encryption and decryption.</param>
        public BlowfishEasy(String password)
        {
            int c = password.Length;

            byte[] passw = new byte[c << 1];
            
            for (int i = 0, j = 0; i < c; i++)
            {
                int chr = (int)password[i];
                passw[j++] = (byte)((chr >> 8) & 0x0ff);
                passw[j++] = (byte)( chr       & 0x0ff);
            }
        
            SHA1 sha = new SHA1Managed();

            byte[] key = sha.ComputeHash(passw);

            this.bfc = new BlowfishCBC(key, 0, key.Length);
        }

        /// <summary>Encrypts a string. The output size is always twice the size of
        /// the input, plus between 18 and 32 additional characters.</summary>
        /// <param name="plainText">The plaintext.</param>
        /// <returns>The encrypted string.</returns>
        public String EncryptString(String plainText)
        {
            int strLen = plainText.Length << 1;
            
            byte[] buf = new byte[ BlowfishCBC.BLOCK_SIZE *
                (strLen / BlowfishCBC.BLOCK_SIZE + 1)];

            int pos = 0;
            for (int i = 0; pos < strLen; i++)
            {
                int chr = plainText[i];

                buf[pos++] = (byte)((chr >> 8) & 0x0ff);
                buf[pos++] = (byte) (chr       & 0x0ff);
            }

            byte padVal = (byte) (buf.Length - strLen);

            while (pos < buf.Length)
            {
                buf[pos++] = padVal;
            }

            byte[] iv = new byte[BlowfishCBC.BLOCK_SIZE];

            lock (_rng)
            {
                _rng.GetBytes(iv);
            }
    
            this.bfc.IV = iv;

            this.bfc.Encrypt(buf, 0, buf, 0, buf.Length);

            return BytesToHexStr(iv) + BytesToHexStr(buf);  
        }

        /// <summary> Decrypts a string fomerly encrypted with the EncryptString() method
        /// and the same password. If the password is wrong the result will be either
        /// garbage or the method will fails and return null.</summary>
        /// <param name="cipherText">The encrypted text to decrypt.</param>
        /// <returns>The decrypted text or null if an error occured, which can be due to
        /// wrong encoding or a bad padding size, which is usually caused by a wrong
        /// password.</returns>
        public String DecryptString(String cipherText)
        {
            // overlapping hex encoding?
            int len = cipherText.Length;

            if (0 != (len & 1))
            {
                return null;
            }

            len >>= 1;
            
            // aligned to block size?
            if (0 != (len % BlowfishCBC.BLOCK_SIZE))
            {
                return null;
            }

            // minimum amount of data (IV plus empty padded block)?
            if ((BlowfishCBC.BLOCK_SIZE << 1) > len)
            {
                return null;
            }

            // hex-decode the whole material
            byte[] buf = HexStrToBytes(cipherText);
            if (null == buf)
            {
                return null;
            }
            
            this.bfc.SetIV(buf, 0);

            len = buf.Length - BlowfishCBC.BLOCK_SIZE;

            // (we overlap in a negative direction , so this will be fine)
            this.bfc.Decrypt(
                buf, 
                BlowfishCBC.BLOCK_SIZE, 
                buf,
                0,
                len);
            
            int padVal = buf[len - 1] & 0x0ff;

            // check if the padding is right, this is a somewhat reliable way
            // to detect if the password was correct...

            // we should never have odd padding (because every Unicode
            // character consists out of two bytes)
            if (0 != (padVal & 1))
            {
                return null;
            }
    
            // valid range?
            if ((BlowfishCBC.BLOCK_SIZE < padVal) || (2 > padVal))
            {
                return null;    
            }

            len -= padVal;

            char[] str = new char[len >> 1];

            for (int i = 0, j = 0; i < len; j++)
            {
                str[j] = (char)((((int)buf[i++]) << 8) |
                                 ((int)buf[i++]) & 0x0ff);
            }

            return new string(str);
        }

        static readonly char[] HEX_TAB =
        {
            '0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f'
        };

        static string BytesToHexStr(byte[] data)
        {
            int c = data.Length;

            StringBuilder sb = new StringBuilder(c << 1);

            for (int i = 0; i < c; i++)
            {
                int val = data[i];
                sb.Append(HEX_TAB[(val >> 4) & 0x0f]); 
                sb.Append(HEX_TAB[ val       & 0x0f]); 
            }

            return sb.ToString();
        }

        static byte[] HexStrToBytes(String hex)
        {
            int c = hex.Length;

            if (1 == (c & 1))
            {
                return null;
            }
        
            c >>= 1;

            byte[] result = new byte[c];

            for (int i = 0, j = 0; i < c; i++)
            {
                int reg = 0;

                for (int k = 0; k < 2; k++)
                {
                    char val = hex[j++];

                    reg <<= 4;

                    if (('0' <= val) && ('9' >= val))
                    {
                        reg |= (int)(val - '0');
                    }
                    else if (('a' <= val) && ('f' >= val))
                    {
                        reg |= (int)(val - 'a') + 10;
                    }
                    else if (('A' <= val) && ('F' >= val))
                    {
                        reg |= (int)(val - 'A') + 10;
                    }
                    else
                    {
                        return null;
                    }
                }
                result[i] = (byte)reg;
            }

            return result;
        }
    }
    #endregion

    #region Streaming

    /// <summary>Stream direction definitions for the BlowfishStream class.</summary>
    public enum BlowfishStreamMode
    {
        /// <summary>Stream is opened for decryption or reading respectively.</summary>
        Read,
        /// <summary>Stream is opened for encryption or writing respectively.</summary>
        Write 
    }

    /// <summary>Stream factory. The instances created read and writes data binary compatible to the
    /// BlowfishInputStream and BlowfishOutputStream of BlowfishJ.</summary>
    /// <remarks>Streams produced and consumed by this class use an SHA-1 digest of the key material
    /// as 160bit keys to set up the cipher. The data is encrypted using CBC, with the first block
    /// being the initial (random) initialization vector. Padding is done with PKCS7. This means that
    /// every stream is aligned to the block size of Blowfish. It also means that there is no
    /// end-of-stream marker used.</remarks>
    public class BlowfishStream : CryptoStream
    {
        private BlowfishStream(Stream s, ICryptoTransform t, CryptoStreamMode m) : base(s, t, m) { }

        /// <summary>Creates a new Blowfish stream.</summary>
        /// <param name="stm">The stream to read or write to.</param>
        /// <param name="mode">Operation mode</param>
        /// <param name="key">The buffer with the key material.</param>
        /// <param name="ofs">Where the key material starts in the buffer.</param>
        /// <param name="len">Length of the key material in bytes.</param>
        public static BlowfishStream Create(
            Stream stm,
            BlowfishStreamMode mode,
            byte[] key,
            int ofs,
            int len)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            BlowfishAlgorithm balg = new BlowfishAlgorithm();
            balg.Key = sha.ComputeHash(key, ofs, len);
            balg.Padding = PaddingMode.PKCS7;
            balg.Mode = CipherMode.CBC;
            sha.Clear();

            if (BlowfishStreamMode.Write == mode)
            {
                byte[] iv = balg.IV;
                stm.Write(iv, 0, iv.Length);
                return new BlowfishStream(stm, balg.CreateEncryptor(), CryptoStreamMode.Write);
            }
            else
            {
                byte[] iv = new byte[balg.BlockSize >> 3];
                for (int i = 0; i < iv.Length; i++)
                {
                    int ivb = stm.ReadByte();
                    if (-1 == ivb)
                    {
                        throw new IOException( Properties.Resources.JAVAIOP_CANNOT_READ_IV);
                    }
                    iv[i] = (byte)ivb;
                }
                balg.IV = iv;
                return new BlowfishStream(stm, balg.CreateDecryptor(), CryptoStreamMode.Read);
            }
        }
    }
    #endregion
}
