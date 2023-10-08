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


        public string parm1 { get; set; }

        public string parm2 { get; set; }
        public string parm3 { get; set; }
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
    
  
 
}
