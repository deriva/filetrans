using Bc.Common.Utilities;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
/// <summary>
/// 升级版本
/// </summary>
namespace Bc.Common.Helpers
{
    /// <summary>
    /// 消息信息
    /// </summary>
    public class MsgInfo
    {
        /// <summary>
        /// 消息回传方向:0客户端-->发服务端    1服务端回客户端
        /// </summary>
        public int flag { get; set; }

        /// <summary>
        /// 指令
        /// </summary>
        public string cmd { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        public string msg { get; set; }
    }
    public enum cmdtype
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        text = 1,

        /// <summary>
        /// 文件消息
        /// </summary>
        file = 2,

    }
    public class TransferFiles
    {
        #region 发送
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

        public static int Send(Socket s, string msg)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(msg);
            return SendVarData(s, data);
        }
        /// <summary>
        /// 发送格式的消息符
        /// </summary>
        /// <param name="s"></param>
        /// <param name="msg"></param>
        /// <param name="cmd"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static int SendMsgInfo(Socket s, string msg, string cmd, int flag)
        {
            var obj = new MsgInfo() { cmd = cmd, flag = flag, msg = msg };
            var str = JsonHelper.Serialize(obj);
            var data = System.Text.Encoding.UTF8.GetBytes(str);
            return SendVarData(s, data);
        }
        #endregion

        #region 接收
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
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReceiveMsg(Socket s)
        {
            var data = ReceiveVarData(s);
            return System.Text.Encoding.UTF8.GetString(data);
        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static MsgInfo ReceiveMsgInfo(Socket s)
        {
            var data = ReceiveVarData(s);
            return JsonHelper.Deserialize<MsgInfo>(System.Text.Encoding.UTF8.GetString(data));
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

        #endregion
    }

     
    public static class FileClient
    {
       
        //  static Label labCon;
        private static Socket clientSocket;
        static IPEndPoint ipep;
        /// <summary>
        /// 连接服务端
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        public static void Init(string IP, string Port)
        {
           
            // labCon = _lab;
            //指向远程服务端节点
            ipep = new IPEndPoint(IPAddress.Parse(IP), Port.ToInt2());

            //创建套接字
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //连接到发送端
            try
            {
                clientSocket.Connect(ipep);
                AddMsg("连接服务器" + IP + ":" + Port + ",成功");

                Thread myThread = new Thread(ListenRec);
                myThread.Start();
            }
            catch (Exception ex)
            {
                // Debug.WriteLine("连接服务器失败！");
                AddMsg(ex.ToStr());
            }

        }

        private static void ListenRec()
        {

            while (true)
            {
                if (clientSocket != null)
                {
                    try
                    {
                        var msginfo = TransferFiles.ReceiveMsgInfo(clientSocket);
                        AddMsg("收到回传:" + msginfo.msg);

                    }
                    catch
                    {
                        break;
                    }
                }
            }
        }
        public static void SendMsg(string msg)
        {
            if (clientSocket == null) return;

            AddMsg(msg);
            if (!clientSocket.Connected)
            {
                clientSocket.Connect(ipep);

            }
            TransferFiles.SendMsgInfo(clientSocket, msg, cmdtype.text.ToStr(), 0);


        }
        public static bool SendFile(string sitename, string fullPath)
        {
            try
            {
                if (clientSocket == null) return false;


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
                IPEndPoint clientep = (IPEndPoint)clientSocket.RemoteEndPoint;


                TransferFiles.SendMsgInfo(clientSocket, "发送文件", cmdtype.file.ToStr(), 0);


                //发送[站点名]到客户端
                TransferFiles.Send(clientSocket, sitename);
                //发送[文件名]到客户端
                TransferFiles.Send(clientSocket, EzoneFile.Name);

                //发送[包的大小]到客户端
                TransferFiles.Send(clientSocket, PacketSize.ToString());

                //发送[包的总数量]到客户端
                TransferFiles.Send(clientSocket, PacketCount.ToString());

                //发送[最后一个包的大小]到客户端
                TransferFiles.Send(clientSocket, LastDataPacket.ToString());
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
                    if (TransferFiles.SendVarData(clientSocket, data) == 3)//3代表异常
                    {
                        isCut = true;
                        break;
                    }
                    stt = (DateTime.Now - now).TotalSeconds;
                    st = stt + "秒";
                    speed = i * 512 / 1024 / stt;
                    speedpack = i / stt;

                    remiantime = (PacketCount - i) * speedpack;
                 
                }
                stt = (DateTime.Now - now).TotalSeconds;
                if (stt > 60) st = (stt / 60).ToInt2() + "分" + (stt % 60) + "秒";
                speed = 0d;
                speed = PacketCount * 3 / stt;
               //如果还有多余的数据包，则应该发送完毕！
                if (LastDataPacket != 0)
                {
                    data = new byte[LastDataPacket];
                    EzoneStream.Read(data, 0, data.Length);
                    TransferFiles.SendVarData(clientSocket, data);
                }

                //关闭套接字
                clientSocket.Close();
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
        public static string FileSizeToStr(double d)
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
        public static string SecondToStr(double d)
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
        public static void AddMsg(string msg)
        {

                 System.Console.WriteLine(string.Format("{0}->{1}", DateTime.Now.ToStr("yyyy-MM-dd HH:mm:ss"), msg));
             
             
        }
        
    }

 
}
