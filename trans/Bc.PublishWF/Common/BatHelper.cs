using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.PublishWF.Common
{
    public class BatHelper
    {
        public static string Exec(string cmd)
        {
            Process process = new Process(); // 创建一个Process类对象
            process.StartInfo.FileName = "cmd.exe"; // 设置进程的可执行文件
            process.StartInfo.UseShellExecute = false; // 是否使用操作系统shell启动进程
            process.StartInfo.CreateNoWindow = true; // 是否在新窗口中启动进程
            process.StartInfo.RedirectStandardInput = true; // 是否重定向输入
            process.StartInfo.RedirectStandardOutput = true; // 是否重定向输出
            process.StartInfo.RedirectStandardError = true; // 是否重定向错误输出
            process.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8; // 输出编码方式
            process.Start();  // 启动进程
            string result = "";
            var lst = cmd.Split('\n');
            foreach (var line in lst)
            {
                if (!line.IsEmpty())
                {
                    // result +=$"{line}<br/>";
                    process.StandardInput.WriteLine(line); // 向CMD进程写入命令

                }
            }
            process.StandardInput.Close(); // 关闭标准输入流
            result += process.StandardOutput.ReadToEnd(); // 读取CMD输出的结果
            result += "<br/>";

            process.WaitForExit(); // 等待CMD进程退出
            process.Close(); // 关闭进程 
            return result;
        }


        /// <summary>
        /// 启动站点
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public static string StartSite(string siteName)
        {
            var r = "";
            var cmd = @"C:\Windows\System32\inetsrv\appcmd.exe start apppool /apppool.name:" + siteName;
            r += "程序池:" + Exec(cmd);
            cmd = @"c:\Windows\System32\inetsrv\appcmd.exe start site " + siteName;
            r += "站点:" + Exec(cmd);
            return r;
        }

        /// <summary>
        /// 停止站点
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public static string StopSite(string siteName)
        {
            var r = "";
            var cmd = @"C:\Windows\System32\inetsrv\appcmd.exe stop  apppool /apppool.name:" + siteName;
            r += "程序池:" + Exec(cmd);
            cmd = @"c:\Windows\System32\inetsrv\appcmd.exe stop  site " + siteName;
            r += "站点:" + Exec(cmd);
            return r;
        }
    }
}
