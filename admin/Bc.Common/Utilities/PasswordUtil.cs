/*
* ==============================================================================
*
* FileName: PasswordUtil.cs
* Created: 2020/5/19 18:14:35
* Author: Meiam
* Description: 
*
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Bc.Common.Utilities
{
    public class PasswordUtil
    {
        public const string secretKey = "MEIAM";

        /// <summary>
        /// 对比用户明文密码是否和加密后密码一致
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="dbPassword">数据库中单向加密后的密码</param>
        /// <param name="userPassword">用户明文密码</param>
        /// <returns></returns>
        public static bool ComparePasswords(string userId, string dbPassword, string userPassword)
        {
            string userPwd = CreateMD5(CreateMD5(userPassword) + secretKey + userId);

            if (userPwd.Length == 0 || string.IsNullOrEmpty(userPwd) || dbPassword != userPwd)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// 创建用户的数据库密码
        /// </summary>
        /// <param name="userName">用户ID</param>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        public static string CreateDbPassword(string userId, string userPassword)
        {
            return CreateMD5(CreateMD5(userPassword) + secretKey + userId);
        }


        #region 私有函数
        private static string CreateMD5(string pwd)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(pwd)));
            t2 = t2.Replace("-", "");
            t2 = t2.ToUpper();
            return t2;
        }

        #endregion




        #region 另一种加密
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="KEY_64">8位长度</param>
        /// <param name="IV_64">8位长度</param>
        /// <returns></returns>
        public static string Encode(string data, string KEY_64, string IV_64)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey,

byIV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="KEY_64">8位长度</param>
        /// <param name="IV_64">8位长度</param>
        /// <returns></returns>
        public static string Decode(string data, string KEY_64, string IV_64)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey,

byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }

        #endregion
    }
}
