using Bc.Admin.Infrastructures;
using Bc.Bussiness.IService;
using Bc.Bussiness.Service;
using Bc.Model;
using Bc.Model.Dto;
using Bc.Model.Dto.System;
using Microsoft.AspNetCore.Mvc;

namespace Bc.Admin.Controllers
{
    [WebAuthorizeAttribute]
    public class WebSiteBindingController : BaseController
    {
        private readonly IWebSiteBindingService biz;

        public WebSiteBindingController()
        {
            biz = new WebSiteBindingService();
        }
        //  private readonly ISiteInfoService biz;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(WebSiteBindingDto search)
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
        public ActionResult Save([FromBody] WebSiteBinding info)
        { 
            var r = biz.Save(info);
            return toResponse(r, "");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var r = biz.Delete(id);
            return toResponse(r, "");
        }
    }
}
