using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace Bc.PublishWF.Common
{
    public class ConfigUtils
    {
        /// <summary>
        /// 根据节点名获取节点值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyName">节点名</param>
        /// <param name="defaultValue">默认值(可以不填)</param>
        /// <returns></returns>
        public static T GetValue<T>(string keyName, T defaultValue = default(T))
        {
            try
            {
                object value = System.Configuration.ConfigurationManager.AppSettings[keyName];
                if (value != null)
                    return value.ChangeValue<T>(defaultValue);
            }
            //catch (FormatException ex)
            catch
            {
                //异常写入日志中  
                //LogUtils.ExceptionLog("读取Config文件中的" + keyName + "节点时发生异常.", ex);
            }

            return defaultValue;
        }
     
    }
}
