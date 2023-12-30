using Bc.Admin.Infrastructures;
using Bc.Admin.Models;
using Bc.Bussiness;
using Bc.Bussiness.Service;
using Bc.Common.Helpers;
using Bc.Dao;
using Bc.Model;
using Bc.Model.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Bc.Admin.Controllers
{
    [WebAuthorizeAttribute]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Type[] types = Assembly
        .LoadFrom(AppDomain.CurrentDomain.BaseDirectory+  "\\Bc.Model.dll")//如果 .dll报错，可以换成 xxx.exe 有些生成的是exe 
        .GetTypes().Where(it => it.FullName.Contains("Bc.Model."))//命名空间过滤，当然你也可以写其他条件过滤
        .ToArray();//断点调试一下是不是需要的Type，不是需要的在进行过滤
            var tt = typeof(SqlSugar.SugarTable);
            types = types.Where(x => x.CustomAttributes.Any(y => y.AttributeType == tt)).ToArray();
            DbContext.Current.CodeFirst.SetStringDefaultLength(200).InitTables(types);//根据types创建表

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
