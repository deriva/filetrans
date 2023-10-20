using Bc.PublishWF.Common;
using Bc.PublishWF.Common.Enums;
using Bc.PublishWF.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Bc.PublishWF.Biz.V3
{
    /// <summary>
    /// 服务端
    /// </summary>
    public static class FileServerV3
    {

        private static Socket serverSocket;
        public static void Init()
        {
            //服务器IP地址
            var serverip = ConfigUtils.GetValue<string>("serverip").Split(':');
            var myProt = serverip[1].ToInt2();
            string ipstr = serverip[0];
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, myProt);
            serverSocket = new Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(localEP);
            //绑定IP地址：端口
            serverSocket.Listen(10);    //设定最多10个排队连接请求
            ControlHelper.AddMsg(string.Format("启动监听{0}成功", serverSocket.LocalEndPoint.ToString()));

            HttpSiteHelper.ReportConection();
            //通过Clientsoket发送数据
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
        }
        public static void Exit()
        {
            if (serverSocket != null)
            {
                serverSocket.Close();
                HttpSiteHelper.ReportDisConection();
                serverSocket = null;
            }
        }
        private static void ListenClientConnect()
        {
            while (true)
            {
                if (serverSocket != null)
                {
                    try
                    {
                        //一旦接受连接，创建一个客户端
                        Socket clientSocket = serverSocket.Accept();
                        Thread receiveThread = new Thread(Create);
                        receiveThread.Start(clientSocket);
                    }
                    catch
                    {
                        break;
                    }
                }
            }
        }
        public static void Create(object clientSocket)
        {
            Socket client = clientSocket as Socket;
            //获得客户端节点对象
            IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;

            var flag = true;
            var allowclinetip = ConfigUtils.GetValue<string>("allowclinetip");
            if (!string.IsNullOrWhiteSpace(allowclinetip) && !allowclinetip.Contains(clientep.Address + ""))
            {
                ControlHelper.AddMsg("非法入侵，来自:" + clientep.Address + ":" + clientep.Port);
                flag = false;
            }
            if (flag)
            {
                ControlHelper.AddMsg("收到远程终端:" + clientep.Address + ":" + clientep.Port);
                var biz = new CmdBiz();

                try
                {
                    var msgInfo = TransferFilesV3.ReceiveMsgInfo(client);
                    if (msgInfo.cmd == cmdtype.text.ToStr())
                    {
                        ControlHelper.AddMsg("收到客户端:" + msgInfo.msg);
                        TransferFilesV3.SendMsgInfo(client, msgInfo.msg, cmdtype.text.ToStr(), 1);
                        ControlHelper.AddMsg("接收完毕:" + clientep.Address + ":" + clientep.Port);
                    }
                    else if (msgInfo.cmd == cmdtype.file.ToStr())
                    {
                        var dt = DateTime.Now;
                        ControlHelper.AddMsg("开始接收文件......");
                        RecieveFileV2(client);
                        ControlHelper.AddMsg($"接收完毕:{clientep.Address}:{clientep.Port} 耗时:{ToolUtils.CalcTime(dt)}秒");

                    }
                    else
                    {
                        ControlHelper.AddMsg($"解析指令:{msgInfo.cmd}");
                        biz.ParseCmd(msgInfo);
                    }
                }
                catch (Exception ex)
                {
                    ControlHelper.AddMsg(ex.Message);
                    LogHelper.Error(ex.ToStr());
                }
            }
            client.Close();
            ControlHelper.AddMsg($"连接关闭");
        }
        /// <summary>
        /// 接收文件(弃用 为按压缩文件方式接收)
        /// </summary>
        /// <param name="clientSocket"></param>
        public static void RecieveFile(Socket client)
        {
            try
            {
                //获得[站点名称]   
                string siteno = System.Text.Encoding.UTF8.GetString(TransferFilesV3.ReceiveVarData(client));
                var dt = HttpSiteHelper.DoGet("/site/GetSiteInfo", "no=" + siteno);
                var dd = JObject.Parse(dt)["attr"].ToStr();
                var info = JsonConvert.DeserializeObject<SiteInfo>(dd);
                if (info == null)
                {
                    ControlHelper.AddMsg("参数错误：siteno" + siteno);
                    return;
                }
                var sitename = info.SiteName;
                ControlHelper.AddMsg(info.SiteName);
                //获得[文件名]   
                string SendFileName = System.Text.Encoding.UTF8.GetString(TransferFilesV3.ReceiveVarData(client));
                ControlHelper.AddMsg(SendFileName);

                //获得[包的大小]   
                string bagSize = TransferFilesV3.ReceiveMsg((client));

                //获得[包的总数量]   
                int bagCount = int.Parse(TransferFilesV3.ReceiveMsg(client));

                //获得[最后一个包的大小]   
                string bagLast = TransferFilesV3.ReceiveMsg(client);

                string roodir = Path.Combine(Environment.CurrentDirectory, "tempfile\\recievefile\\");
                string fullPath = roodir + SendFileName;


                //Path.Combine(Environment.CurrentDirectory, "tempfile\\recievefile\\" + SendFileName);
                if (!Directory.Exists(roodir)) Directory.CreateDirectory(roodir);

                //创建一个新文件   
                FileStream MyFileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);

                //已发送包的个数   
                int SendedCount = 0;
                while (SendedCount < bagCount)
                {

                    byte[] data = TransferFilesV3.ReceiveVarData(client);
                    if (data.Length == 0)
                    {
                        break;
                    }
                    else
                    {
                        SendedCount++;
                        //将接收到的数据包写入到文件流对象   
                        MyFileStream.Write(data, 0, data.Length);
                        //显示已发送包的个数     

                    }
                }
                //关闭文件流   
                MyFileStream.Close();
                //关闭套接字   
                client.Close();
                ControlHelper.AddMsg("文件名:" + SendFileName);
                //var info = new DBLiteBiz().GetSiteByName(sitename);
                var target = info.SiteDir;//移到到的站点
                                          // 接收文件后解决
                if (SendFileName.Contains("public.tagz"))
                {
                    var unrarDir = Path.Combine(Environment.CurrentDirectory, "tempfile\\temprar\\");//解压目录
                    if (Directory.Exists(unrarDir))
                        Directory.Delete(unrarDir, true);//删除目录
                    Directory.CreateDirectory(unrarDir);


                    ControlHelper.AddMsg("开始解压:" + fullPath + "=>" + unrarDir);
                    //if (File.Exists(fullPath))
                    //    ControlHelper.AddMsg("文件不存在:"+ fullPath);
                    //解压文件
                    var flag2 = FileCommonHelper.UnRarOrZip(unrarDir, fullPath, true, "");
                    if (flag2)
                    {
                        ControlHelper.AddMsg("开始移动,关闭站点");

                        StartOrStopWebsite(sitename, false);//关闭站点

                        //移动文件
                        FileCommonHelper.FileMove(unrarDir, target, null, null, "", true);

                        // File.Delete(fullPath);//清空文件
                        //  Directory.Delete(unrarDir, true);//删除目录;//清空文件

                        StartOrStopWebsite(sitename, true);//开启站点站点
                    }
                    else
                    {
                        ControlHelper.AddMsg("解压失败");
                    }

                    ControlHelper.AddMsg("站点开启");
                }


            }
            catch (Exception ex)
            {
                ControlHelper.AddMsg("收到异常,关闭重启" + ex.ToStr());
                LogHelper.Error(ex.ToStr());
            }
        }



        /// <summary>
        /// 接收文件(不解压模式)
        /// </summary>
        /// <param name="clientSocket"></param>
        public static void RecieveFileV2(Socket client)
        {
            try
            {
                //获得[站点名称]   
                string siteno = System.Text.Encoding.UTF8.GetString(TransferFilesV3.ReceiveVarData(client));
                var dt = HttpSiteHelper.DoGet("/site/GetSiteInfo", "no=" + siteno);
                var dd = JObject.Parse(dt)["attr"].ToStr();
                var info = JsonConvert.DeserializeObject<SiteInfo>(dd);
                if (info == null)
                {
                    ControlHelper.AddMsg("参数错误：siteno" + siteno);
                    return;
                }
                var sitename = info.SiteName;
                ControlHelper.AddMsg(info.SiteName);
                string roodir = Path.Combine(Environment.CurrentDirectory, "tempfile\\recievefile\\" + sitename);
                if (Directory.Exists(roodir)) Directory.Delete(roodir, true);
                //获得总文件数
                var totalFileCount = int.Parse(TransferFilesV3.ReceiveMsg(client));

                //  TransferFilesV3.SendMsgInfo(client, "继续发送", cmdtype.text.ToStr(), 1);
                var recFiles = 0;
                while (totalFileCount > recFiles)
                {
                    var isFlag = RecieveSingleFile(client, roodir);
                    if (!isFlag) break;//中断
                    recFiles++;
                    ControlHelper.AddToolStripStatusLabel($"进度:{totalFileCount}/{recFiles}");
                }
                var target = info.SiteDir;//移到到的站点
                client.Close();
                if (info.IsIIS.ToInt(1) == 1)//如果是IIS站点
                {
                    StartOrStopWebsite(sitename, false);//关闭站点
                    Thread.Sleep(2000);

                    ControlHelper.AddMsg($"开始移动文件到站点[{sitename}]......");
                    //移动文件
                    FileCommonHelper.FileMove(roodir, target, null, null, "", true);

                    Thread.Sleep(2000);
                    StartOrStopWebsite(sitename, true);//开启站点站点 
                }
                else if (info.IsIIS.ToInt(1) == 2)//如果是IIS站点
                {
                    //关闭进程
                    BatHelper.Exec($"taskkill /im {info.SiteName}.exe /f");
                    Thread.Sleep(2000);
                    //移动文件
                    FileCommonHelper.FileMove(roodir, target, null, null, "", true);

                    //启动进程
                    BatHelper.Exec("taskkill /im xxx.exe /f");
                }


            }
            catch (Exception ex)
            {
                ControlHelper.AddMsg("收到异常" + ex.ToStr());
                LogHelper.Error(ex.ToStr());
            }
        }
        private static bool RecieveSingleFile(Socket client, string roodir)
        {
            var isSuccess = true;
            //创建一个新文件   
            FileStream MyFileStream = null;
            string SendFileName = "";
            try
            {
                //获得[文件名]   
                SendFileName = System.Text.Encoding.UTF8.GetString(TransferFilesV3.ReceiveVarData(client));
                ControlHelper.AddMsg(SendFileName);

                //获得[包的大小]   
                string bagSize = TransferFilesV3.ReceiveMsg((client));

                //获得[包的总数量]   
                int bagCount = int.Parse(TransferFilesV3.ReceiveMsg(client));

                //获得[最后一个包的大小]   
                string bagLast = TransferFilesV3.ReceiveMsg(client);
                if (bagCount <= 0) bagCount = 1;
                else
                {
                    if (bagLast.ToInt2() > 0) bagCount += 1;

                }
                string fullPath = roodir + "\\" + SendFileName;

                if (!Directory.Exists(roodir)) Directory.CreateDirectory(roodir);
                var dir = fullPath.Substring(0, fullPath.LastIndexOf('\\'));
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                //创建一个新文件   
                MyFileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);

                //已发送包的个数   
                int SendedCount = 0;
                while (SendedCount < bagCount)
                {
                    byte[] data = TransferFiles.ReceiveVarData(client);
                    //将接收到的数据包写入到文件流对象   
                    MyFileStream.Write(data, 0, data.Length);
                    SendedCount++;
                }
                ControlHelper.AddMsg("接收end:" + SendFileName);
            }
            catch (Exception ex)
            {
                ControlHelper.AddMsg("接收异常:" + SendFileName);
                ControlHelper.AddMsg("接收中断!");
                isSuccess = false;
                LogHelper.Error(SendFileName + ":" + ex.ToString());
            }
            finally
            {
                if (MyFileStream != null)        //关闭文件流   
                    MyFileStream.Close();
            }
            return isSuccess;
        }
        /// <summary>
        /// 关闭其它站点，只开启输入名称的站点
        /// </summary>
        /// <param name="startSiteName"></param>
        public static void StartOrStopWebsite(string startSiteName, bool open)
        {
            var msg = ""; var r = 0;

            try
            {

                if (open)
                {
                    msg = ("开启站点");
                    r = IISSiteHelper.EnableSite(startSiteName, startSiteName, 1);
                }
                else
                {
                    msg = ("关闭站点");
                    r = IISSiteHelper.EnableSite(startSiteName, startSiteName, 2);
                }
            }
            catch (Exception ex)
            {
                msg += ex.Message;
                ControlHelper.AddMsg(ex.ToString());
            }
            ControlHelper.AddMsg($"{msg}操作{(r > 0 ? "成功" : "失败")}");

        }
    }



}
