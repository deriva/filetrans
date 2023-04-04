using Bc.Admin.Infrastructures;
using Bc.Bussiness;
using Bc.Bussiness.Service;
using Bc.Model;
using Bc.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;


namespace Bc.Admin.Controllers
{
    [WebAuthorizeAttribute.NoPermissionRequiredAttribute]
    public class ApiController : BaseController
    {
        private readonly ILogger<ApiController> _logger;
 
        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }
        // GET: ApiController
        public ActionResult Add(Sys_Logs ifno)
        {
            ISys_LogsService biz = new Sys_LogsService();
            var lst = biz.AddInfo(ifno);
            return Json(new Models.JsonPage() { code = 0 });
        }

        // GET: ApiController/Details/5
        [HttpGet]
        public ActionResult getdata(Sys_LogsDto search)
        {

            ISys_LogsService biz = new Sys_LogsService();
            var lst = biz.QueryPages(search);
            return Json(new Models.JsonPage() { code = 0, data = lst.DataSource, count = lst.TotalCount, msg = "" });
        }

        // GET: ApiController/Create
        public ActionResult List(Sys_LogsDto search)
        {
            return View(search);
        }

        // POST: ApiController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ApiController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ApiController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ApiController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ApiController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
