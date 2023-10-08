using Bc.Common.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Common.Helpers
{
    /// <summary>
    /// JSON帮助类
    /// </summary>
    public static class JsonHelper
    {
        //private static readonly JavaScriptSerializer _jss = new JavaScriptSerializer();

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static object ResponseJson(bool success, string message = "", object data = null)
        {
            return new { success = success, message = message, data = data };
        }

        public static object JsonObject(bool success, string message = "", object data = null)
        {
            return new { success = success, message = message, attr = data };
        }

        /// <summary>
        /// 返回JSON，object对象
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="message">消息</param>
        /// <param name="data">附加信息，是一个object对象</param>
        /// <returns></returns>
        public static object ToObject(bool success, string message = "", object data = null)
        {
            return new { success = success, message = message, attr = data };
        }

        /// <summary>
        /// 返回JSON，object对象字符串
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="message">消息</param>
        /// <param name="data">附加信息，是一个object对象</param>
        /// <returns></returns>
        public static string ToObjectStr(bool success, string message = "", object data = null)
        {
            var obj = new { success = success, message = message, attr = data };
            return Serialize(obj);
        }

        /// <summary>
        /// 返回验证错误响应信息
        /// 错误格式：ErrorMessage ， PropertyName
        /// 前后台需格式统一
        /// </summary>
        /// <param name="errors">错误消息</param>
        /// <returns>JSON</returns>
        public static object ValidateErrorResponse(Dictionary<string, string> errors)
        {
            var list = new List<object>();
            foreach (var error in errors)
            {
                list.Add(new { PropertyName = error.Key, ErrorMessage = error.Value });
            }
            return JsonObject(false, "", list);
        }

        /// <summary>
        /// 返回验证错误响应信息
        /// 错误格式：ErrorMessage ， PropertyName
        /// 前后台需格式统一
        /// </summary>
        /// <param name="errorMessage">错误小心</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>JSON</returns>
        public static object ValidateErrorResponse(string propertyName, string errorMessage)
        {
            var list = new List<object> { new { ErrorMessage = errorMessage, PropertyName = propertyName } };
            return JsonObject(false, "", list);
        }
        public static string Serialize(bool success, string message = "", object data = null)
        {
            return JsonConvert.SerializeObject(new { success = success, message = message, attr = data });
        }
        /// <summary>
        /// 反序列化json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string Json)
        {
            return JsonConvert.DeserializeObject<T>(Json);
        }

        #region Json与DataTable互转 add by  2017-11-17
        /// <summary>
        /// DataTable 对象 转换为Json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static ArrayList ToJsonArray(DataTable dt)
        {
            //  JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            //  javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToStr());
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }

            return arrayList;  //返回一个json字符串
        }
        public static T Deserialize<T>(DataTable dt)
        {
            var str = ToJson(dt);
            return JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>
        /// DataTable 对象 转换为Json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(DataTable dt)
        {
            return JsonConvert.SerializeObject(ToJsonArray(dt));  //返回一个json字符串
        }
        private static void DataColumnToLower(DataTable dt)
        {
            foreach (DataColumn item in dt.Columns)
            {
                item.ColumnName = item.ColumnName;
            }
        }

        public static string DataTableToJson(DataTable dt, int records)
        {

            DataColumnToLower(dt);
            if (records <= 0) records = dt.Rows.Count;

            string json = "{\"total\":\"" + records + "\",\"rows\":" + ToJson(dt) + "}";
            return json;
        }

        #region 转换为string字符串类型
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
        #endregion


        #endregion

        public static T Deserialize<T>(string strJson)
        {
            if (strJson == null)
                strJson = "";
            return JsonConvert.DeserializeObject<T>(strJson);
        }
        public static T CloneObj<T>(this object obj)
        {
            if (obj == null)
                return default(T);
            var strJson = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(strJson);
        }
        /// <summary>
        /// 通过JSON序列化，复制实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T CloneJson<T>(T source)
        {
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }

    }
}
