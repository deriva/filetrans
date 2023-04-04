using Bc.Admin.Infrastructures;
using Bc.Bussiness.IService;
using Bc.Bussiness.Service;
using Bc.Model;
using Bc.Model.Dto.System;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bc.Admin.Controllers
{
    [WebAuthorizeAttribute]
    public class ServerAppController : BaseController
    {
        private readonly IServerAppService biz;

        public ServerAppController()
        {
            biz = new ServerAppService();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetList(ServerAppDto search)
        {
            var lst = biz.QueryPages(search);
            return toResponse(lst.TotalCount, "", lst);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save([FromBody] ServerApp info)
        {
           // info.Port = HttpContext.Request.ur.Port.ToString();
            if (info == null) return toFail("失败");
            var r = biz.Save(info);
            return toResponse(r, "");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string ip, string port)
        {
          //  string port = Request.Host.Port.ToString();
            var r = biz.Delete(ip,port);
            return toResponse(r, "");
        }
    }
}
