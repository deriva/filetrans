using Bc.Admin.Infrastructures;
using Bc.Bussiness.IService;
using Bc.Bussiness.Service;
using Bc.Model;
using Bc.Model.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Bc.Admin.Controllers
{
    [WebAuthorizeAttribute]
    public class SiteVirtualDirController : BaseController
    {
        private readonly ISiteVirtualDirService biz;

        public SiteVirtualDirController()
        {
            biz = new SiteVirtualDirService();
        }
        //  private readonly ISiteInfoService biz;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(SiteVirtualDirDto search)
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
        public ActionResult Save([FromBody] SiteVirtualDir info)
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
