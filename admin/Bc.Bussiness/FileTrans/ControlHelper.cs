using Bc.Bussiness.Extensions;
using Bc.Common;
using Bc.Common.Utilities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Bussiness.FileTrans
{
    public class ControlHelper
    {

        public static async void AddMsg(string msg)
        {
            var _hubContext = GlobalContext.ServiceProvider.GetService<IHubContext<SignalRHub>>();


            if (_hubContext != null)
            {
                var msg2 = $"{DateTime.Now.ToStr("HH:mm:ss:fff")}-{msg}";
                await _hubContext.Clients.All.SendAsync("ReceiveMsg", msg2);
            }

        }

    }
}
