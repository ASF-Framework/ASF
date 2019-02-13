using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ASF.Internal.Security
{
    public static class RSA
    {
        //encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
        private static byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
        private static byte[] Seq = new byte[15];

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="content">签名内容</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="encoding">编码类型</param>
        /// <param name="hashAlgorithm">加密方式</param>
        /// <returns>加密密文</returns>
        public static string Sign(string content, string privateKey, Encoding encoding, HashAlgorithmName hashAlgorithm)
        {
            var rsa = System.Security.Cryptography.RSA.Create();
            rsa.ImportParameters(DecodePkcsPrivateKey(privateKey));
            var contentBytes = encoding.GetBytes(content);
            var cipherBytes = rsa.SignData(contentBytes, hashAlgorithm, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(cipherBytes);
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="content">验证签名内容</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="sign">签名密文</param>
        /// <param name="encoding">编码类型</param>
        /// <param name="hashAlgorithm">加密方式</param>
        /// <returns>加密密文</returns>
        public static bool CheckSign(string content, string publicKey, string sign, Encoding encoding, HashAlgorithmName hashAlgorithm)
        {
            var rsa = System.Security.Cryptography.RSA.Create();
            rsa.ImportParameters(DecodePkcsPublicKey(publicKey));
            var contentBytes = encoding.GetBytes(content);
            var signBytes = Convert.FromBase64String(sign);
            return rsa.VerifyData(contentBytes, signBytes, hashAlgorithm, RSASignaturePadding.Pkcs1);

        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="cipher">密文</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="encoding">编码</param>
        /// <returns>明文</returns>
        public static string Decrypt(string cipher, string privateKey, Encoding encoding)
        {
            var rsa = System.Security.Cryptography.RSA.Create();
            rsa.ImportParameters(DecodePkcsPrivateKey(privateKey));
            var cipherBytes = System.Convert.FromBase64String(cipher);
            var plainTextBytes = rsa.Decrypt(cipherBytes, RSAEncryptionPadding.Pkcs1);
            return encoding.GetString(plainTextBytes);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainText ">明文</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="encoding">编码</param>
        /// <returns>密文</returns>
        public static string Encrypt(string plainText, string publicKey, Encoding encoding)
        {
            var rsa = System.Security.Cryptography.RSA.Create();
            rsa.ImportParameters(DecodePkcsPublicKey(publicKey));
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = rsa.Encrypt(plainTextBytes, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(cipherBytes);
        }

        public static RSAParameters DecodePkcsPublicKey(string publicKey)
        {
            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentNullException("publicKey", "This arg cann't be empty.");

            publicKey = publicKey.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\n", "").Replace("\r", "");
            var publicKeyData = Convert.FromBase64String(publicKey);
            //生成RSA参数
            var rsaParams = new RSAParameters();

            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  -----
            using (BinaryReader binr = new BinaryReader(new MemoryStream(publicKeyData)))
            {
                byte bt = 0;
                ushort twobytes = 0;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();   //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();  //advance 2 bytes
                else
                    throw new ArgumentException("PemToXmlPublicKey Conversion failed");

                Seq = binr.ReadBytes(15);       //read the Sequence OID
                if (!CompareBytearrays(Seq, SeqOID))    //make sure Sequence for OID is correct
                    throw new ArgumentException("PemToXmlPublicKey Conversion failed");

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8203)
                    binr.ReadInt16();   //advance 2 bytes
                else
                    throw new ArgumentException("PemToXmlPublicKey Conversion failed");

                bt = binr.ReadByte();
                if (bt != 0x00)     //expect null byte next
                    throw new ArgumentException("PemToXmlPublicKey Conversion failed");

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();   //advance 2 bytes
                else
                    throw new ArgumentException("PemToXmlPublicKey Conversion failed");

                twobytes = binr.ReadUInt16();
                byte lowbyte = 0x00;
                byte highbyte = 0x00;

                if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                    lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
                else if (twobytes == 0x8202)
                {
                    highbyte = binr.ReadByte(); //advance 2 bytes
                    lowbyte = binr.ReadByte();
                }
                else
                    throw new ArgumentException("PemToXmlPublicKey Conversion failed");
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                int modsize = BitConverter.ToInt32(modint, 0);

                int firstbyte = binr.PeekChar();
                if (firstbyte == 0x00)
                {   //if first byte (highest order) of modulus is zero, don't include it
                    binr.ReadByte();    //skip this null byte
                    modsize -= 1;   //reduce modulus buffer size by 1
                }

                byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes

                if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
                    throw new ArgumentException("PemToXmlPublicKey Conversion failed");
                int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                byte[] exponent = binr.ReadBytes(expbytes);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                rsaParams.Modulus = modulus;
                rsaParams.Exponent = exponent;
            }
            return rsaParams;
        }




        public static RSAParameters DecodePkcsPrivateKey(string privateKey)
        {

            if (string.IsNullOrEmpty(privateKey))
            {
                throw new ArgumentNullException("pemFileConent", "This arg cann't be empty.");
            }
            try
            {
                privateKey = privateKey.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "").Replace("\n", "").Replace("\r", "");
                var privateKeyData = Convert.FromBase64String(privateKey);

                //解析Pkcs证书
                PKCSType type = GetPrivateKeyType(privateKeyData.Length);
                if (type == PKCSType.PKCS_8_1024 || type == PKCSType.PKCS_8_2048)
                {
                    //Pkcs#8秘钥需要特殊处理
                    privateKeyData = DecodePkcs8PrivateKey(privateKeyData);
                }
                var rsaParams = new RSAParameters();
                byte bt = 0;
                ushort twobytes = 0;
                //转换为二进制值
                using (var binr = new BinaryReader(new MemoryStream(privateKeyData)))
                {
                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        throw new ArgumentException("Unexpected value read )");

                    twobytes = binr.ReadUInt16();
                    if (twobytes != 0x0102)
                        throw new ArgumentException("Unexpected version");

                    bt = binr.ReadByte();
                    if (bt != 0x00)
                        throw new ArgumentException("Unexpected value read ");

                    //转换XML
                    rsaParams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                    rsaParams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                    rsaParams.D = binr.ReadBytes(GetIntegerSize(binr));
                    rsaParams.P = binr.ReadBytes(GetIntegerSize(binr));
                    rsaParams.Q = binr.ReadBytes(GetIntegerSize(binr));
                    rsaParams.DP = binr.ReadBytes(GetIntegerSize(binr));
                    rsaParams.DQ = binr.ReadBytes(GetIntegerSize(binr));
                    rsaParams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
                }
                return rsaParams;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("此私钥证书无效", ex);
            }
        }

        /// <summary>
        /// Pkcs#8 证书解密
        /// </summary>
        /// <param name="privateKeyData"></param>
        /// <returns></returns>
        private static byte[] DecodePkcs8PrivateKey(byte[] privateKeyData)
        {
            byte bt = 0;
            ushort twobytes = 0;
            MemoryStream mem = new MemoryStream(privateKeyData);
            int lenstream = (int)mem.Length;
            using (var binr = new BinaryReader(mem))
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)    //data read as little endian order (actual data order for Sequence is 30 81)    
                    binr.ReadByte();    //advance 1 byte    
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes    
                else
                    throw new Exception("Unexpected value read");

                bt = binr.ReadByte();
                if (bt != 0x02)
                    throw new Exception("Unexpected version");

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)
                    throw new Exception("Unexpected value read");

                Seq = binr.ReadBytes(15);        //read the Sequence OID    
                if (!CompareBytearrays(Seq, SeqOID))    //make sure Sequence for OID is correct    
                    throw new Exception("Unexpected value read");

                bt = binr.ReadByte();
                if (bt != 0x04)    //expect an Octet string    
                    throw new Exception("Unexpected value read");

                bt = binr.ReadByte();        //read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count    
                if (bt == 0x81)
                    binr.ReadByte();
                else
                    if (bt == 0x82)
                    binr.ReadUInt16();
                //------ at this stage, the remaining sequence should be the RSA private key    

                return binr.ReadBytes((int)(lenstream - mem.Position));
            }
        }

        /// <summary>
        /// 获取Integer的大小
        /// </summary>
        /// <param name="binr"></param>
        /// <returns></returns>
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }
        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        /// <summary>
        /// 获取私钥的类型
        /// </summary>
        /// <param name="privateKeyLength"></param>
        /// <returns></returns>
        private static PKCSType GetPrivateKeyType(int privateKeyLength)
        {
            if (privateKeyLength >= 630 && privateKeyLength <= 640)
                return PKCSType.PKCS_8_1024;
            if (privateKeyLength >= 600 && privateKeyLength <= 610)
                return PKCSType.PKCS_1_1024;
            if (privateKeyLength >= 1210 && privateKeyLength <= 1220)
                return PKCSType.PKCS_8_2048;
            if (privateKeyLength >= 1190 && privateKeyLength <= 1199)
                return PKCSType.PKCS_1_2048;
            else
                throw new ArgumentException("此私钥证书标准不支持");
        }

    }

    public enum PKCSType
    {
        PKCS_1_1024,
        PKCS_1_2048,
        PKCS_8_1024,
        PKCS_8_2048,
    }

}
