using Bc.PublishWF.Biz;
using Bc.PublishWF.Biz.HttpServer;
using Bc.PublishWF.Biz.V3;
using Bc.PublishWF.Common;
using Bc.PublishWF.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bc.PublishWF.V3._0
{
    public partial class LocalServer : Form
    {
        //服务对象
        public static MyHttpServer httpServer;
        //http服务路由
        static Dictionary<string, string> routes = new Dictionary<string, string>
            {    {"site","index"},{ "home","index"},{ "tools","index"}  };
        public LocalServer()
        {
            InitializeComponent();
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            StartServer();
            btnShow(1);
        }
        public void StartServer()
        {
            int port = 15080;
            httpServer = new MyHttpServer(routes);//初始化，传入路由
            httpServer.respNotice += dataHandle;//回调方法，接收到http请求时触发
            httpServer.Start(port);//端口  
            ControlHelper.AddMsg($"已启动，端口:{port}");

        }
        private void btnShow(int type)
        {
            if (type == 1)//启动按钮关闭
            {
                btnEnable.Enabled = false;

            }
            else
            {
                btnEnable.Enabled = true;
            }

        }


        /// <summary>
        /// 收到请求的回调函数
        /// </summary>
        /// <param name="data">客户端请求的数据</param>
        /// <param name="resp">respon对象</param>
        /// <param name="route">网址路径,如/api/test</param>
        /// <param name="request_type">请求类型，get或者post</param>
        public void dataHandle(object data, HttpListenerResponse resp, string route = "", string request_type = "get")
        {
            var resp_data = "响应:";
            try
            {
                string view = string.Empty;
                var ss = route.Split('/');
                string controller = ss[1].ToLower();
                if (ss.Length >= 3)
                    view = route.Split('/')[2].ToLower();

                if (JsonHelper.Serialize(data) != "{}")
                {

                    var msg = data;// JsonConvert.SerializeObject(data);
                    ControlHelper.AddMsg($"执行开始:{controller}/{view}");
                    //根据路由key的val匹配相应的算法,以下是自己的逻辑
                    switch (controller)
                    {
                        case "site":
                            if (view == "publicsite")
                            {
                                var mgs = string.Empty;
                                var r = SendFileHelper.SendFile(data.ToStr(), ref mgs);
                                resp_data = ResultHelper.ToResponse(r, mgs).ToResult();
                            }
                            else
                            {
                                ///站点指定解析细 
                                var obj = JsonHelper.Deserialize<MsgInfo>(msg.ToStr());
                                resp_data = FileClientV3.CmdPareMsgObj(obj);
                            }

                            break;
                        case "tools":
                            {
                                if (view == "txttoimg")
                                {
                                    var obj = JObject.Parse(data.ToStr());
                                    var html = obj["html"].ToStr(); 
                                    var savepath= new TxtToImgHelper().ConvertToImg(html);
                                    resp_data = ResultHelper.ToResponse(1, savepath).ToResult();
                                }
                                break;
                            }
                        default:

                            break;
                    }
                    ControlHelper.AddMsg($"执行结束:{controller}/{view}");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToStr());
            }
            httpServer.responData(resp_data, resp);

        }


        private void btnStop_Click(object sender, EventArgs e)
        {
            httpServer.close(); ControlHelper.AddMsg($"服务已关闭"); btnShow(0);
        }

        private void LocalServer_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void LocalServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                this.Close();
            });
        }

        private void LocalServer_Load(object sender, EventArgs e)
        {
            ControlHelper.InitListBox(lstMsg);
            ControlHelper.InitToolStripStatusLabel(toolStripStatusLabel1);
            ControlHelper.AddToolStripStatusLabel("准备");
        }

        private void lstMsg_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
