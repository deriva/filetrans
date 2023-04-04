using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bc.PublishWF.Common
{
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
                LogHelper.Error("分解对象值出错" + ex.ToString());
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
}
