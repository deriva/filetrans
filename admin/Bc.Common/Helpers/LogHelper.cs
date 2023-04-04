using Bc.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Bc.Common.Helpers
{
    public class LogHelper
    {
        private string _FilePath = @"D:\Program Files\Zm.TaskWindowsService\Log";
        public static object LogLock = new object();

        public LogHelper()
        {
            CreateDirectory();
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            (new LogHelper()).WtireLog("Error", message);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message, Exception ex)
        {

            (new LogHelper()).WtireLog("Error", message + ":(行：" + ex.StackTrace + ")" + ex.ToString());
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        public static void Error(Exception ex)
        {
            (new LogHelper()).WtireLog("Error", ex.ToString());
        }
        //          /// <summary>
        ///// 错误日志
        ///// </summary>
        ///// <param name="message"></param>
        //public static void BlockInfo(string message, string message2)
        //{
        //    (new LogHelper()).WtireLog("BlockInfo", message + ":" + message2);
        //}


        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            (new LogHelper()).WtireLog("Warn", message);

        }

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            (new LogHelper()).WtireLog("Info", message);

        }

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string type, string message)
        {
            (new LogHelper()).WtireLog(type, message);

        }
        public static void Write(string type, string message, string filetype = "html")
        {
            (new LogHelper()).WtireFile(type, message, filetype = "html");

        }
        /// <summary>
        /// create directory if filePathSection has string
        /// </summary>
        private void CreateDirectory()
        {
            try
            {
                string root = GetLogFileRootPath();

                if (!System.IO.Directory.Exists(root))
                {
                    System.IO.Directory.CreateDirectory(root);
                }

                this.RemoveDirectoryInfoReadOnly(root);

                _FilePath = root;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        private void WtireLog(string type, string message, string title = "")
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            _FilePath += "\\Log" + type;
            if (!Directory.Exists(_FilePath))
            {
                //   LogHelper.Info(_FilePath);
                Directory.CreateDirectory(_FilePath);
            }
            string msg = "<hr color='red'/>Time:" + System.DateTime.Now.ToString("HH:mm:ss fff") + "     Grade：" + type + "<br/><hr size='1'/> " + message;

            try
            {
                lock (LogHelper.LogLock)
                {
                    string path = string.Empty;

                    path = _FilePath + "\\" + System.DateTime.Now.ToString("yyyyMMdd") + ".html";

                    FileInfo finfo = new FileInfo(path);
                    if (!finfo.Exists)
                    {
                        StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8"));
                        sw.Write("");
                        sw.Close();

                    }
                    using (StreamWriter fs = finfo.AppendText())
                    {
                        StreamWriter w = fs;//new StreamWriter(fs);
                        w.BaseStream.Seek(0, SeekOrigin.End);
                        w.WriteLine(msg);
                        w.Flush();
                        w.Close();

                    }
                    var splitPath = _FilePath + "\\" + System.DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_{0}.html";
                    var m1 = 1024 * 1024;
                    SplitFile(path, m1, m1, splitPath);


                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

        }


        private void WtireFile(string type, string message, string filetype = "html")
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            _FilePath += "\\Log" + type;
            if (!Directory.Exists(_FilePath))
            {
                //   LogHelper.Info(_FilePath);
                Directory.CreateDirectory(_FilePath);
            }
            string msg = message;

            try
            {
                lock (LogHelper.LogLock)
                {
                    string path = string.Empty;

                    path = _FilePath + "\\" + System.DateTime.Now.ToString("yyyyMMdd") + "." + filetype;

                    FileInfo finfo = new FileInfo(path);
                    if (!finfo.Exists)
                    {
                        StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8"));
                        sw.Write("");
                        sw.Close();
                    }
                    using (StreamWriter fs = finfo.AppendText())
                    {
                        StreamWriter w = fs;//new StreamWriter(fs);
                        w.BaseStream.Seek(0, SeekOrigin.End);
                        w.WriteLine(msg);
                        w.Flush();
                        w.Close();
                    }
                    var splitPath = _FilePath + "\\" + System.DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_{0}." + filetype;
                    var m1 = 1024 * 1024;
                    SplitFile(path, m1, m1, splitPath);


                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

        }

        /// <summary>
        /// log file root path
        /// </summary>
        private static string GetLogFileRootPath()
        {
            string logPath = "";// (string)System.Configuration.ConfigurationManager.AppSettings["TextLog.Path"];
            try
            {
                if (logPath.Length >= 2)
                {
                    if (logPath.Substring(0, 2) == @"\\")
                    {
                        return logPath;
                    }
                    else if (logPath.Substring(1, 1) == ":")
                    {
                        return logPath;
                    }
                    else
                    {
                        return System.AppDomain.CurrentDomain.BaseDirectory + "Log\\" + logPath;
                    }
                }
                else
                {
                    return System.AppDomain.CurrentDomain.BaseDirectory + "Log\\" + logPath;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                return System.AppDomain.CurrentDomain.BaseDirectory;
            }

        }

        /// <summary>
        /// remove directory readonly property
        /// </summary>
        /// <param name="filepath"></param>
        private void RemoveDirectoryInfoReadOnly(string filepath)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    System.IO.DirectoryInfo finfo = new DirectoryInfo(filepath);
                    if (finfo.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        finfo.Attributes = FileAttributes.Normal;
                }
            }
            catch { }
        }

        public static BlockInfo BlockInfo(string startMessage, string endMessage, bool logTimeCost = false)
        {
            return new BlockInfo(startMessage, endMessage, logTimeCost);
        }

        /// <summary>
        /// 生成修改参数信息日志
        /// </summary>
        /// <param name="SrcModel">原模型</param>
        /// <param name="ChgModel">新模型</param>
        /// <param name="SelectNames">选择器</param>
        /// <param name="Names">参数名列表,不填或Null则使用属性名</param>
        /// <param name="Sep">间隔符</param>
        /// <returns></returns>
        public static string CreateChageParamsLog<TIn, TOut>(TIn SrcModel, TIn ChgModel, Func<TIn, TOut> SelectNames, IList<string> Names = null, string Sep = ",")
        {
            var msg = new StringBuilder();
            var tp = typeof(TOut);
            var mmbs = tp.GetProperties();
            var minlen = mmbs.Length;
            var srcmod = SelectNames(SrcModel);
            var chgmod = SelectNames(ChgModel);
            var count = Names == null ? 0 : Names.Count;
            for (int i = 0; i < minlen; i++)
            {
                var mbmothed = (System.Reflection.PropertyInfo)mmbs[i];
                var src = mbmothed.GetValue(srcmod, null);
                var chg = mbmothed.GetValue(chgmod, null);
                if (src == null)
                {
                    if (chg != null)
                    {
                        msg.Append(Sep + mbmothed.Name + "::" + src + "=>" + chg);
                    }
                    continue;
                }
                else if (chg == null)
                {
                    msg.Append(Sep + mbmothed.Name + "::" + src + "=>" + chg);
                    continue;
                }
                if (!src.Equals(chg))
                {
                    msg.Append(Sep + (count > i ? Names[i] : mbmothed.Name) + "::" + src + "=>" + chg);
                }
            }
            int len = msg.Length, slen = Sep.Length;
            return len == 0 ? string.Empty : msg.ToString(slen, len - slen);
        }

        /// <summary>
        /// 生成修改参数信息日志
        /// </summary>
        /// <param name="Names">参数名列表</param>
        /// <param name="SrcValues">参数原值列表</param>
        /// <param name="ChgValues">参数新值列表</param>
        /// <param name="Sep">间隔符</param>
        /// <returns></returns>
        public static string CreateChageParamsLog(IList<string> Names, IList<string> SrcValues, IList<string> ChgValues, string Sep = ", ")
        {
            var msg = new StringBuilder();
            var minlen = new int[] { Names.Count, SrcValues.Count, ChgValues.Count }.Min();
            for (int i = 0; i < minlen; i++)
            {
                var src = SrcValues[i];
                var chg = ChgValues[i];
                if (src != chg)
                {
                    msg.Append(Sep + Names[i] + "::" + src + "=>" + chg);
                }
            }
            return msg.ToString(Sep.Length, msg.Length - Sep.Length);
        }
        /// <summary>
        /// 大文件分割
        /// </summary>
        /// <param name="filepath">待分割文件路径:如：D:\x_20160407.txt</param>
        /// <param name="splitMinFileSize">当文件大于这个配置项时就执行文件分隔:单位M</param>
        /// <param name="splitFileSize">每个分隔出来的文件大小:单位M</param>
        /// <param name="splitFileFormat">新分割出的文件格式：如D:\x_20160407{0}.txt</param>
        /// <returns></returns>
        public static int SplitFile(string filepath, int splitMinFileSize, int splitFileSize, string splitFileFormat)
        {
            string file = filepath;

            FileInfo fileInfo = new FileInfo(file);
            if (fileInfo.Length > splitMinFileSize)
            {
                //  Console.WriteLine("判定结果：需要分隔文件！");
            }
            else
            {
                //Console.WriteLine("判定结果：不需要分隔文件！");
                //Console.ReadKey();
                return 0;
            }

            int steps = Math.Ceiling(fileInfo.Length / (splitFileSize * 1.0)).ToInt2();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    int couter = 1;
                    bool isReadingComplete = false;
                    while (!isReadingComplete)
                    {
                        string filePath = string.Format(splitFileFormat, couter);
                        //   Console.WriteLine("开始读取文件【{1}】：{0}", filePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                        byte[] input = br.ReadBytes(splitFileSize);
                        //能否读取到指定长度的字节
                        if (input.Length >= splitFileSize)
                        {
                            using (FileStream writeFs = new FileStream(filePath, FileMode.Create))
                            {
                                using (BinaryWriter bw = new BinaryWriter(writeFs))
                                {
                                    bw.Write(input);
                                }
                            }
                        }
                        else
                        {
                            isReadingComplete = (input.Length < splitFileSize);
                        }

                        if (!isReadingComplete)
                        {
                            couter += 1;
                        }
                        //   Console.WriteLine("完成读取文件【{1}】：{0}", filePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                }
            }
            if (fileInfo.Length >= splitFileSize)
                fileInfo.Delete();

            return 0;


        }
    }
    public struct BlockInfo : IDisposable
    {
        private string endMessage;
        private Stopwatch stopwatch;

        internal BlockInfo(string startMessage, string endMessage, bool logTimeCost)
        {
            this.endMessage = endMessage;

            if (!string.IsNullOrWhiteSpace(startMessage))
            {
                LogHelper.Info(startMessage);
            }

            if (logTimeCost)
            {
                stopwatch = Stopwatch.StartNew();
                stopwatch.Start();
            }
            else
            {
                stopwatch = null;
            }
        }

        public void Dispose()
        {
            string message = null;

            if (stopwatch != null)
            {
                stopwatch.Stop();
                message = string.IsNullOrWhiteSpace(endMessage) ? ("[消耗时间：" + stopwatch.Elapsed + "]") : (endMessage + "[消耗时间：" + stopwatch.Elapsed + "]");
            }
            else
            {
                message = endMessage;
            }

            if (!string.IsNullOrWhiteSpace(message))
            {
                LogHelper.Info(message);
            }
        }
    }
}
