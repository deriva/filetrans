using Bc.PublishWF.Common;
using Bc.PublishWF.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Bc.PublishWF.Biz
{
    /// <summary>
    /// socket传输类
    /// </summary>
    public class TransferFiles
    {
        public static int SendData(Socket s, byte[] data)
        {
            int total = 0;
            int size = data.Length;
            int dataleft = size;
            int sent;

            while (total < size)
            {
                sent = s.Send(data, total, dataleft, SocketFlags.None);
                total += sent;
                dataleft -= sent;
            }

            return total;
        }


        public static byte[] ReceiveData(Socket s, int size)
        {
            int total = 0;
            int dataleft = size;
            byte[] data = new byte[size];
            int recv;
            while (total < size)
            {
                recv = s.Receive(data, total, dataleft, SocketFlags.None);
                if (recv == 0)
                {
                    data = null;
                    break;
                }

                total += recv;
                dataleft -= recv;
            }
            return data;
        }

        public static int SendVarData(Socket s, byte[] data)
        {
            int total = 0;
            int size = data.Length;
            int dataleft = size;
            int sent;
            byte[] datasize = new byte[4];

            try
            {
                datasize = BitConverter.GetBytes(size);
                sent = s.Send(datasize);

                while (total < size)
                {
                    sent = s.Send(data, total, dataleft, SocketFlags.None);
                    total += sent;
                    dataleft -= sent;
                }

                return total;
            }
            catch
            {
                return 3;

            }
        }

        public static byte[] ReceiveVarData(Socket s)
        {
            int total = 0;
            int recv;
            byte[] datasize = new byte[4];
            recv = s.Receive(datasize, 0, 4, SocketFlags.None);
            int size = BitConverter.ToInt32(datasize, 0);
            int dataleft = size;
            byte[] data = new byte[size];
            while (total < size)
            {
                recv = s.Receive(data, total, dataleft, SocketFlags.None);
                if (recv == 0)
                {
                    data = null;
                    break;
                }
                total += recv;
                dataleft -= recv;
            }
            return data;
        }
    }

    public static class FileServer
    {
        static ListBox plstbox;
        private static Socket serverSocket;
        public static void Init(ListBox listbox)
        {
            plstbox = listbox;
            //服务器IP地址
            var serverip = ConfigUtils.GetValue<string>("serverip").Split(':');
            var myProt = serverip[1].ToInt2();
            string ipstr = serverip[0];
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, myProt);
            serverSocket = new Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(localEP);
            //绑定IP地址：端口
            serverSocket.Listen(10);    //设定最多10个排队连接请求
            AddMsg(string.Format("启动监听{0}成功", serverSocket.LocalEndPoint.ToString()));

            //通过Clientsoket发送数据
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
        }
        public static void Exit()
        {
            serverSocket.Close();
            serverSocket = null;
        }
        private static void ListenClientConnect()
        {
            while (true)
            {
                if (serverSocket != null)
                {
                    try
                    {
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
                AddMsg("非法入侵，来自:" + clientep.Address + ":" + clientep.Port);
                flag = false;
            }
            if (flag)
            {
                AddMsg("收到远程终端:" + clientep.Address + ":" + clientep.Port);

            }

            //SocketServer.pForm.ShowMessageBox(SendFileName + "接收完毕！");
        }
        /// <summary>
        /// 接收文件
        /// </summary>
        /// <param name="clientSocket"></param>
        public static void RecieveFile(object clientSocket)
        {
            try
            {
                Socket client = clientSocket as Socket;


                //获得[站点名称]   
                string siteno = System.Text.Encoding.Unicode.GetString(TransferFiles.ReceiveVarData(client));
                var dt = HttpSiteHelper.DoGet("site/GetSiteInfo", "no=" + siteno); ;
                var info = JsonConvert.DeserializeObject<SiteInfo>(dt);
                if (info == null)
                {
                    AddMsg("参数错误：siteno" + siteno);
                    return;
                }
                var sitename = info.SiteName;
                AddMsg(info.SiteName);
                //获得[文件名]   
                string SendFileName = System.Text.Encoding.Unicode.GetString(TransferFiles.ReceiveVarData(client));
                AddMsg(SendFileName);

                //获得[包的大小]   
                string bagSize = System.Text.Encoding.Unicode.GetString(TransferFiles.ReceiveVarData(client));

                //获得[包的总数量]   
                int bagCount = int.Parse(System.Text.Encoding.Unicode.GetString(TransferFiles.ReceiveVarData(client)));

                //获得[最后一个包的大小]   
                string bagLast = System.Text.Encoding.Unicode.GetString(TransferFiles.ReceiveVarData(client));

                string roodir = Path.Combine(Environment.CurrentDirectory, "tempfile\\recievefile\\");
                string fullPath = roodir + SendFileName;


                //Path.Combine(Environment.CurrentDirectory, "tempfile\\recievefile\\" + SendFileName);
                if (!Directory.Exists(roodir)) Directory.CreateDirectory(roodir);

                //创建一个新文件   
                FileStream MyFileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);

                //已发送包的个数   
                int SendedCount = 0;
                while (true)
                {

                    byte[] data = TransferFiles.ReceiveVarData(client);
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
                AddMsg("文件名:" + SendFileName);
                //var info = new DBLiteBiz().GetSiteByName(sitename);
                var target = info.SiteDir;//移到到的站点
                                          // 接收文件后解决
                if (SendFileName.Contains("public.tagz"))
                {
                    var unrarDir = Path.Combine(Environment.CurrentDirectory, "tempfile\\temprar\\");//解压目录
                    if (Directory.Exists(unrarDir))
                        Directory.Delete(unrarDir, true);//删除目录
                    Directory.CreateDirectory(unrarDir);


                    AddMsg("开始解压:" + fullPath + "=>" + unrarDir);
                    //if (File.Exists(fullPath))
                    //    AddMsg("文件不存在:"+ fullPath);
                    //解压文件
                    var flag2 = FileCommonHelper.UnRarOrZip(unrarDir, fullPath, true, "");
                    if (flag2)
                    {
                        AddMsg("开始移动,关闭站点");

                        StartOrStopWebsite(sitename, false);//关闭站点

                        //移动文件
                        FileCommonHelper.FileMove(unrarDir, target, null, null, "", true);

                        // File.Delete(fullPath);//清空文件
                        //  Directory.Delete(unrarDir, true);//删除目录;//清空文件

                        StartOrStopWebsite(sitename, true);//开启站点站点
                    }
                    else
                    {
                        AddMsg("解压失败");
                    }
                    // AddMsg("收到完毕:" + clientep.Address + ":" + clientep.Port);
                    AddMsg("站点开启");
                }


            }
            catch (Exception ex)
            {
                AddMsg("收到异常,关闭重启" + ex.ToStr());
                LogHelper.Error(ex.ToStr());
            }
        }

        public static void AddMsg(string msg)
        {
            if (plstbox != null)
            {
                if (plstbox.InvokeRequired)
                {
                    Action<string> myAction = (p) => { AddMsg(p); };
                    plstbox.Invoke(myAction, msg);
                }
                else
                {
                    plstbox.Items.Insert(0, string.Format("{0}->{1}", DateTime.Now.ToStr("yy-MM-dd HH:mm:ss"), msg));
                }
            }
        }

        /// <summary>
        /// 关闭其它站点，只开启输入名称的站点
        /// </summary>
        /// <param name="startSiteName"></param>
        public static void StartOrStopWebsite(string startSiteName, bool open)
        {
            try
            {
                var webManager = new Microsoft.Web.Administration.ServerManager();
                var startSite = webManager.Sites[startSiteName];

                if (startSite == null)
                {
                    Console.WriteLine("Can't not find site:{0}", startSiteName);
                    return;
                }
                if (open)
                {
                    AddMsg("开启站点");
                    startSite.Start();
                }
                else
                {
                    AddMsg("关闭站点");
                    startSite.Stop();
                }
            }
            catch (Exception ex)
            {
                AddMsg(ex.ToString());
            }


        }
    }

    public class FileClient
    {
        private Socket client;
        ListBox plstbox;
        Label labCon;
        public FileClient(string IP, int Port, ListBox listbox, Label _lab)
        {
            client = GetSocket(IP, Port);
            AddMsg("连接服务器" + IP + ":" + Port );

            plstbox = listbox;
            labCon = _lab;
        }
        Socket GetSocket(string IP, int Port)
        {
            //指向远程服务端节点
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(IP), Port);

            //创建套接字
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //连接到发送端
            try
            {
                client.Connect(ipep);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine("连接服务器失败！");
                AddMsg(ex.ToStr());
                return null;
            }
            return client;
        }


      
        public bool SendFile(string sitename, string fullPath)
        {
            try
            {
                if (client == null) return false;

                //创建一个文件对象
                FileInfo EzoneFile = new FileInfo(fullPath);
                //打开文件流
                FileStream EzoneStream = EzoneFile.OpenRead();

                //包的大小
                int PacketSize = 1024 * 512;//512KB

                //包的数量
                int PacketCount = (int)(EzoneStream.Length / ((long)PacketSize));
                var totalsize = EzoneStream.Length;
                //最后一个包的大小
                int LastDataPacket = (int)(EzoneStream.Length - ((long)(PacketSize * PacketCount)));
           
                //获得客户端节点对象
                IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
                //发送[站点名]到客户端
                TransferFiles.SendVarData(client, System.Text.Encoding.Unicode.GetBytes(sitename));

                //发送[文件名]到客户端
                TransferFiles.SendVarData(client, System.Text.Encoding.Unicode.GetBytes(EzoneFile.Name));

                //发送[包的大小]到客户端
                TransferFiles.SendVarData(client, System.Text.Encoding.Unicode.GetBytes(PacketSize.ToString()));

                //发送[包的总数量]到客户端
                TransferFiles.SendVarData(client, System.Text.Encoding.Unicode.GetBytes(PacketCount.ToString()));

                //发送[最后一个包的大小]到客户端
                TransferFiles.SendVarData(client, System.Text.Encoding.Unicode.GetBytes(LastDataPacket.ToString()));
                var now = DateTime.Now;
                var stt = 0d; var speed = 0d;
                var speedpack = 0d;//传输一个包的速度
                var st = stt + "秒"; var remiantime = 0d;//预计需要的时间
                bool isCut = false;
                //数据包
                byte[] data = new byte[PacketSize];
                //开始循环发送数据包
                for (int i = 0; i < PacketCount; i++)
                {
                    //从文件流读取数据并填充数据包
                    EzoneStream.Read(data, 0, data.Length);
                    //发送数据包
                    if (TransferFiles.SendVarData(client, data) == 3)//3代表异常
                    {
                        isCut = true;
                        break;
                    }
                    stt = (DateTime.Now - now).TotalSeconds;
                    st = stt + "秒";
                    speed = i * 512 / 1024 / stt;
                    speedpack = i / stt;

                    remiantime = (PacketCount - i) * speedpack;
                    AddLabMsg(string.Format("包数{0}=>{1},进度{2}%,耗时:{3},速度:{4}MB/s,预计剩余需:{5}秒", PacketCount, i, (i / (PacketCount * 1.0m) * 100).ToStr("N2"), SecondToStr(stt), speed.ToStr("N2"), SecondToStr(remiantime)));

                }
                stt = (DateTime.Now - now).TotalSeconds;
                if (stt > 60) st = (stt / 60).ToInt2() + "分" + (stt % 60) + "秒";
                speed = 0d;
                speed = PacketCount * 3 / stt;
                AddLabMsg(string.Format("包数{0}=>{1},进度{2},耗时:{3},速度:{4}MB/s", PacketCount, PacketCount, "100%", SecondToStr(stt), speed.ToStr("N2")));
                //如果还有多余的数据包，则应该发送完毕！
                if (LastDataPacket != 0)
                {
                    data = new byte[LastDataPacket];
                    EzoneStream.Read(data, 0, data.Length);
                    TransferFiles.SendVarData(client, data);
                }

                //关闭套接字
                client.Close();
                //关闭文件流
                EzoneStream.Close();
                if (!isCut)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToStr());
                AddMsg(ex.ToStr());

            }
            AddMsg("发送完成");
            return false;
        }

         

        //将字节大小转换成字符串
        public   string FileSizeToStr(double d)
        {
            var st = d + "B";
            if (d >= 1024 * 1024 * 1024)
            {
                st = (d / 1024 / 1024 / 1024).ToStr("N2") + "G";
            }
            else if (d >= 1024 * 1024)
            {
                st = (d / 1024 / 1024).ToStr("N2") + "MB";
            }
            else if (d >= 1024)
            {
                st = (d / 1024).ToStr("N2") + "KB";
            }
            return st;
        }
        /// <summary>
        /// 将秒转换成显示字符串
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public   string SecondToStr(double d)
        {
            var st = d.ToStr("N2") + "秒";
            if (d >= 60 * 60)
            {
                st = (d / 60 / 60).ToInt2() + "小时" + (d % 60).ToInt2() + "分";
            }
            else if (d >= 60)
            {
                st = (d / 60).ToInt2() + "分" + (d % 60).ToInt2() + "秒";
            }
            return st;
        }
        public   void AddMsg(string msg)
        {

            if (plstbox != null)
            {
                if (plstbox.InvokeRequired)
                {
                    Action<string> myAction = (p) => { AddMsg(p); };
                    plstbox.Invoke(myAction, msg);
                }
                else
                {
                    if (plstbox.Items.Count > 100)
                    {
                        plstbox.Items.Clear();
                    }
                    plstbox.Items.Insert(0, string.Format("{0}->{1}", DateTime.Now.ToStr("yyyy-MM-dd HH:mm:ss"), msg));
                }
            }
        }
        public   void AddLabMsg(string msg)
        {

            if (plstbox != null)
            {
                if (plstbox.InvokeRequired)
                {
                    Action<string> myAction = (p) => { AddLabMsg(p); };
                    labCon.Invoke(myAction, msg);
                }
                else
                {
                    labCon.Text = msg;
                }
            }
        }
    }


    public class TextBoxWriter : System.IO.TextWriter
    {
        ListBox lstBox;
        delegate void VoidAction();

        public TextBoxWriter(ListBox box)
        {
            lstBox = box;
        }

        public override void Write(string value)
        {
            VoidAction action = delegate
            {
                lstBox.Items.Insert(0, string.Format("[{0:HH:mm:ss}]{1}", DateTime.Now, value));
            };
            lstBox.BeginInvoke(action);
        }

        public override void WriteLine(string value)
        {
            VoidAction action = delegate
            {
                lstBox.Items.Insert(0, string.Format("[{0:HH:mm:ss}]{1}", DateTime.Now, value));
            };
            lstBox.BeginInvoke(action);
        }

        public override System.Text.Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
