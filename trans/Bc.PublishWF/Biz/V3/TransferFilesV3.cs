
using Bc.PublishWF.Common;
using Bc.PublishWF.Model;
using System;
using System.Net.Sockets;
using System.Windows.Forms;
/// <summary>
/// 升级版本
/// </summary>
namespace Bc.PublishWF.Biz.V3
{

    public class TransferFilesV3
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
            catch(Exception ex)
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
        public static int SendMsgInfo(Socket s, string msg, string cmd, int flag,string parm1="",string parm2="")
        {
            var obj = new MsgInfo() { cmd = cmd, flag = flag, msg = msg,parm1=parm1,parm2= parm2 };
            var str = JsonHelper.Serialize(obj); 
            var data = System.Text.Encoding.UTF8.GetBytes(str);
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
        public static int SendMsgInfo(Socket s, MsgInfo obj)
        {
         //   var obj = new MsgInfo() { cmd = cmd, flag = flag, msg = msg, parm1 = parm1, parm2 = parm2 };
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
            try
            {
                var data = ReceiveVarData(s);
                var str = System.Text.Encoding.UTF8.GetString(data);
                LogHelper.Info("ReceiveMsgInfo", str);
                return JsonHelper.Deserialize<MsgInfo>(str);
            }catch(Exception ex)
            {
                LogHelper.Error(ex.ToStr());
                return null;
            }
        }



        public static byte[] ReceiveVarData(Socket s)
        {
            try
            {
                int total = 0;
                int recv;
                byte[] datasize = new byte[4];
                //recv = s.Receive(datasize);
                recv = s.Receive(datasize, 0, datasize.Length, SocketFlags.None);
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
            catch(Exception ex)
            {
                return null;
            }
        }

        #endregion
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
