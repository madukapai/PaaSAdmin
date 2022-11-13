using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.Common
{
    /// <summary>
    /// 加密與編碼的共用模組
    /// </summary>
    public class Encrypt
    {
        /// <summary>
        /// 透過DES進行加密的動作
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="strDesKey">Key</param>
        /// <returns></returns>
        public string DESEncrypt(string strSource, string strDesKey)
        {
            StringBuilder sb = new StringBuilder();
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(strDesKey);
            byte[] iv = Encoding.ASCII.GetBytes(strDesKey);
            byte[] dataByteArray = Encoding.UTF8.GetBytes(strSource);

            des.Key = key;
            des.IV = iv;
            string encrypt = "";
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dataByteArray, 0, dataByteArray.Length);
                cs.FlushFinalBlock();
                //輸出資料
                foreach (byte b in ms.ToArray())
                {
                    sb.AppendFormat("{0:X2}", b);
                }
                encrypt = sb.ToString();
            }
            return encrypt;
        }

        /// <summary>
        /// 透過DES進行解密的動作
        /// </summary>
        /// <param name="encrypt"></param>
        /// <param name="strDesKey">Key</param>
        /// <returns></returns>
        public string DESDecrypt(string encrypt, string strDesKey)
        {
            string strDecodeValue = "";

            try
            {
                byte[] dataByteArray = new byte[encrypt.Length / 2];
                for (int x = 0; x < encrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(encrypt.Substring(x * 2, 2), 16));
                    dataByteArray[x] = (byte)i;
                }

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] key = Encoding.ASCII.GetBytes(strDesKey);
                byte[] iv = Encoding.ASCII.GetBytes(strDesKey);
                des.Key = key;
                des.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        strDecodeValue = Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch
            {
                strDecodeValue = "";
            }

            return strDecodeValue;
        }

        /// <summary>
        /// 透過DES進行加密的動作
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public string DESEncrypt(string strSource)
        {
            string strDesKey = ConfigurationManager.AppSettings["DESKey"].ToString();
            return this.DESEncrypt(strSource, strDesKey);
        }

        /// <summary>
        /// 透過DES進行解密的動作
        /// </summary>
        /// <param name="encrypt"></param>
        /// <returns></returns>
        public string DESDecrypt(string encrypt)
        {
            string strDesKey = ConfigurationManager.AppSettings["DESKey"].ToString();
            return this.DESDecrypt(encrypt, strDesKey);
        }

        /// <summary>
        /// SHA256編碼
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public string SHA256Encrypt(string strSource)
        {
            byte[] source = Encoding.Default.GetBytes(strSource);
            byte[] crypto = new SHA256CryptoServiceProvider().ComputeHash(source);
            string result = string.Empty;

            for (int i = 0; i < crypto.Length; i++)
            {
                result += crypto[i].ToString("X2");
            }

            return result.ToUpper();
        }

        /// <summary>
        /// SHA512編碼
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public string SHA512Encrypt(string strSource)
        {
            string strHash = "";
            byte[] source = Encoding.UTF8.GetBytes(strSource);//將字串轉為Byte[]

            using (SHA512 shaM = new SHA512Managed())
            {
                var hashValue = shaM.ComputeHash(source);
                foreach (byte x in hashValue)
                {
                    strHash += String.Format("{0:x2}", x);
                }
            }

            return strHash;
        }

        /// <summary>
        /// MD5加密的動作
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public string MD5Encrypt(string strSource)
        {
            MD5 md5 = MD5.Create();
            byte[] encodedPassword = new UTF8Encoding().GetBytes(strSource);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            return encoded;
        }

        /// <summary>
        /// 字串加密(非對稱式)
        /// </summary>
        /// <param name="SourceStr">加密前字串</param>
        /// <param name="CryptoKey">加密金鑰</param>
        /// <returns>加密後字串</returns>
        public string AESEncrypt(string SourceStr, string CryptoKey)
        {
            string encrypt = "";
            try
            {
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(CryptoKey));
                byte[] iv = md5.ComputeHash(Encoding.UTF8.GetBytes(CryptoKey));
                aes.Key = key;
                aes.IV = iv;

                byte[] dataByteArray = Encoding.UTF8.GetBytes(SourceStr);
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(ms.ToArray());
                }
            }
            catch
            {

            }
            return encrypt;
        }

        /// <summary>
        /// 字串解密(非對稱式)
        /// </summary>
        /// <param name="SourceStr">解密前字串</param>
        /// <param name="CryptoKey">解密金鑰</param>
        /// <returns>解密後字串</returns>
        public string AESDecrypt(string SourceStr, string CryptoKey)
        {
            string decrypt = "";
            try
            {
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(CryptoKey));
                byte[] iv = md5.ComputeHash(Encoding.UTF8.GetBytes(CryptoKey));
                aes.Key = key;
                aes.IV = iv;

                byte[] dataByteArray = Convert.FromBase64String(SourceStr);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception)
            {

            }
            return decrypt;
        }
    }
}
