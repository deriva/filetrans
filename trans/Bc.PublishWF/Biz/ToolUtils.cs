using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.PublishWF.Biz
{
    public class ToolUtils
    {
        /// <summary>
        /// 打印消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="iscludedate"></param>
        public static void Print(string msg,int iscludedate=1)
        {
            var tt = string.Empty;
            if (iscludedate == 1) tt = DateTime.Now.ToString("MM-dd HH:mm:ss")+ "=>";
            tt +=   msg + "\r\n";
            Console.WriteLine(tt);
        }

        /// <summary>
        /// 计算耗时描述
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="iscludedate"></param>
        public static double CalcTime(DateTime start)
        {
         return   (DateTime.Now - start).TotalSeconds;
        }
    }
}
