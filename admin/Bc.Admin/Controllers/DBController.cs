using Bc.Admin.Infrastructures;
using Bc.Bussiness.IService;
using Bc.Bussiness.Service;
using Bc.Common.Helpers;
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


        public DBController()
        {
            biz = new DbConfigService();
            bizGT = new GroupTableService();
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
    }
}
