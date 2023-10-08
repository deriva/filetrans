using Bc.Common.Helpers;
using Bc.Common.Utilities;
using Bc.Model;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
namespace Bc.Bussiness.FileTrans
{
    public static class FileClientV3
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
        public static bool Init(string IP, string Port)
        {

            if (clientSocket != null) clientSocket.Close();

            // labCon = _lab;
            //指向远程服务端节点
            ipep = new IPEndPoint(IPAddress.Parse(IP), Port.ToInt2());

            //创建套接字
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //连接到发送端
            try
            {
                clientSocket.Connect(ipep);
                ControlHelper.AddMsg("连接服务器" + IP + ":" + Port + ",成功");

                //Thread myThread = new Thread(ListenRec);
                //myThread.Start();
                return true;
            }
            catch (Exception ex)
            {
                // Debug.WriteLine("连接服务器失败！");
                ControlHelper.AddMsg(ex.ToStr());
            }
            return false;
        }

        private static void ListenRec()
        {

            while (true)
            {
                if (clientSocket != null)
                {
                    try
                    {
                        //var msginfo = TransferFilesV3.ReceiveMsgInfo(clientSocket);
                        //if (msginfo != null)
                        //    ControlHelper.AddMsg("收到回传:" + msginfo.msg);

                    }
                    catch
                    {
                        break;
                    }
                }
            }
        }        //将字节大小转换成字符串
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

        public static void SendMsg(string msg)
        {
            if (clientSocket == null) return;

            ControlHelper.AddMsg(msg);
            if (!clientSocket.Connected)
            {
                clientSocket.Connect(ipep);

            }
            TransferFilesV3.SendMsgInfo(clientSocket, msg, "", 0);


        }
        /// <summary>
        /// 发送文件到服务器
        /// </summary>
        /// <param name="sitename"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static bool SendFile(string siteno, string fullPath)
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


                TransferFilesV3.SendMsgInfo(clientSocket, "发送文件", cmdtype.file.ToStr(), 0);


                //发送[站点名]到客户端
                TransferFilesV3.Send(clientSocket, siteno);
                //发送[文件名]到客户端
                TransferFilesV3.Send(clientSocket, EzoneFile.Name);

                //发送[包的大小]到客户端
                TransferFilesV3.Send(clientSocket, PacketSize.ToString());

                //发送[包的总数量]到客户端
                TransferFilesV3.Send(clientSocket, PacketCount.ToString());

                //发送[最后一个包的大小]到客户端
                TransferFilesV3.Send(clientSocket, LastDataPacket.ToString());
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
                    if (TransferFilesV3.SendVarData(clientSocket, data) == 3)//3代表异常
                    {
                        isCut = true;
                        break;
                    }
                    stt = (DateTime.Now - now).TotalSeconds;
                    st = stt + "秒";
                    speed = i * 512 / 1024 / stt;
                    speedpack = i / stt;

