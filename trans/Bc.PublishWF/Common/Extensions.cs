using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;

namespace Bc.PublishWF.Common
{
    /// <summary>
    ///     扩展方法
    /// </summary>
    public static class Extensions
    {
        private static readonly JavaScriptSerializer jss = new JavaScriptSerializer();

        /// <summary>
        ///     将Object的值根据泛型转换成相应的类型的值
        ///     如果转换时发生异常，会抛出异常信息
        /// </summary>
        /// <typeparam name="T">要转换的类型</typeparam>
        /// <param name="value">要转换的值</param>
        /// <param name="defaultValue">转换失败的默认值(可以不填)</param>
        /// <returns></returns>
        public static T ChangeValue<T>(this object value, T defaultValue = default(T))
        {
            if (value == null) throw new ArgumentNullException("对象不能为null");
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (FormatException ex)
            {
                //异常写入日志中

                if (defaultValue == null)
                    throw ex;
            }
            return (T)Convert.ChangeType(defaultValue, typeof(T));
        }

        /// <summary>
        ///     将Object的值根据泛型转换成相应的类型的值
        ///     如果转换时发生异常，会返回相应的类型的默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ChangeValueDefault<T>(this object value, T defaultValue = default(T))
        {
            if (value == null)
                return default(T);
            try
            {
                return ChangeValue(value, defaultValue);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return default(T);
        }

        /// <summary>
        ///     判断2个字符串是否相等（忽略大小写）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string value, string param)
        {
            return string.Equals(value, param, StringComparison.CurrentCultureIgnoreCase);
        }

        public static string ReplaceStartsWith(this string input, string replace, string to)
        {
            var retval = input;
            if (!string.IsNullOrEmpty(input) && !string.IsNullOrEmpty(replace) && input.StartsWith(replace))
            {
                retval = to + input.Substring(replace.Length);
            }
            return retval;
        }

        public static string Trims(this string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                return val.Trim();
            }
            return "";
        }

        /// <summary>
        ///     将一个对象序列化成 JSON 格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            if (obj == null)
                return string.Empty;

            return jss.Serialize(obj);
        }

        /// <summary>
        ///     从JSON字符串中反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return default(T);

