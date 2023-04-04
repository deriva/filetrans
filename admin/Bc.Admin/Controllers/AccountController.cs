using Bc.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bc.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: AccountController
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginSubmit(string pwd)
        {
            string comkey = AppSettings.Configuration["LoginKey:CommonKey"];
            string superkey = AppSettings.Configuration["LoginKey:SuperKey"];
            if (pwd == comkey  ||pwd==superkey)
            {
                Response.Cookies.Append("authcode",pwd);
                return Json(new Models.JsonPage() { code = 1, msg = "" });
            }
       
            return Json(new Models.JsonPage() { code = 0, msg = "登录失败" });
        }


    }
}
