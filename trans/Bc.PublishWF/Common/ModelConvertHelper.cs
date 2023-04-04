using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;

namespace Bc.PublishWF.Common
{

    /// <summary>
    /// 对象转换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelConvertHelper<T> where T : new()  // 此处一定要加上new()
    {

        public static IList<T> ConvertToModel(DataTable dt)
        {

            IList<T> ts = new List<T>();// 定义集合
            Type type = typeof(T); // 获得此模型的类型
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {

                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            try
                            {
                                if (pi.PropertyType.Name == "Boolean")
                                {
                                    value = value.ToBoolen2();
                                }

                                pi.SetValue(t, value, null);
                            }
                            catch { }
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }



        /// <summary>
        /// 不需要分页
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="flag">false</param>
        /// <returns></returns>
        public static string Serialize(DataTable dt)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(result);
            }
            return serializer.Serialize(list); ;
        }


        public static string ToFormParm(Object o)
        {
            var dic = ToMap(o);
            var parm = new List<string>();
            foreach (var it in dic)
            {
                parm.Add(string.Format("{0}={1}", it.Key, (it.Value.ToStr())));

            }
            return string.Join("&", parm);
        }


        /// <summary>
        ///
        /// 将对象属性转换为key-value对
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Dictionary<String, Object> ToMap(Object o)
        {
            Dictionary<String, Object> map = new Dictionary<string, object>();

            Type t = o.GetType();

            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in pi)
            {
                MethodInfo mi = p.GetGetMethod();

                if (mi != null && mi.IsPublic)
                {
                    map.Add(p.Name, mi.Invoke(o, new Object[] { }));
                }
            }

            return map;

        }


        /// <summary>
        /// 获取两个model的参数变化并替换值
        /// </summary>
        /// <param name="olddic"></param>
        /// <param name="newdic"></param>
        /// <returns></returns>
        public static string GetModelParmChange(Object olddic, Object newdic, List<string> exclude = null)
        {
            var dicoldmodel = ToMap(olddic);
            var dicnewmodel = ToMap(newdic);
            var listTy = new List<string>();
            var desc = new StringBuilder();
            foreach (var item in dicnewmodel)
            {
                if (exclude != null && exclude.Contains(item.Key)) { continue; }
                var newval = item.Value;
                object old;
                dicoldmodel.TryGetValue(item.Key, out old);
                var pro = olddic.GetType().GetProperties().Where(x => x.Name == item.Key).FirstOrDefault();
                if (pro == null)
                    continue;
                var type = pro.PropertyType.FullName;
                if (!listTy.Contains(type))
                    listTy.Add(type);
                //  DateTime dateTimeValue1 = "1981-08-24".ConvertTo<DateTime>(); 
                //item.Value.ConvertTo<type>(); 
                //Convert.to
                if (item.Value != null)
                {
                    var name = ModelProperty.GetPropertyDesc(typeof(T), item.Key);
                    desc.Append(string.Format("{0}:{1}=>{2}", name, old, newval));
                    LogHelper.Info(desc.ToString());
                    pro.SetValue(olddic, item.Value);
                }
                //if (newval != null && newval.ToString() != "")
                //{

                //    pro.SetValue(olddic, item.Value);
                //}
            }
            //LogHelper.Info(string.Join(",", listTy));
            return desc.ToString();

        }
        private static bool CompareVal(string type, object source, object desc)
        {
            var flag = false;
            if (desc == null && source == null) return true;
            if (desc != null)
            {
                if (source == null) return true;
                // System.Int32,System.String,System.DateTime,System.Double,System.Boolean,System.Byte,System.Nullable`1[[System.DateTime, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]],System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]],System.Decimal,System.Nullable`1[[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]],System.Nullable`1[[System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]],System.Nullable`1[[System.Decimal, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
                if (type == "System.Int32")
                    flag = source.ToInt2() == desc.ToInt2();
                else if (type == "System.String")
                    flag = source.ToStr() == desc.ToStr();
                else if (type == "System.DateTime")
                    flag = source.ToDateTime() == desc.ToDateTime();
                else if (type == "System.Boolean")
                    flag = !source.ToBoolen2() && source.ToBoolen2() == desc.ToBoolen2();
                else if (type == "System.Decimal")
                    flag = source.ToDecimal2() == desc.ToDecimal2();
                else if (type == "System.Byte")
                    flag = (byte)source == (byte)desc;
                else if (type == "System.Double")
                    flag = (Double)source == (Double)desc;
            }

            return flag;
        }


        /// <summary>
        /// 生产插入语句
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="tablename"></param>
        /// <param name="olddic"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        public static string ToInsertSql(Dictionary<string, string> columns, string tablename, Object olddic, List<string> exclude = null)
        {
            var dicoldmodel = ToMap(olddic);
            var lstcolumn = new List<string>();
            var lstvalue = new List<string>();
            var chartype = "varchar,datetime";
            foreach (var item in columns)
            {
                if (exclude != null && exclude.Contains(item.Key)) { continue; }
                lstcolumn.Add(item.Key);
                object value = "";
               
                dicoldmodel.TryGetValue(item.Key, out value);
                if (chartype.Contains(item.Value.ToLower()))
                {
                    if ("datetime".Contains(item.Value)) value = value.ToDateTime().ToStr("yyyy-MM-dd HH:mm:ss");
                    if (value == null) value = "";

                    lstvalue.Add(string.Format("N'{0}'", FilteSQLStr(value.ToStr())));
                }
                else
                {
                    if (item.Value == "bit" && value != null) value = value.ToBoolen2() ? 1 : 0;
                    if (value == null) value = value.ToInt2();

                    lstvalue.Add(string.Format("{0}", value));
                }

            }
            var sql = string.Format("insert into {0}({1})values({2})", tablename, string.Join(",", lstcolumn), string.Join(",", lstvalue));
            return sql;

        }

        /// <summary>
        /// 过滤不安全的字符串
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        private static string FilteSQLStr(string Str)
        {

            Str = Str.Replace("'", "");
            //Str = Str.Replace("\"", "");
            //Str = Str.Replace("&", "&amp");
            //Str = Str.Replace("<", "&lt");
            //Str = Str.Replace(">", "&gt");

            Str = Str.Replace("delete", "");
            Str = Str.Replace("update", "");
            Str = Str.Replace("insert", "");

            return Str;
        }
    }

    // 对象克隆（C# 快速高效率复制对象另一种方式 表达式树转）
    public static class TransExpV2Helper<TIn, TOut>
    {

        private static readonly Func<TIn, TOut> cache = GetFunc();
        private static Func<TIn, TOut> GetFunc()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();

            foreach (var item in typeof(TOut).GetProperties())
            {
                try
                {
                    if (!item.CanWrite)
                        continue;
                    var t = typeof(TIn).GetProperty(item.Name);
                    if (t != null)
                    {
                        MemberExpression property = Expression.Property(parameterExpression, t);
                        MemberBinding memberBinding = Expression.Bind(item, property);
                        memberBindingList.Add(memberBinding);
                    }
                }
                catch { }
            }

            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });

            return lambda.Compile();
        }

        public static TOut Trans(TIn tIn)
        {
            return cache(tIn);
        }

    }
}
