using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Bussiness.Extensions
{
    public class SignalRHub : Hub
    {
        public static ConcurrentDictionary<string, string> OnLineUsers = new ConcurrentDictionary<string, string>();

        // 定义了 SendMessage 方法,客户端可以调用该方法
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMsg", user, message);
        }
        /// <summary>
        /// 客户连接成功时触发
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var cid = Context.ConnectionId;

            ////根据id获取指定客户端
            //var client = Clients.Client(cid);

            ////向指定用户发送消息
            //await client.SendAsync("Self", cid);

            ////像所有用户发送消息
            //await Clients.All.SendAsync("AddMsg", $"{cid}加入了聊天室");
        }

        //[HubMethodName("send")]
        //public void Send(string message)
        //{
        //    string clientName = OnLineUsers[Context.ConnectionId];
        //    message = HttpUtility.HtmlEncode(message).Replace("\r\n", "<br/>").Replace("\n", "<br/>");
        //    Clients.All.receiveMessage(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), clientName, message);
        //}

        //[HubMethodName("sendOne")]
        //public void Send(string toUserId, string message)
        //{
        //    string clientName = OnLineUsers[Context.ConnectionId];
        //    message = HttpUtility.HtmlEncode(message).Replace("\r\n", "<br/>").Replace("\n", "<br/>");
        //    Clients.Caller.receiveMessage(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), string.Format("您对 {1}", clientName, OnLineUsers[toUserId]), message);
        //    Clients.Client(toUserId).receiveMessage(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), string.Format("{0} 对您", clientName), message);
        //}

        //public override System.Threading.Tasks.Task OnConnected()
        //{
        //    string clientName = Context.QueryString["clientName"].ToString();
        //    OnLineUsers.AddOrUpdate(Context.ConnectionId, clientName, (key, value) => clientName);
        //    Clients.All.userChange(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), string.Format("{0} 加入了。", clientName), OnLineUsers.ToArray());
        //    return base.OnConnected();
        //}

        //public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        //{
        //    string clientName = Context.QueryString["clientName"].ToString();
        //    Clients.All.userChange(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), string.Format("{0} 离开了。", clientName), OnLineUsers.ToArray());
        //    OnLineUsers.TryRemove(Context.ConnectionId, out clientName);
        //    return base.OnDisconnected(stopCalled);
        //}
    }
}
