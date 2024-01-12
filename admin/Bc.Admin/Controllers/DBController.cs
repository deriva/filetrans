using Bc.Admin.Infrastructures;
using Bc.Bussiness.IService;
using Bc.Bussiness.Service;
using Bc.Common.Helpers;
using Bc.Common.Utilities;
using Bc.Model;
using Bc.Model.Dto;
using Bc.Model.Dto.System;
using Microsoft.AspNetCore.Mvc;

namespace Bc.Admin.Controllers
{
    [WebAuthorizeAttribute]
    public class DBController : BaseController
    {
        private readonly IDbConfigService biz;
        private readonly IGroupTableService bizGT;
        private readonly IGroupInfoService bizGI;

        

        public DBController()
        {
            biz = new DbConfigService();
            bizGT = new GroupTableService();
            bizGI = new GroupInfoService();
        }
        public IActionResult config()
        {
            return View();
        }
        public IActionResult mydb()
        {
            return View();
        }


        public IActionResult group()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetList(DbConfigDto search)
        {
            var lst = biz.QueryPages(search);
            return toResponse(lst.TotalCount, "", lst);
        }
        [HttpGet]
        public ActionResult GroupTableList(GroupTableDto search)
        {
            var lst = bizGT.QueryPages(search);
            return toResponse(lst.TotalCount, "", lst);
        }
        [HttpGet]
        public ActionResult GroupInfoList(GroupInfoDto search)
        {
            var lst = bizGI.QueryPages(search);
            return toResponse(lst.TotalCount, "", lst);
        }

        #region 表信息
        [HttpGet]
        public ActionResult GetTableColumn(string configno, string database, string table)
        {
            var lst = biz.GetTableColumn(configno, database, table);
            return toResponse(1, "", lst);
        }
        [HttpGet]
        public ActionResult GetTableRows(string configno, string database = "", string tablename = "")
        {
            var lst = biz.GetTableRows(configno, database, tablename);
            return toResponse(1, "", JsonHelper.ToJson(lst));
        }
        [HttpGet]
        public ActionResult GetDatabaseList(string configno)
        {
            var lst = biz.GetDatabaseList(configno);
            return toResponse(1, "", lst);
        }
        #endregion

        #region 表信息

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveConfig([FromBody] DbConfig info)
        { 
            var r = biz.Saveable(info);
            return toResponse(1, "");
        }/// <summary>
         /// 取消配置
         /// </summary>
         /// <returns></returns>
        [HttpGet]
        public ActionResult CancelConfig([FromBody] DbConfig info)
        {
            var r = biz.Delete(info.ID);
            return toResponse(1, "");
        }



        /// <summary>
        /// 保存组的表信息
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveGroupTable([FromBody] GroupTable info)
        {
           var tt= bizGT.GetFirst(x => x.GroupNo == info.GroupNo && x.TableName == info.TableName);
            if(tt!=null) return toResponse(1, ""); 
            var r = bizGT.Saveable(info);
            return toResponse(1, "");
        }
        /// <summary>
        /// 移除组的表信息
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveGroupTable([FromBody] GroupTable info)
        {
            var tt = bizGT.GetFirst(x => x.GroupNo == info.GroupNo && x.TableName == info.TableName);
            if (tt != null)
            {
                bizGT.Delete(tt.ID);
                return toResponse(1, "");
            }
            return toResponse(0, "");
        }

        /// <summary>
        /// 保存组信息
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveGroupInfo([FromBody] GroupInfo info)
        {
            if (info.ID == 0) info.GroupNo = CaptchaUtil.GetRandomEnDigitalText(16);
            var r = bizGI.Saveable(info);
            return toResponse(1, "");
        }
        #endregion
    }
}
