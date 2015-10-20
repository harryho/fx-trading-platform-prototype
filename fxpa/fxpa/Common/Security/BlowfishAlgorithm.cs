/*
  Copyright 2001-2009 Markus Hahn 
  All rights reserved. See documentation for license details.
*/

using System;
using System.Security;
using System.Security.Cryptography;


namespace fxpa
{
    /// <summary>Implementation of the Blowfish algorithm as a standard component for
    /// the .NET security framework.</summary>
    public class BlowfishAlgorithm : SymmetricAlgorithm, ICryptoTransform
    {
        // in factory mode the Blowfish instances are always null, they only get
        // initialized in transformation mode

        BlowfishECB bfe;
        BlowfishCBC bfc;

        bool isEncryptor;
        byte[] block;
        byte[] origIV;

        RNGCryptoServiceProvider rng;

        /// <summary>Default constructor. Starts as an uninitialized ECB instance.</summary>
        public BlowfishAlgorithm() : base()
        {
            KeySizeValue = BlowfishECB.MAX_KEY_LENGTH << 3;

            LegalBlockSizesValue = new KeySizes[1];
            LegalBlockSizesValue[0] = new KeySizes(
                    BlockSize,
                    BlockSize,
                    BlowfishECB.BLOCK_SIZE);

            LegalKeySizesValue = new KeySizes[1];
            LegalKeySizesValue[0] = new KeySizes(
                    0,
                    BlowfishECB.MAX_KEY_LENGTH << 3,
                    8);

            ModeValue = CipherMode.ECB;
        }

        private BlowfishAlgorithm(byte[] key, byte[] iv, bool useCBC, bool isEncryptor)
        {
            if (null == key)
            {
                GenerateKey();
            }
            else
            {
                Key = key;
            }

            if (useCBC)
            {
                IV = this.origIV = iv;

                this.bfc = new BlowfishCBC(KeyValue, 0, KeyValue.Length);
                this.bfc.SetIV(IVValue, 0);
            }
            else
            {
                this.bfe = new BlowfishECB(KeyValue, 0, KeyValue.Length);
            }

            this.isEncryptor = isEncryptor;
        }

        /// <see cref="System.Security.Cryptography.SymmetricAlgorithm.BlockSize"/>
        public override int BlockSize
        {
            get
            {
                return BlowfishECB.BLOCK_SIZE << 3;
            }
            set
            {
                if (value != BlowfishECB.BLOCK_SIZE << 3)
                {
                    throw new CryptographicException();
                }
            }
        }

        /// <see cref="System.Security.Cryptography.SymmetricAlgorithm.IV"/>
        public override byte[] IV
        {
            get
            {
                if (null == IVValue)
                {
                    GenerateIV();
                }
                return (byte[])IVValue.Clone();
            }
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                if (value.Length != BlowfishECB.BLOCK_SIZE)
                {
                    throw new CryptographicException();
                }
                IVValue = (byte[])value.Clone();
            }
        }

        /// <see cref="System.Security.Cryptography.SymmetricAlgorithm.Key"/>
        public override byte[] Key
        {
            get
            {
                return KeyValue;
            }
            set
            {
                KeyValue = value;
            }
        }

        /// <see cref="System.Security.Cryptography.SymmetricAlgorithm.KeySize"/>
        public override int KeySize
        {
            get
            {
                return KeySizeValue;
            }
            set
            {
                KeySizes ks = LegalKeySizes[0];

                if ((0 != (value % ks.SkipSize)) ||
                          (value > ks.MaxSize) ||
                          (value < ks.MinSize))
                {
                    throw new CryptographicException();
                }
                KeySizeValue = value;
            }
        }

        /// <see cref="System.Security.Cryptography.SymmetricAlgorithm.Mode"/>
        public override CipherMode Mode
        {
            get
            {
                return ModeValue;
            }
            set
            {
                if (value != CipherMode.CBC &&
                    value != CipherMode.ECB)
                {
                    throw new CryptographicException();
                }
                ModeValue = value;
            }
        }

        void CopyPadding(BlowfishAlgorithm ba)
        {
            switch (Padding)
            {
                case PaddingMode.ANSIX923:
                case PaddingMode.ISO10126:
                case PaddingMode.PKCS7:
                case PaddingMode.Zeros:
                {
                    ba.Padding = Padding;
                    break;
                }
                default:
                {
                    throw new CryptographicException();
                }
            }
        }

