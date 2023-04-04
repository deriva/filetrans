using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.PublishWF.Model
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
}
 