                    remiantime = (PacketCount - i) * speedpack;
                    ControlHelper.AddMsg(string.Format("包数{0}=>{1},进度{2}%,耗时:{3},速度:{4}MB/s,预计剩余需:{5}秒", PacketCount, i, (i / (PacketCount * 1.0m) * 100).ToStr("N2"), SecondToStr(stt), speed.ToStr("N2"), SecondToStr(remiantime)));

                }
                stt = (DateTime.Now - now).TotalSeconds;
                if (stt > 60) st = (stt / 60).ToInt2() + "分" + (stt % 60) + "秒";
                speed = 0d;
                speed = PacketCount * 3 / stt;
                ControlHelper.AddMsg(string.Format("包数{0}=>{1},进度{2},耗时:{3},速度:{4}MB/s", PacketCount, PacketCount, "100%", SecondToStr(stt), speed.ToStr("N2")));
                //如果还有多余的数据包，则应该发送完毕！
                if (LastDataPacket != 0)
                {
                    data = new byte[LastDataPacket];
                    EzoneStream.Read(data, 0, data.Length);
                    TransferFilesV3.SendVarData(clientSocket, data);
                }

                //关闭套接字
                // clientSocket.Close();
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
                ControlHelper.AddMsg(ex.ToStr());

            }
            ControlHelper.AddMsg("发送完成");
            return false;
        }
        /// <summary>
        /// 发送文件到服务器(单文件发送)
        /// </summary>
        /// <param name="sitename"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static bool SendFileV2(string siteno, string fullPath)
        {
            try
            {
                if (clientSocket == null) return false;
                var allfile = FileCommonHelper.GetFiles(fullPath, null, true, true);


                //获得客户端节点对象
                IPEndPoint clientep = (IPEndPoint)clientSocket.RemoteEndPoint;


                TransferFilesV3.SendMsgInfo(clientSocket, "发送文件", cmdtype.file.ToStr(), 0);


                //发送[站点名]到客户端
                TransferFilesV3.Send(clientSocket, siteno);





                //发送[文件数]到客户端
                TransferFilesV3.Send(clientSocket, allfile.Count() + "");

                var st = 0;
                while (st < allfile.Count)
                {

                    SendSingleFile(allfile[st], fullPath);
                    st++;
                    ControlHelper.AddMsg($"{allfile.Count}/{st}");

                }
                return st > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToStr());
                ControlHelper.AddMsg(ex.ToStr());
            }

            ControlHelper.AddMsg("发送完成");
            return false;
        }

        private static void SendSingleFile(string filepath, string fullPath)
        {
            //   var filepath = allfile[j];
            var sendfilename = filepath.Replace(fullPath, "");
            ControlHelper.AddMsg($"发送文件{sendfilename}");
            LogHelper.Info(sendfilename);
            //创建一个文件对象
            FileInfo EzoneFile = new FileInfo(filepath);
            //打开文件流
            FileStream EzoneStream = EzoneFile.OpenRead();

            //包的大小
            int PacketSize = 1024 * 512;//512KB

            //包的数量
            int PacketCount = (int)(EzoneStream.Length / ((long)PacketSize));
            var totalsize = EzoneStream.Length;
            //最后一个包的大小
            int LastDataPacket = (int)(EzoneStream.Length - ((long)(PacketSize * PacketCount)));


            //发送[文件名]到客户端
            TransferFilesV3.Send(clientSocket, sendfilename);



            //发送[包的大小]到客户端
            TransferFilesV3.Send(clientSocket, PacketSize.ToString());

            //发送[包的总数量]到客户端
            TransferFilesV3.Send(clientSocket, PacketCount.ToString());

            //发送[最后一个包的大小]到客户端
            TransferFilesV3.Send(clientSocket, LastDataPacket.ToString());
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
                if (TransferFilesV3.SendVarData(clientSocket, data) == 3)//3代表异常
                {
                    isCut = true;
                    break;
                }
            }
            //如果还有多余的数据包，则应该发送完毕！
            if (LastDataPacket != 0)
            {
                data = new byte[LastDataPacket];
                EzoneStream.Read(data, 0, data.Length);
                TransferFilesV3.SendVarData(clientSocket, data);
            }

            //关闭套接字
            // clientSocket.Close();
            //关闭文件流
            EzoneStream.Close();
        }
        /// <summary>
        /// 解析所需要的参数
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static bool CmdPare(string parmmsg, string cmd)
        {
            if (clientSocket == null) return false;
            TransferFilesV3.SendMsgInfo(clientSocket, parmmsg, cmd, 0);
            return true;
        }
        /// <summary>
        /// 解析所需要的参数
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static ApiResult CmdPareMsgObj(MsgInfo obj)
        {
            if (obj == null) return ResultHelper.ToFail("无法获取参数"); 
            Init(obj.parm2, obj.parm3);//ip 和端口
            if (clientSocket == null) return ResultHelper.ToFail("失败:Socket无法连接");
            var r = TransferFilesV3.SendMsgInfo(clientSocket, obj);
            return ResultHelper.ToResponse(r);

        }
    }
}