        /// <see cref="System.Security.Cryptography.SymmetricAlgorithm.CreateEncryptor(byte[], byte[])"/>
        public override ICryptoTransform CreateEncryptor(byte[] key, byte[] iv)
        {
            BlowfishAlgorithm result = new BlowfishAlgorithm(
                key,
                iv,
                (CipherMode.CBC == ModeValue),
                true);

            CopyPadding(result);
            return result;
        }

        /// <see cref="System.Security.Cryptography.SymmetricAlgorithm.CreateDecryptor(byte[], byte[])"/>
        public override ICryptoTransform CreateDecryptor(byte[] key, byte[] iv)
        {
            BlowfishAlgorithm result = new BlowfishAlgorithm(
                key,
                iv,
                (CipherMode.CBC == ModeValue),
                false);

            CopyPadding(result);
            return result;
        }

        /// <see cref="System.Security.Cryptography.SymmetricAlgorithm.GenerateKey"/>
        public override void GenerateKey()
        {
            if (null == this.rng)
            {
                this.rng = new RNGCryptoServiceProvider();
            }

            KeyValue = new byte[KeySizeValue >> 3];

            this.rng.GetBytes(KeyValue);
        }

        /// <see cref="System.Security.Cryptography.SymmetricAlgorithm.GenerateIV"/>
        public override void GenerateIV()
        {
            if (null == this.rng)
            {
                this.rng = new RNGCryptoServiceProvider();
            }

            IVValue = new byte[BlowfishECB.BLOCK_SIZE];

            this.rng.GetBytes(IVValue);
        }

        /// <see cref="System.Security.Cryptography.ICryptoTransform.CanReuseTransform"/>
        public bool CanReuseTransform
        {
            get
            {
                return true;
            }
        }

        /// <see cref="System.Security.Cryptography.ICryptoTransform.CanTransformMultipleBlocks"/>
        public bool CanTransformMultipleBlocks
        {
            get
            {
                return true;
            }
        }

        /// <see cref="System.Security.Cryptography.ICryptoTransform.InputBlockSize"/>
        public int InputBlockSize
        {
            get
            {
                return BlowfishECB.BLOCK_SIZE;
            }
        }

        /// <see cref="System.Security.Cryptography.ICryptoTransform.OutputBlockSize"/>
        public int OutputBlockSize
        {
            get
            {
                return BlowfishECB.BLOCK_SIZE;
            }
        }

        /// <see cref="System.Security.Cryptography.ICryptoTransform.TransformBlock"/>
        public int TransformBlock(byte[] bufIn, int ofsIn, int count, byte[] bufOut, int ofsOut)
        {
            if (0 == count)
            {
                return 0;
            }
            if (0 != count % BlowfishECB.BLOCK_SIZE)
            {
                throw new CryptographicException("unexpected unaligned data");
            }
            if (this.isEncryptor)
            {
                if (null == this.bfe) return this.bfc.Encrypt(bufIn, ofsIn, bufOut, ofsOut, count);
                else                  return this.bfe.Encrypt(bufIn, ofsIn, bufOut, ofsOut, count);
            }
            else
            {
                // for decryption we have to buffer the last block, since it could be the very last one
                int outp = 0;
                if (null == this.block)
                {
                    this.block = new byte[BlowfishECB.BLOCK_SIZE];
                }
                else
                {
                    // and also flush the former last one
                    if (null == this.bfe) this.bfc.Decrypt(this.block, 0, bufOut, ofsOut, BlowfishECB.BLOCK_SIZE);
                    else                  this.bfe.Decrypt(this.block, 0, bufOut, ofsOut, BlowfishECB.BLOCK_SIZE);
                    ofsOut += BlowfishECB.BLOCK_SIZE;
                    outp += BlowfishECB.BLOCK_SIZE;
                }
                count -= BlowfishECB.BLOCK_SIZE;
                // keep the last block as _ciphertext_ (for safety reasons)
                Array.Copy(bufIn, ofsIn + count, this.block, 0, this.block.Length);

                if (null == this.bfe) return outp + this.bfc.Decrypt(bufIn, ofsIn, bufOut, ofsOut, count);
                else                  return outp + this.bfe.Decrypt(bufIn, ofsIn, bufOut, ofsOut, count);
            }
        }

