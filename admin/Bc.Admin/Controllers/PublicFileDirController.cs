using Bc.Admin.Infrastructures;
using Bc.Bussiness.IService;
using Bc.Bussiness.Service;
using Bc.Common.Helpers;
using Bc.Common.Utilities;
using Bc.Model;
using Bc.Model.Api;
using Bc.Model.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System.Linq;

namespace Bc.Admin.Controllers
{
    [WebAuthorizeAttribute]
    public class PublicFileDirController : BaseController
    {

        private readonly ISiteInfoService biz;
        private readonly IPublicCmdService bizCmd;



        public PublicFileDirController()
        {
            biz = new SiteInfoService();
            bizCmd = new PublicCmdService();
        }



        #region 发布目录管理
        public IActionResult Public()
        {
            return View();
        }
        public IActionResult ExlcludeDir(SitePublicExcludeDirDto search)
        {
            return View(search);
        }
        [HttpGet]
        public ActionResult GetPublicInfoList(PublicFileDirDto search)
        {
            //  search.PageSize = 1000;
            var lst = biz.PublicFileDirQueryPages(search);
            return toResponse(lst.TotalCount, "", lst);
        }
   

        /// <summary>
        /// 获取发布目录的信息V2版本（BcTrans用到）
        /// </summary>
        /// <param name="sitename"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPublicFileDirByNoV2(string no, int env)
        {
            var lst = biz.GetPublicFileDirByNoV2(no, env);
            return toResponse(1, "", lst);
        }
        [HttpGet]
        public ActionResult GetSitePublicExcludeDir(SitePublicExcludeDirDto search)
        {
            if (search.PublicNo.IsEmpty()) return toFail("发布编号不为空");
            // search.PageSize = 1000;
            var lst = biz.SitePublicExcludeDirQueryPages(search);
            return toResponse(1, "", new { data = lst.DataSource, count = lst.TotalCount });
        }



        /// <summary>
        /// 保存发布目录
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SavePublicFileDir([FromBody] PublicFileDir search)
        {
            if (search.PublicNo.IsEmpty()) search.PublicNo = CaptchaUtil.GetRandomEnDigitalText(10);
            var lst = biz.SavePublicFileDir(search);
            return toResponse(lst, "");
        }

        /// <summary>
        /// 保存排除的目录
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveSitePublicExcludeDir([FromBody] SitePublicExcludeDir search)
        {
            if (search.PublicNo.IsEmpty()) return toFail("发布编号不为空");
            var lst = biz.SaveSitePublicExcludeDir(search);
            return toResponse(lst, "");
        }




        /// <summary>
        /// 发布目录对站点
        /// </summary>
        /// <returns></returns>
        public IActionResult PublicToSite()
        {
            return View();
        }
        /// <summary>
        ///  获取发布目录对站点
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPublicToSiteList(PublicToSite search)
        {
            var lst = biz.PublicToSiteList(search);
            return toResponse(lst.Count, "", lst);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SavePublicToSite([FromBody] PublicToSite search)
        {
            var lst = biz.SavePublicToSite(search);
            return toResponse(lst);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePublicToSite([FromBody] PublicToSite search)
        {
            var lst = biz.DeletePublicToSite(search);
            return toResponse(lst);
        }

        #endregion

        #region 发布编译指令管理
        public IActionResult PublicCmd()
        {
            return View();
        }
     
        [HttpPost]
        public ActionResult SavePublicCmd(PublicCmd info)
        {
            var msg = "";
            var lst = bizCmd.Save(info, ref   msg);
            return toResponse(lst, msg, lst);
        }
      

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sitename"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePublicCmd(int id)
        {
            var lst = bizCmd.Delete(id);
            return toResponse(1, "", lst);
        }
        [HttpGet]
        public ActionResult PublicCmdList(string PublicNo, string groupName="")
        {
            var lst = bizCmd.PublicCmdList(PublicNo,   groupName);
            return toResponse(1, "", new { DataSource = lst, TotalCount = lst.Count });
        }

         
        #endregion



    }
}
