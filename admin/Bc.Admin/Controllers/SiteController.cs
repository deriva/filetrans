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
using System.Linq;

namespace Bc.Admin.Controllers
{
    [WebAuthorizeAttribute]
    public class SiteController : BaseController
    {
        private readonly ILogger<SiteController> _logger;
        private readonly ISiteInfoService biz;

        public SiteController(ILogger<SiteController> logger)
        {
            _logger = logger;
            biz = new SiteInfoService();
        }

        #region 站点管理
        public IActionResult List()
        {
            return View();
        }
        // GET: ApiController/Details/5
        [HttpGet]
        public ActionResult GetSiteInfoList(SiteInfoDto search)
        {
            //   search.PageSize = 1000;
            var lst = biz.QueryPages(search);
            return toResponse(lst.TotalCount, "", lst);
        }
        /// <summary>
        /// 获取站点的信息
        /// </summary>
        /// <param name="sitename"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSiteInfo(string no)
        {

            var lst = biz.GetSiteInfo(no);
            return toResponse(1, "", lst);
        }
        /// <summary>
        /// 保存站点
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveSiteInfo([FromBody] SiteInfoVm search)
        {
            if (search == null) return toFail("未获取到参数");
            if (!search.Id.HasValue) search.Id = 0;
             var info = search.Mapper<SiteInfo>();
            var lst = biz.SaveSiteInfo(info);
            return toResponse(lst, "");
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Cancel(int id)
        { 
            var lst = biz.Cancel(id);
            return toResponse(lst, "");
        }
        #endregion


        #region  发布程序调用的API

        /// <summary>
        /// 获取发布客户端需要的信息
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetIISWebSite(string no)
        {
            var info = biz.GetSiteInfo(no);
            if (info == null) return toFail("数据未获取到");
            var iisSiteInfo = new IISWebSite() { CLRVeersion = info.CLRVeersion, InstallPath = info.SiteDir, PoolName = info.SiteName, SiteName = info.SiteName };
            iisSiteInfo.SiteVirtualDirs = new SiteVirtualDirService().GetWhere(x => x.SiteNo == no).ToList();
            
            
            iisSiteInfo.Bindings= new SiteInfoService().GetWhere(x => x.No == no).ToList().Select(x=>new WebSiteBinding() { 
            Id=x.Id, HostName=x.SiteName, Port=x.Port, Protocol=x.Protocol, ServerIP=x.ServerIP,No=x.No
            
            }).ToList();
            return toResponse(1, "", iisSiteInfo);

        }




        #endregion

        // GET: ApiController/Details/5

        #region IIS站点服务
        /// <summary>
        /// 建立IIS站点
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateSite(string no)
        {
            var info = biz.GetSiteInfo(no);
            if (info == null) return toFail("不存在");
            var site = new IISWebSite();
            site.Bindings = new System.Collections.Generic.List<WebSiteBinding>();
            site.InstallPath = info.SiteDir;
            site.PoolName = info.SiteName;
            site.SiteName = info.SiteName;
            var tt = new WebSiteBinding() { Protocol = info.Protocol, Port = info.Port, HostName = "", ServerIP = "" };
            site.Bindings.Add(tt);
            var r = IISSiteHelper.Create(site);
            return toResponse(r, r > 0 ? "操作成功" : "操作失败");
        }
        /// <summary>
        /// 建立IIS站点
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EnableSite(string no, int type)
        {
            var r = 0;
            var info = biz.GetSiteInfo(no);
            if (info == null) return toFail("不存在");
            var site = new IISWebSite();
            site.Bindings = new System.Collections.Generic.List<WebSiteBinding>();
            site.InstallPath = info.SiteDir;
            site.PoolName = info.SiteName;
            site.SiteName = info.SiteName;
            var tt = new WebSiteBinding() { Protocol = info.Protocol, Port = info.Port, HostName = "", ServerIP = "" };
            site.Bindings.Add(tt);

            r = IISSiteHelper.EnableSite(site.PoolName, site.PoolName, type);

            return toResponse(r, r > 0 ? "操作成功" : "操作失败");
        }
        #endregion

    }
}