        /// <see cref="System.Security.Cryptography.ICryptoTransform.TransformFinalBlock"/>
        public byte[] TransformFinalBlock(byte[] buf, int ofs, int count)
        {
            if (BlowfishECB.BLOCK_SIZE < count)
            {
                throw new CryptographicException();
            }

            byte[] result;

            if (this.isEncryptor)
            {
                int resLen = BlowfishECB.BLOCK_SIZE +
                            (BlowfishECB.BLOCK_SIZE == count ? BlowfishECB.BLOCK_SIZE : 0);
                result = new byte[resLen];

                if (PaddingMode.PKCS7 == PaddingValue)
                {
                    byte pval = (byte)(resLen - count);
                    for (int i = count; i < resLen; i++) result[i] = pval;
                }
                else if (PaddingMode.Zeros == PaddingValue)
                {
                    for (int i = count; i < resLen; i++) result[i] = 0;
                }
                else if (PaddingMode.ANSIX923 == PaddingValue)
                {
                    for (int i = count; i < resLen - 1; i++) result[i] = 0;
                    result[resLen - 1] = (byte)(resLen - count);
                }
                else if (PaddingMode.ISO10126 == PaddingValue)
                {
                    RandomNumberGenerator.Create().GetBytes(result);
                }
                else
                {
                    throw new CryptographicException();
                }
                Array.Copy(buf, ofs, result, 0, count);
                if (null == this.bfe) this.bfc.Encrypt(result, 0, result, 0, BlowfishECB.BLOCK_SIZE);
                else                  this.bfe.Encrypt(result, 0, result, 0, BlowfishECB.BLOCK_SIZE);
            }
            else
            {
                byte[] tmp;
                if (0 == count)
                {
                    if (null == (tmp = this.block))
                    {
                        // special case: nothing to do at all
                        return new byte[0];
                    }
                }
                else
                {
                    if (null == this.bfe) this.bfc.Decrypt(this.block, 0, this.block, 0, BlowfishECB.BLOCK_SIZE);
                    else                  this.bfe.Decrypt(this.block, 0, this.block, 0, BlowfishECB.BLOCK_SIZE);
                    tmp = new byte[BlowfishECB.BLOCK_SIZE];
                    // NOTE: we use 'count', but only some padding schemes will really work if it is
                    //       not aligned to the block size (but even then very likely produce garbage)
                    Array.Copy(buf, ofs, tmp, 0, count);
                }
                if (null == this.bfe) this.bfc.Decrypt(tmp, 0, tmp, 0, BlowfishECB.BLOCK_SIZE);
                else                  this.bfe.Decrypt(tmp, 0, tmp, 0, BlowfishECB.BLOCK_SIZE);

                // make sure the padding looks ok as far as we can tell
                int rest;
                if (PaddingMode.PKCS7 == PaddingValue)
                {
                    byte pval = tmp[tmp.Length - 1];
                    if (BlowfishECB.BLOCK_SIZE < (int)pval)
                    {
                        throw new CryptographicException();
                    }
                    rest = tmp.Length - pval;
                    for (int i = tmp.Length - 2; i >= rest; i--)
                    {
                        if (tmp[i] != pval)
                        {
                            throw new CryptographicException();
                        }
                    }
                }
                else if (PaddingMode.Zeros == PaddingValue ||
                         PaddingMode.ISO10126 == PaddingValue)
                {
                    // (there's no real way of telling, since the plaintext could be padding or vice versa)
                    rest = tmp.Length;
                }
                else if (PaddingMode.ANSIX923 == PaddingValue)
                {
                    byte pval = tmp[tmp.Length - 1];
                    if (BlowfishECB.BLOCK_SIZE < pval)
                    {
                        throw new CryptographicException();
                    }
                    rest = tmp.Length - pval;
                    for (int i = tmp.Length - 2; i >= rest; i--)
                    {
                        if (tmp[i] != 0)
                        {
                            throw new CryptographicException();
                        }
                    }
                }
                else
                {
                    throw new CryptographicException();
                }

                if (tmp == this.block)
                {
                    result = new byte[rest];
                    Array.Copy(tmp, 0, result, 0, rest);
                }
                else
                {
                    result = new byte[BlowfishECB.BLOCK_SIZE + rest];
                    Array.Copy(this.block, 0, result, 0, BlowfishECB.BLOCK_SIZE);
                    Array.Copy(tmp, 0, result, BlowfishECB.BLOCK_SIZE, rest);
                }
            }

            reset();

            return result;
        }

        void reset()
        {
            if (null != this.bfc)
            {
                this.bfc.SetIV(this.origIV, 0);
            }
            this.block = null;
        }
    }
}
