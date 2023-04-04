using Bc.PublishWF.Common;
using Bc.PublishWF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bc.PublishWF.Biz.V3
{
    /// <summary>
    /// 指令管理
    /// </summary>
    public class CmdBiz
    {
        /// <summary>
        /// 指令解析
        /// </summary>
        /// <param name="msg"></param>
        public void ParseCmd(MsgInfo msg)
        {

            var obj = new object();
            try
            {
                Type ht = this.GetType();
                MethodInfo m = ht.GetMethod(msg.cmd, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                object[] parm = new object[1];
                parm[0] = msg;
                obj = m.Invoke(this, parm);
            }
            catch (Exception ex)
            {

                LogHelper.Error(ex);

            }

        }
        #region IIS指令解析
        /// <summary>
        /// 解析指令
        /// </summary>
        /// <param name="msg"></param>
        public  int IIS_Create(MsgInfo msg)
        {
          //  var msg2 = HttpSiteHelper.GetIISWebSite(msg.msg.ToString());
            var site = JsonHelper.Deserialize<IISWebSite>(msg.msg);
            return IISSiteHelper.Create(site);
        }
        /// <summary>
        /// 启动/停止/移除站点
        /// </summary>
        /// <param name="msg"></param>
        public  int IIS_Enabled(MsgInfo msg)
        {
           // var msg2 = HttpSiteHelper.GetIISWebSite(msg.msg.ToString());
            var site = JsonHelper.Deserialize<IISWebSite>(msg.msg);
            return IISSiteHelper.EnableSite(site.SiteName, site.PoolName,msg.parm1.ToInt2());
        }
        #endregion
         
    }
}