            return jss.Deserialize<T>(obj);
        }

        /// <summary>
        ///     编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Escape(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                sb.Append((char.IsLetterOrDigit(c)
                           || c == '-' || c == '_' || c == '\\'
                           || c == '/' || c == '.')
                    ? c.ToString()
                    : Uri.HexEscape(c));
            }
            return sb.ToString();
        }

        public static string EscapeZH(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                var temp = string.Empty;
                if (char.IsLetterOrDigit(c)
                    || c == '-' || c == '_' || c == '\\'
                    || c == '/' || c == '.')
                    temp = c.ToString();
                else
                {
                    int charInt = c;
                    temp = "\\u" + Convert.ToString(charInt, 16);
                }
                sb.Append(temp);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     UNICODE 支持汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnEscapeZH(this string str)
        {
            var outstr = "";
            if (!string.IsNullOrEmpty(str) && str.IndexOf('u') > -1)
            {
                var strlist = str.Replace("%", "").Split('u');
                try
                {
                    for (var i = 1; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符
                        outstr += (char)int.Parse(strlist[i].Substring(0, 4), NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outstr = ex.Message;
                }
            }
            else
            {
                return str;
            }
            return outstr;
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnEscape(this string str)
        {
            if (str == null)
                return string.Empty;

            var sb = new StringBuilder();
            var len = str.Length;
            var i = 0;
            while (i != len)
            {
                if (Uri.IsHexEncoding(str, i))
                    sb.Append(Uri.HexUnescape(str, ref i));
                else
                    sb.Append(str[i++]);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     返回long类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long? ToLong(this string str, long? defaultVal = null)
        {
            long reOut;
            if (long.TryParse(str, out reOut))
                return reOut;
            return defaultVal;
        }

        /// <summary>
        ///     对象是否 空
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsNull(this object val)
        {
            if (val == null || val == DBNull.Value)
                return true;
            return false;
        }

        /// <summary>
        ///     对象是否 空
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsEmpty(this object val)
        {
            if (val == null || val == DBNull.Value)
                return true;
            return false;
        }

        /// <summary>
        ///     对象是否 不为空
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this object val)
        {
            if (val != null && val != DBNull.Value && val != string.Empty)
                return true;
            return false;
        }

        /// <summary>
        ///     返回int类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int? ToInt(this object str, int? defaultVal = null)
        {
            try
            {
                int reOut;
                if (str.IsEmpty())
                {
                    return defaultVal;
                }
                if (int.TryParse(str.ObjectToString(), out reOut))
                    return reOut;

                var db = Convert.ToInt32(str.ToDouble());
                return db;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                return 0;
            }
        }

        public static double ToDouble(this object str, double defaultVal = 0)
        {
            double reOut;
            if (str.IsEmpty())
            {
                return defaultVal;
            }
            if (double.TryParse(str.ObjectToString(), out reOut))
                return reOut;
            return defaultVal;
        }

        public static int? ToInt(this object obj)
        {
            return obj.ToInt(null);
        }

        public static int ToInt2(this object obj)
        {
            return obj.ToInt(0).Value;
        }
        public static decimal DoubleToDecimal(this object str, decimal defaultVal = 0)
        {
            decimal reOut;
            if (str.IsEmpty())
            {
                return 0;
            }
            if (decimal.TryParse(str.ObjectToString(), out reOut))
                return reOut;
            return defaultVal;
        }
        public static short ToShort(this object obj)
        {
            return Convert.ToInt16(obj);
        }

        public static bool? ToBoolen(this object str, bool? defaultVal = null)
        {
            if (str.IsEmpty())
            {
                return defaultVal;
            }
            var re = false;
            if (bool.TryParse(str.ObjectToString(), out re))
                return re;
            return defaultVal;
        }

        public static bool? ToBoolen(this object obj)
        {
            return obj.ToBoolen(null);
        }

        public static bool ToBoolen2(this object obj)
        {
            return obj.ToBoolen(false).Value;
        }
        public static decimal ToDecimal2(this object obj, decimal def = 0)
        {
            try
            {
                if (obj == null) return def;
                return obj.ToString().ToDecimal(def).Value;

            }
            catch
            {
                return def;
            }
        }
        public static string ToDataRow(this System.Data.DataRow[] obj, string column, int index = 0)
        {
            try
            {
                if (obj == null) return "";
                if (obj.Length > 0 && obj.Length > index)
                {
                    return obj[index][column].ToStr();
                }
                return "";

            }
            catch
            {
                return "";
            }
        }

        public static string ToBoolStrCn(this object obj)
        {
            if (obj != null)
            {
                if (obj.GetType().Name.ToLower().Contains("int"))
                {
                    return obj.ToInt2() == 1 ? "是" : "否";
                }
                return obj.ToBoolen(false).Value ? "是" : "否";
            }
            return "否";
        }
        public static string ReplaceAll(this string obj, string source, string desc)
        {
            var arr = obj.Split(new string[] { source }, StringSplitOptions.RemoveEmptyEntries);
            var result = obj;
            for (var i = 0; i < arr.Length + 2; i++)
            {
                result = obj.ToString().Replace(source, desc);
            }
            return result;

        }
        /// <summary>
        /// 将制定的字符全部替换为空
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cc"></param>
        /// <returns></returns>
        public static string ReplaceAll(this string obj, List<string> cc)
        {
            var result = obj;
            foreach (var it in cc)
            {
                var arr = obj.Split(new string[] { it }, StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < arr.Length + 2; i++)
                {
                    result = obj.ToString().Replace(it, "");
                }
            }
            return result;

        }
 

        /// <summary>
        /// 默认保留2位小数
        /// </summary>
        /// <param name="dig">默认保留2位小数</param>
        /// <param name="digtal"></param>
        /// <returns></returns>
        public static decimal ToDecimalByDig(this object dig, int digtal = 2)
        {
            var dec = dig.ToDecimal2(0);
            return Math.Round(dec, digtal, MidpointRounding.AwayFromZero);
        }
      
        /// <summary>
        /// 默认保留2位小数 不四舍五入
        /// </summary>
        /// <param name="dig">默认保留2位小数</param>
        /// <param name="digtal"></param>
        /// <returns></returns>
        public static decimal ToDecimalByDig2(this decimal dig, int digtal = 2)
        {
            string numToString = dig.ToString();

            int index = numToString.IndexOf(".");
            int length = numToString.Length;

            if (index != -1)
            {
                return Convert.ToDecimal(string.Format("{0}.{1}",
                  numToString.Substring(0, index),
                  numToString.Substring(index + 1, Math.Min(length - index - 1, digtal))));
            }
            else
            {
                return dig;
            }
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

        /// <summary>
        ///     转换时间类型
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDateString(this object obj)
        {
            var date = obj.ToDateTime();
            if (date == null)
                return string.Empty;
            return date.Value.ToString("yyyy年MM月dd日");
        }

        /// <summary>
        ///     短时间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToShortDateString(this object obj)
        {
            var date = obj.ToDateTime();
            if (date == null)
                return string.Empty;
            return date.Value.ToString("yyyy-MM-dd");
        }

        /// <summary>
        ///     显示为分钟时间格式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToTimeMinuteString(this object obj)
        {
            var date = obj.ToDateTime();
            if (date == null)
                return string.Empty;
            return date.Value.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 显示年份为短年份，只有分钟 如 18-05-30 16:29
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToTimeShortYearString(this object obj)
        {
            var date = obj.ToDateTime();
            if (date == null)
                return string.Empty;
            return date.Value.ToString("yy-MM-dd HH:mm");
        }

        /// <summary>
        ///     显示为分钟时间格式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToTimeSecondString(this object obj)
        {
            var date = obj.ToDateTime();
            if (date == null)
                return string.Empty;
            return date.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        ///     返回Date 类型,精确到天
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ToDate(this string str)
        {
            DateTime reOut;
            if (DateTime.TryParse(str, out reOut))
                return reOut.Date;
            return null;
        }


        /// <summary>
        /// 时间转时间戳
        /// </summary>
        public static long ToTimestamp(this DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return Convert.ToInt64((time - startTime).TotalMilliseconds);
        }

        /// <summary>
        /// 时间戳转时间  (适合时间戳是毫秒数的情况）
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long timestamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = new TimeSpan(timestamp * 10000);
            return dtStart.Add(toNow);
        }

        /// <summary>
        ///     返回Date 类型,精确到天，无法转换用当天值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateWithDefault(this string str)
        {
            DateTime reOut;
            if (DateTime.TryParse(str, out reOut))
                return reOut.Date;
            return DateTime.Now;
        }

        public static List<int> ToListInt(this List<string> strList)
        {
            var list = new List<int>();
            foreach (var str in strList)
            {
                if (str.ToInt().HasValue)
                    list.Add(str.ToInt().Value);
            }
            return list;
        }

        public static List<long> ToListLong(this List<string> strList)
        {
            var list = new List<long>();
            foreach (var str in strList)
            {
                if (str.ToLong().HasValue)
                    list.Add(str.ToLong().Value);
            }
            return list;
        }

       
        /// <summary>
        ///     转换为字符串，null时转换为string.Empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToNullString(this string str)
        {
            return str == null ? string.Empty : str.Trim();
        }

        /// <summary>
        ///     转换为字符串，null或空时转换为默认字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToStringWithDefault(this string str, string defaultStr)
        {
            return string.IsNullOrEmpty(str) ? defaultStr : str;
        }

        /// <summary>
        ///     返回byte类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte? ToByte(this string str, byte? defaultVal = null)
        {
            byte reOut;
            if (byte.TryParse(str, out reOut))
                return reOut;
            return defaultVal;
        }

        /// <summary>
        ///     全角数字转英文数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToNumberString(this string str)
        {
            var re = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("０", "0").Replace("１", "1").Replace("２", "2").Replace("３", "3").Replace("４", "4");
                str = str.Replace("５", "5").Replace("６", "6").Replace("７", "7").Replace("８", "8").Replace("９", "9");
                str = str.Replace("，", ",").Replace("。", ".");
                re = str;
            }
            return re;
        }

        /// <summary>
        ///     整型变时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeToTime(this string timeStamp)
        {
            var dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var lTime = long.Parse(timeStamp + "0000000");
            var toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        ///     转换为long时间,用于微信支付接口
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public static long ToUniversalTimeLong(this DateTime now)
        {
            return Convert.ToInt64((now.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        }

        /// <summary>
        ///     取字典返回值
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDictValue(this Dictionary<string, string> dict, string key)
        {
            if (dict.ContainsKey(key))
                return dict[key];
            return null;
        }

        /// <summary>
        ///     时间变整型
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int DateTimeToInt(this DateTime time)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        ///     转化为字符串，空时改化为string.Empty
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToString(this object obj)
        {
            if (obj == null)
                return string.Empty;
            return obj.ToString();
        }

        /// <summary>
        ///     转为为Decimal数据类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal? ToDecimal(this string str, decimal? defaultVal = null)
        {
            if (str == null)
                return defaultVal;
            decimal re;
            if (decimal.TryParse(str, out re))
            {
                return re;
            }
            return defaultVal;
        }

        /// <summary>
        ///     转为为Decimal数据类型(保留2位小数)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal? ToDecimalN2(this string str, decimal? defaultVal = null)
        {
            if (str == null)
                return Math.Round(defaultVal.Value, 2); ;
            decimal re;
            if (decimal.TryParse(str, out re))
            {
                return Math.Round(re, 2);
            }
            return Math.Round(defaultVal.Value, 2);
        }

        public static string FormatDate(this DateTime date)
        {
            return FormatDate(date, "yyyy-MM-dd");
        }

        public static string FormatDate(this DateTime date, string formatString)
        {
            if (date != null)
            {
                var dNow = DateTime.Now;
                var time = dNow - date;

                if (time.Days > 28)
                {
                    return date.ToString(formatString);
                }
                if (time.Days > 0)
                {
                    return time.Days + "天前";
                }
                if (time.Hours > 0)
                {
                    return time.Hours + "小时前";
                }
                if (time.Minutes > 0)
                {
                    return time.Minutes + "分钟前";
                }
                if (time.Seconds > 0)
                {
                    return time.Seconds + "秒前";
                }
                return "刚刚";
            }
            return "";
        }

        /// <summary>
        ///     转货为缩略图地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ToThumbImageUrl(this string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;
            if (url.Length > 3 && url.ToLower().IndexOf("_thumb") == -1)
            {
                var index = url.LastIndexOf(".");
                url = url.Substring(0, index) + "_Thumb" + url.Substring(index, url.Length - index);
                return url;
            }
            return url;
        }

       
        /// <summary>
        ///     转换为绝对地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ToAbsoluteUrl(this string url)
        {
            var baseUrl = ConfigurationManager.AppSettings["IhoomeWeiSite.Url"];
            if (string.IsNullOrEmpty(url))
                baseUrl = "http://wx.ihoome.com";

            var re = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                if (url.Length > 5)
                {
                    if (url.Substring(0, 5).ToLower().Trim() == "http:")
                        re = url;
                    else
                        re = baseUrl + url;
                }
            }

            return re.ToLower();
        }

        public static string CodeToName(this object obj, string fm)
        {
            if (string.IsNullOrEmpty(fm))
                throw new Exception("CodeToName(this object obj, string fm) fm参数格式不对!");

            var arr = fm.Split(';');
            var defaultVal = arr[0].Split(',')[1];

            if (obj == null || obj == DBNull.Value)
            {
                return string.Empty;
            }

            var str = obj.ToString();
            if (string.IsNullOrEmpty(str))
                return defaultVal;

            try
            {
                var hash = new Hashtable();
                for (var i = 0; i < arr.Length; i++)
                {
                    var key = arr[i].Split(',')[0].ToUpper().Trim();
                    var val = arr[i].Split(',')[1];

                    if (!hash.ContainsKey(key))
                        hash.Add(key, val);
                }
                return hash[obj.ToString().ToUpper().Trim()].ToString();
            }
            catch
            {
                return defaultVal;
            }
        }

        /// <summary>
        ///     取URL后面的参数的键值
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetUrlParamByKey(this string url, string key)
        {
            var dict = new Dictionary<string, string>();
            if (url.Split('?').Length >= 2)
            {
                var paramsStr = url.Split('?')[1];

                var arrayOneParm = paramsStr.Split('&');
                foreach (var oneParm in arrayOneParm)
                {
                    var array = oneParm.Split('=');
                    if (array.Length >= 1)
                    {
                        var dictKey = array[0];
                        var val = array.Length >= 2 ? array[1] : string.Empty;

                        if (!dict.ContainsKey(dictKey))
                            dict.Add(dictKey, val);
                    }
                }
            }

            return dict.ContainsKey(key) ? dict[key] : null;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        ///     取前一个工作日，例：即周四取周三，周一取前一周周五
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetPreWorkDate(this DateTime date)
        {
            var dt = date.Date.AddDays(-1);
        stardo:
            if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
            {
                dt = dt.AddDays(-1);
                goto stardo;
            }
            return dt;
        }

        /// <summary>
        ///     日期转字符串
        /// </summary>
        /// <param name="source">日期对象</param>
        /// <param name="format">字符串格式</param>
        /// <returns>字符串</returns>
        public static string DataTimeToString(this object source, string format = "yyyy-MM-dd HH:mm")
        {
            return source == null ? string.Empty : Convert.ToDateTime(source).ToString(format);
        }

        public static string RemoveHtmlEmpty(this string str)
        {
            return str.ObjectToString().Replace("&nbsp;", "").Replace("/r", "").Replace("/n", "").Trim();
        }

        public static string ToConvertHtml(this string Str)
        {
            Str = Str.ReplaceAll("&amp", "&");
            Str = Str.ReplaceAll("&lt", "<");
            Str = Str.ReplaceAll("&gt", ">");
            return Str.ReplaceAll("&nbsp;", " ").Trim();
        }

        /// <summary>
        ///     移除最后的字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="endStr"></param>
        /// <returns></returns>
        public static string RemoveEndChar(this string str, string endStr)
        {
            if (str.Length > endStr.Length)
            {
                var len = endStr.Length;
                var lastStr = str.Substring(str.Length - len, len);
                if (lastStr == endStr)
                {
                    return str.Substring(0, str.Length - len);
                }
                return str;
            }
            if (str.Length == endStr.Length)
            {
                return str.Replace(endStr, "");
            }
            return str;
        }

        /// <summary>
        ///     SQL拼串安全字符检查
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static string SqlSensitivityCheck(this string contents)
        {
            if (contents.Length > 0)
            {
                contents = contents.ObjectToString().Replace("'", "");
                //convert to lower
                var sLowerStr = contents.ToLower();
                //RegularExpressions
                var sRxStr = @"(\sand\s)|(\sand\s)|(\slike\s)|(select\s)|(insert\s)|
(delete\s)|(update\s[\s\S].*\sset)|(create\s)|(\stable)|(<[iframe|/iframe|script|/script])|
(')|(\sexec)|(\sdeclare)|(\struncate)|(\smaster)|(\sbackup)|(\smid)|(\scount)";
                //Match
                var bIsMatch = false;
                var sRx = new
                    Regex(sRxStr);
                bIsMatch = sRx.IsMatch(sLowerStr, 0);
                if (bIsMatch)
                    return string.Empty;
                return contents;
            }
            return contents;
        }

        public static void UpdateDictionary(this Dictionary<string, string> source, string key, string newVal)
        {
            if (source.ContainsKey(key))
            {
                source.Remove(key);
                source.Add(key, newVal);
            }
        }



     
        public static string ToUtf8(this string str)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(str);
            String decodedString = utf8.GetString(encodedBytes);
            return decodedString;

        }
        /// <summary>
        /// 验证数是否在一个范围
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static bool ToValidRange(this object str, double start, double end)
        {
            var v = str.ToDouble(0);
            if (v >= start && v <= end)
                return true;
            return false;

        }

        /// <summary>
        /// 客编
        /// </summary>
        /// <param name="id"></param>
        /// <param name="datafrom"></param>
        /// <returns></returns>
        public static string ToMemberNo(this int id, string datafrom = "")
        {
            return string.Format("J{0}A", id.ToString().PadLeft(5, '0'));
        }

        /// <summary>
        /// 将数字转化为某数字的倍数  如11整成5的倍数  即15
        /// </summary>
        /// <param name="num"></param>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static int NumToBs(this int num, int bs)
        {
            return ((num % bs > 0 ? 1 : 0) + num / bs) * bs;
        }


        /// <summary>
        /// 参数不在指定值时返回错误信息
        /// </summary>
        /// <param name="count"></param>
        /// <param name="defmsg"></param>
        /// <returns></returns>
        public static string ToParmErrorMsg(this int count, string defmsg)
        {
            if (count == 0)
            {
                return defmsg;
            }
            return "";
        }

        /// <summary>
        /// 过滤掉特殊字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilterSpecialChar(this string str)
        {
            if (str == null) return "";

            string[] aryReg = { "'", "<", ">", "%", "\"\"", ",", ">=", "=<", "-", "_", ";", "||", "[", "]", "&", "/", "-", "|", " ", };
            for (int i = 0; i < aryReg.Length; i++)
            {
                str = str.ReplaceAll(aryReg[i], "");
            }
            return str;
        }
        /// <summary>
        /// 是否可发短信
        /// </summary>
        /// <param name="checkStr"></param>
        /// <returns></returns>
        public static bool IsCanSendMobile(this string checkStr)
        {
            if (string.IsNullOrEmpty(checkStr)) { return false; }
            return Regex.IsMatch(checkStr, @"^1\d{10}(\/1\d{10}){0,2}$");
        }

        /// <summary>
        /// 设置为*
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public static string SetXin(this string str, int cnt, int startcount = 2)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {

                var len = str.Length;
                var resut = str;
                resut = str.Substring(0, len > startcount ? startcount : len);

                for (var i = 0; i < cnt; i++)
                {
                    resut += "*";

                }
                if (len - cnt - startcount > 0)
                    resut += str.Substring(len - 2, 1);

                return resut;
            }
            return "";
        }

        /// <summary>
        /// 保留前面的字符后面跟上cnt个*
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public static string SetXin3(this string str, int cnt, int startcount = 2)
        {
            var ishideemail = ConfigUtils.GetValue<int>("IsCloseXin", 0);//全局关闭*字符
            if (ishideemail == 0)
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    var len = str.Length;
                    var resut = str;
                    resut = str.Substring(0, len > startcount ? startcount : len);
                    for (var i = 0; i < cnt; i++)
                    {
                        resut += "*";
                    }
                    if (len > 2)
                    {
                        resut += str.Substring(len - 1, 1);//取最后一个字
                    }
                    return resut;
                }
                return "";
            }
            return str;
        }
        /// <summary>邮箱特殊字符过滤
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public static string ToEmailFilt(this string str, bool isshow = false)
        {
            if (isshow)
            {
                return str;
            }

            var startcount = 3;
            var cnt = 2;
            var resut = str;
            var arr = new string[] { "@qq.com", "@163.com", "@8989.com", "@9009.com", "@90.com" };
            var t = 0;
            if (!string.IsNullOrWhiteSpace(str))
            {
                arr.ToList().ForEach(x =>
                {
                    if (str.ToLower().Contains(x))
                        t++;

                });
            }
            if (t > 0) return str;
            if (!string.IsNullOrWhiteSpace(str))
            {
                var len = str.Length;

                resut = str.Substring(0, len > startcount ? startcount : len);
                for (var i = 0; i < cnt; i++)
                {
                    resut += "*";
                }
                if (len > 2)
                {
                    resut += str.Substring(len - 1, 1);//取最后一个字
                }
                return resut;
            }
            return str;

        }
        /// <summary>
        /// 保留前面的字符后面跟上cnt个*
        /// </summary>
        /// <param name="str">截取字符串</param>
        /// <param name="cnt">**数</param>
        /// <param name="startcount">开头的个数</param>
        /// <param name="endcount">结尾的个数</param>
        /// <returns></returns>
        public static string SetXin4(this string str, int cnt, int startcount = 1, int endcount = 1)
        {
            var len = str.Length;
            var resut = str;
            resut = str.Substring(0, len > startcount ? startcount : len);

            for (var i = 0; i < cnt; i++)
            {
                resut += "*";

            }
            if (len >= endcount)
            {
                resut += str.Substring(len - endcount, endcount);
            }

            return resut;
        }
        /// <summary>
        /// 显示n个* 剩余的显示
        /// </summary>
        /// <param name="str">截取字符串</param>
        /// <param name="cnt">**数</param>
        /// <param name="startcount">开头的个数</param>
        /// <returns></returns>
        public static string SetXin5(this string str, int cnt, int startcount = 1)
        {
            if (string.IsNullOrWhiteSpace(str)) return "";
            var len = str.Length;
            var resut = str;
            resut = str.Substring(0, len > startcount ? startcount : len);

            for (var i = 0; i < cnt; i++)
            {
                resut += "*";

            }
            if (len >= cnt + startcount)
            {
                resut += str.Substring(cnt + startcount);
            }

            return resut;
        }

        /// <summary>
        /// 根据状态获取进度百分比
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string HandleOrderProgress(this object status)
        {
            var status2 = int.Parse(status.ToString()) / 10 + "";
            var result = " '";
            switch (status2)
            {
                case "1":
                    result = "10%"; break;
                case "2":
                    result = "20%"; break;
                case "3":
                    result = "30%"; break;
                case "4":
                    result = "40%"; break;
                case "5":
                    result = "50%"; break;
                case "6":
                    result = "60%"; break;
                case "7":
                    result = "70%"; break;
                case "8":
                    result = "80%"; break;
                case "9":
                    result = "90%"; break;
                default:
                    result = "100%"; break;
            }
            return result;
        }



        /**
         * 生成签名 
         */
        public static string GenSign(Dictionary<string, string> param, string apisec)
        {
            Dictionary<string, string> dictSort = param.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            string str = apisec;
            string separator = "";
            foreach (KeyValuePair<string, string> kv in dictSort)
            {
                if (kv.Key == "signature")
                {
                    continue;
                }
                str += separator + kv.Key + "=" + kv.Value;
                separator = "&";
            }
            str += apisec;
            byte[] result = Encoding.UTF8.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(result)).Replace("-", "");
        }

        /// <summary>
        /// 安全Sql字符串
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string _ToSqlStr(this string Text)
        {
            return Text.Trim().Replace("'", "''");
        }

        /// <summary>
        /// 安全Sql字段名
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string _ToSqlField(this string Text)
        {
            return Regex.Replace(Text, @"[^\w_\.]", "");
        }

        /// <summary>
        /// 金额转换成中文大写金额
        /// </summary>
        /// <param name="LowerMoney">eg:10.74</param>
        /// <returns></returns>
        public static string ToMoneyUpper(this string LowerMoney)
        {
            string functionReturnValue = null;
            bool IsNegative = false; // 是否是负数
            if (LowerMoney.Trim().Substring(0, 1) == "-")
            {
                // 是负数则先转为正数
                LowerMoney = LowerMoney.Trim().Remove(0, 1);
                IsNegative = true;
            }
            string strLower = null;
            string strUpart = null;
            string strUpper = null;
            int iTemp = 0;
            // 保留两位小数 123.489→123.49　　123.4→123.4
            LowerMoney = Math.Round(double.Parse(LowerMoney), 2).ToString();
            if (LowerMoney.IndexOf(".") > 0)
            {
                if (LowerMoney.IndexOf(".") == LowerMoney.Length - 2)
                {
                    LowerMoney = LowerMoney + "0";
                }
            }
            else
            {
                LowerMoney = LowerMoney + ".00";
            }
            strLower = LowerMoney;
            iTemp = 1;
            strUpper = "";
            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "圆";
                        break;
                    case "0":
                        strUpart = "零";
                        break;
                    case "1":
                        strUpart = "壹";
                        break;
                    case "2":
                        strUpart = "贰";
                        break;
                    case "3":
                        strUpart = "叁";
                        break;
                    case "4":
                        strUpart = "肆";
                        break;
                    case "5":
                        strUpart = "伍";
                        break;
                    case "6":
                        strUpart = "陆";
                        break;
                    case "7":
                        strUpart = "柒";
                        break;
                    case "8":
                        strUpart = "捌";
                        break;
                    case "9":
                        strUpart = "玖";
                        break;
                }

                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                        break;
                    case 2:
                        strUpart = strUpart + "角";
                        break;
                    case 3:
                        strUpart = strUpart + "";
                        break;
                    case 4:
                        strUpart = strUpart + "";
                        break;
                    case 5:
                        strUpart = strUpart + "拾";
                        break;
                    case 6:
                        strUpart = strUpart + "佰";
                        break;
                    case 7:
                        strUpart = strUpart + "仟";
                        break;
                    case 8:
                        strUpart = strUpart + "万";
                        break;
                    case 9:
                        strUpart = strUpart + "拾";
                        break;
                    case 10:
                        strUpart = strUpart + "佰";
                        break;
                    case 11:
                        strUpart = strUpart + "仟";
                        break;
                    case 12:
                        strUpart = strUpart + "亿";
                        break;
                    case 13:
                        strUpart = strUpart + "拾";
                        break;
                    case 14:
                        strUpart = strUpart + "佰";
                        break;
                    case 15:
                        strUpart = strUpart + "仟";
                        break;
                    case 16:
                        strUpart = strUpart + "万";
                        break;
                    default:
                        strUpart = strUpart + "";
                        break;
                }

                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }

            strUpper = strUpper.Replace("零拾", "零");
            strUpper = strUpper.Replace("零佰", "零");
            strUpper = strUpper.Replace("零仟", "零");
            strUpper = strUpper.Replace("零零零", "零");
            strUpper = strUpper.Replace("零零", "零");
            strUpper = strUpper.Replace("零角零分", "整");
            strUpper = strUpper.Replace("零分", "整");
            strUpper = strUpper.Replace("零角", "零");
            strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("零亿零万", "亿");
            strUpper = strUpper.Replace("零万零圆", "万圆");
            strUpper = strUpper.Replace("零亿", "亿");
            strUpper = strUpper.Replace("零万", "万");
            strUpper = strUpper.Replace("零圆", "圆");
            strUpper = strUpper.Replace("零零", "零");

            // 对壹圆以下的金额的处理
            if (strUpper.Substring(0, 1) == "圆")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "零")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "角")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "分")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "整")
            {
                strUpper = "零圆整";
            }
            functionReturnValue = strUpper;

            if (IsNegative == true)
            {
                return "负" + functionReturnValue;
            }
            else
            {
                return functionReturnValue;
            }
        }


        ///// <summary>
        /////  转换为string字符串类型
        ///// </summary>
        ///// <param name="s">获取需要转换的值</param>
        ///// <param name="format">需要格式化的位数</param>
        ///// <returns>返回一个新的字符串</returns>
        //public static string ToStr(this object s, string format = "")
        //{
        //    string result = "";
        //    try
        //    {
        //        if (s == null) return "";
        //        if (string.IsNullOrWhiteSpace(format))
        //        {
        //            result = s.ToString();
        //        }
        //        else
        //        {
        //            result = string.Format("{0:" + format + "}", s);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    return result;
        //}
        /// <summary>判断俩个字符串是否存在交集
        /// 
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="compareStr">对比的字符串</param>
        /// <param name="split2">分割符</param>
        /// <returns></returns>
        public static bool IsIntersection(this string str, string compareStr, char split2)
        {
            if (IsNull(str)) return false;
            var arr = compareStr.Split(split2).ToList();
            var i = 0;
            arr.ForEach(x =>
            {
                if (str.Contains(x))
                {
                    i++;
                }
            });
            return i > 0;


        }

   

        /// <summary>
        /// 获取paypal手续费--正推
        /// </summary>
        /// <param name="TotalMoney"></param>
        /// <returns></returns>
        public static decimal ToPayPalFee(this decimal TotalMoney)
        {
            decimal Fee = 0;
            if (TotalMoney == 0)
            {
                return Fee;
            }
            var fl = ConfigUtils.GetValue<decimal>("PayFeeLv");//0.029
            var tt = (TotalMoney + 0.3m) / (1 - fl) - TotalMoney;
            return Math.Round(tt, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 计算 Paypal 手续费  --反推
        /// </summary>
        /// <param name="TotalMoney"></param>
        /// <returns></returns>
        public static decimal ToPayPalChare(this decimal TotalMoney)
        {
            decimal Fee = 0;
            if (TotalMoney == 0)
            {
                return Fee;
            }
            var fl = ConfigUtils.GetValue<decimal>("PayFeeLv");
            var tt = (TotalMoney * fl) + 0.3m;
            return Math.Round(tt, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 通过订单编号获取产品名称
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="decription"></param>
        /// <returns></returns>
        public static string ToProduceNameByOrderNo(this string orderNo, string decription = "")
        {
            var orderNoChar = (orderNo ?? " ").Substring(0, 1).ToLower();
            string result = "";
            switch (orderNoChar)
            {
                case "i":
                case "j":
                case "b": result = "Electronic Components"; break;
                case "h": result = "PCB Products"; break;
                case "u": result = "Universal Boards"; break;
                case "s":
                    if (decription.ToStr().Length > 0)
                    {
                        if (decription.Contains("CustomSize"))
                        {
                            result = "Non-frameWork";
                        }
                        else
                        {
                            result = "Stencil with frame";
                        }
                    }
                    else
                    {
                        result = "";
                    }
                    break;
                case "a": result = "PCB Assembly( Stencil included)"; break;
                case "k": result = "Membrane Switch"; break;
                case "m": result = "Graphic Overlay"; break;
                default: result = "Code"; break;
            }
            return result;
        }
        /// <summary>
        /// 静态网址转换成键值列表
        /// </summary>
        /// <param name="Url">网址,例如?a=0&b=3&c=9, /doc/detail?id=999, http://www.aa.com/doc/detail?id=2</param>
        /// <returns></returns>
        public static NameValueCollection _GetParams(this Uri Url)
        {
            return Url.Query._GetParams();
        }
        /// <summary>
        /// 静态网址转换成键值列表
        /// </summary>
        /// <param name="Url">网址,例如?a=0&b=3&c=9, /doc/detail?id=999, http://www.aa.com/doc/detail?id=2</param>
        /// <returns></returns>
        public static NameValueCollection _GetParams(this string Url)
        {
            var nvs = new NameValueCollection();
            var prmlst = Url.TrimStart('?').Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var i in prmlst)
            {
                var at = i.IndexOf('=');
                if (at == -1)
                {
                    nvs.Add(i, string.Empty);
                }
                else
                {
                    nvs.Add(i.Substring(0, at), i.Substring(at + 1));
                }
            }
            return nvs;
        }

        /// <summary>
        /// 仓位简写
        /// </summary>
        /// <param name="putarea"></param>
        /// <returns></returns>
        public static string ToPutAreaBrif(this string putarea)
        {
            var str = "";
            if (!string.IsNullOrWhiteSpace(putarea))
            {
                var lstStr = new List<string>();
                putarea.Split(',').ToList().ForEach(y =>
                {
                    if (y == ("A") || y == "A号间")
                    {
                        lstStr.Add("A");
                    }
                    else
                    {
                        var lst = y.Substring(1).Split('/').ToList();
                        try
                        {
                            var j = 0; var ss = "";
                            lst.ForEach(x =>
                            {
                                if (j == 3)
                                {
                                    ss += "/" + x;
                                }
                                else
                                {
                                    var fs = x.Substring(0, 1);
                                    if (x == "退货区") fs = "退";
                                    else if (x == "退供区") fs = "供";
                                    ss += "/" + fs;
                                }

                                j++;
                            });

                            lstStr.Add(ss);

                        }
                        catch { }

                    }
                });
                str = string.Join(",", lstStr);

            }
            else str = putarea;
            return str;

        }

    }
}