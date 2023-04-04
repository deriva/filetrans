using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Common.Utilities
{
    public static class StringExtensions
    {
        #region 字符串和数字转换
        /// <summary>
        /// 转换decimal,并指定保留N位小数
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <param name="digtal"></param>
        /// <returns></returns>
        public static decimal ToDecimal2(this object s, decimal? def = null, int digtal = 2)
        {
            decimal result = 0;
            if (s == null && def != null) return def.Value;
            decimal.TryParse(s.ToString(), out result);
            return Math.Round(result, digtal, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 转换decimal,并指定保留N位小数
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <param name="digtal"></param>
        /// <returns></returns>
        public static int ToInt2(this object s, int? def = null)
        {
            int result = 0;
            if (s == null && def != null) return def.Value;
            int.TryParse(s.ToString(), out result);
            return result;
        }
        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string s)
        {
            if (s == null) return true;
            if (s.Length == 0) return true;
            return false;
        }
        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string SubLen(this string s,int len)
        {
            if (s == null) return "";
            if (s.Length == 0) return "";
            if (s.Length < len) return s;
            return s.Substring(0,len);
        }

 
        /// <summary>
        ///  转换为string字符串类型
        /// </summary>
        /// <param name="s">获取需要转换的值</param>
        /// <param name="format">需要格式化的位数</param>
        /// <returns>返回一个新的字符串</returns>
        public static string ToStr(this object s, string format = "")
        {
            string result = "";
            try
            {
                if (s == null) return "";
                if (string.IsNullOrWhiteSpace(format))
                {
                    result = s.ToString();
                }
                else
                {
                    result = string.Format("{0:" + format + "}", s);
                }
            }
            catch
            {
            }
            return result;
        }
        public static bool ToBoolen2(this object obj)
        {
            return  bool.Parse(obj.ToString());
        }
        /// <summary>
        ///     返回DateTime类型,精确到秒
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this object obj)
        {
            if (obj == null)
                return null;
            var str = obj.ToString();

            DateTime reOut;
            if (DateTime.TryParse(str, out reOut))
                return reOut;
            return null;
        }
        /// <summary>
        /// 将DateTime?转换成DateTime，如为空则返回1900-01-01 00:00:00
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToDateTime2(this DateTime? dt)
        {
            if (dt == null)
            {
                return DateTime.Parse("1900-01-01 00:00:00");
            }
            else
            {
                return DateTime.Parse(dt.ToStr("yyyy-MM-dd HH:mm:ss.fff"));
            }
        }
        #endregion

        #region 加密类

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        public static string ToEncrySalry(this string s, string key1, string key2)
        {
            if (s.IsEmpty()) return "";
            if (key1.Length < 8) key1 = key1.PadLeft(8, '0');
            if (key2.Length < 8) key2 = key2.PadLeft(8, '0');
            var ss = PasswordUtil.Encode(s, key1, key2);
            return ss;

        }
        /// <summary>
        /// 解密密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        public static string ToDecrySalry(this string s, string key1, string key2)
        {
            if (s.IsEmpty()) return "";
            if (key1.Length < 8) key1 = key1.PadLeft(8, '0');
            if (key2.Length < 8) key2 = key2.PadLeft(8, '0');
            return PasswordUtil.Decode(s, key1, key2);
        }

        #endregion

        #region 时间类

        /// <summary>
        /// DateTime转换为13位时间戳（单位：毫秒）
        /// </summary>
        /// <param name="dateTime"> DateTime</param>
        /// <returns>13位时间戳（单位：毫秒）</returns>
        public static long DateTimeToLongTimeStamp(this DateTime dateTime)
        {
            var timeStampStartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(dateTime.ToUniversalTime() - timeStampStartTime).TotalMilliseconds;
        }

        /// <summary>
        /// 13位时间戳（单位：毫秒）转换为DateTime
        /// </summary>
        /// <param name="longTimeStamp">13位时间戳（单位：毫秒）</param>
        /// <returns>DateTime</returns>
        public static DateTime LongTimeStampToDateTime(long longTimeStamp)
        {
            var timeStampStartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return timeStampStartTime.AddMilliseconds(longTimeStamp).ToLocalTime();
        }
        #endregion



    }
}
