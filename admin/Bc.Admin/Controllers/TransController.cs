using Bc.Bussiness.FileTrans;
using Bc.Bussiness.IService;
using Bc.Bussiness.Service;
using Bc.Common.Helpers;
using Bc.Common.Utilities;
using Bc.Model;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Ocsp;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Bc.Admin.Controllers
{
    public class TransController : Controller
    {
        private readonly ISiteInfoService biz;
        private readonly IPublicCmdService bizCmd;



        public TransController()
        {
            biz = new SiteInfoService();
            bizCmd = new PublicCmdService();
        }
        public IActionResult Trans()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Bat()
        {
            var cmd = Request.Form["cmd"].ToStr();

            var r = BatHelper.Exec(cmd);
            Regex reg = new Regex("\r\n");
            r = reg.Replace(r, "<br/>");

            return Json(JsonHelper.JsonObject(true, r));
        }
        /// <summary>
        /// 发布站点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResult publicsite()
        {
            var msg = "";
            Request.EnableBuffering();
            var postJson = "";
            var stream = Request.Body;
            long? length = Request.ContentLength;
            if (length != null && length > 0)
            {
                // 使用这个方式读取，并且使用异步
                StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                postJson = streamReader.ReadToEndAsync().Result;
            }
            Request.Body.Position = 0;
            var jo = JObject.Parse(postJson);
         
            var no = jo["no"].ToStr();
            var env = jo["env"].ToInt2();
            var lst = biz.GetPublicFileDirByNoV2(no, env);
            SendFileHelper.SendFile(lst, ref msg);
            return ResultHelper.ToSuccess(msg);
        }
        /// <summary>
        /// 指令解析
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResult CmdPareMsg()
        {
            // 获取请求参数
            Request.EnableBuffering();
            var postJson = "";
            var stream = Request.Body;
            long? length =  Request.ContentLength;
            if (length != null && length > 0)
            {
                // 使用这个方式读取，并且使用异步
                StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                postJson = streamReader.ReadToEndAsync().Result;
            }
            Request.Body.Position = 0;
            var jo = JObject.Parse(postJson);
            var msg = jo["msg"].ToStr();
            ///站点指定解析细 
            var obj = JsonHelper.Deserialize<MsgInfo>(postJson.ToStr());
            return FileClientV3.CmdPareMsgObj(obj);
        }


    }
}
