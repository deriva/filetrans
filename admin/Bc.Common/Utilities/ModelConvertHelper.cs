using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Common.Utilities
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

            string tempName;
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
                                if (pi.PropertyType.Name == "Int32")
                                {
                                    value = value.ToInt2();
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
        /// 将list转换为datatable
        /// </summary>
        public static DataTable ToDataTable(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }
        /// <summary>
        /// 不需要分页
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="flag">false</param>
        /// <returns></returns>
        public static string Serialize(DataTable dt)
        {
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
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
            return Newtonsoft.Json.JsonConvert.SerializeObject(list); ;
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
                   // LogHelper.Info(desc.ToString());
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
    public class ModelProperty
    {
        public static string GetPropertyDesc(Type objectType, string propertyName)
        {
            try
            {
                var first = objectType.GetProperty(propertyName).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault(); //objectType.GetProperty(propertyName).GetCustomAttributes(typeof(DataFieldAttribute),false).FirstOrDefault();  
                return ((DisplayAttribute)first).Name;
            }
            catch (Exception ex)
            {
               // LogHelper.Error("分解对象值出错" + ex.ToString());
            }
            return propertyName;
        }
        public static string GetPropertyName<T>(Expression<Func<T, object>> expr)
        {
            var rtn = "";
            if (expr.Body is UnaryExpression)
            {
                rtn = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
            }
            else if (expr.Body is MemberExpression)
            {
                rtn = ((MemberExpression)expr.Body).Member.Name;
            }
            else if (expr.Body is ParameterExpression)
            {
                rtn = ((ParameterExpression)expr.Body).Type.Name;
            }
            return rtn;
        }
    }

    public static class ORMHelper
    {
        /// <summary>
        /// 设置默认值
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isadd"></param>
        /// <param name="userId"></param>
        public static void AopVal(this object model, bool isadd, long userId)
        {
            PropertyInfo[] propertys = model.GetType().GetProperties();// 获得此模型的公共属性
            string[] columns = new string[] { "createdby", "updatedby", "createdtime", "updatedtime" };
            foreach (PropertyInfo pi in propertys)
            {
                var tempName = pi.Name;
                if (columns.ToList().Where(x => x.ToLower() == tempName.ToLower()).Count() > 0)
                {
                    if (!pi.CanWrite) continue;
                    object value = pi.GetValue(model);//.[tempName];
                    if (value == null)
                    {
                        try
                        {
                            if (isadd)
                            {
                                if (tempName.ToLower() == "createdtime")
                                    pi.SetValue(model, DateTime.Now, null);
                                if (tempName.ToLower() == "createdby")
                                    pi.SetValue(model, userId, null);
                            }
                            if (tempName.ToLower() == "updatedtime")
                                pi.SetValue(model, DateTime.Now, null);
                            if (tempName.ToLower() == "updatedby")
                                pi.SetValue(model, userId, null);

                        }
                        catch (Exception ex) { }
                    }
                }
            }
        }

        /// <summary>
        /// 映射对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T Mapper<T>(this object model) where T : new()
        {
            T t = new T();
            PropertyInfo[] t_P = t.GetType().GetProperties();// 获得此模型的公共属性 
            PropertyInfo[] propertys = model.GetType().GetProperties();
            foreach (PropertyInfo pi in t_P)
            {
                var isDefault = true;
                if (!pi.CanWrite) continue;
                var tempName = pi.Name.ToLower();
                var d = propertys.Where(x => x.Name.ToLower() == tempName.ToLower()).FirstOrDefault();
                if (d != null)
                {
                    object value = d.GetValue(model);//.[tempName];
                    if (value != null)//值不为空
                    {
                        isDefault = false;
                        pi.SetValue(t, value, null);
                    }
                }
                if (isDefault && pi.GetValue(t) != null && pi.GetValue(t) != "")
                {//不能为空的属性值 付默认值

                    if (tempName.Contains("date"))
                    {
                        pi.SetValue(t, DateTime.Parse("1990-01-01"));
                    }
                    if (tempName.Contains("int32")) pi.SetValue(t, 0);
                    if (tempName.Contains("decimal")) pi.SetValue(t, 0);
                    if (tempName.Contains("boolean")) pi.SetValue(t, false);

                }
            }
            return t;

        }
        public static List<T> MapperLst<T>(this object[] lst) where T : new()
        {

            var info = lst.GetType();


            IList objList = (IList)lst;
            var ls = new List<T>();

            lst.ToList().ForEach(x =>
            {
                ls.Add(x.Mapper<T>());
            });
            return ls;

        }

    }
}
