using Bc.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bc.Admin.Infrastructures
{
    public class WebAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
   
            //var controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
 

            var authcode = string.Empty;
            if (!string.IsNullOrWhiteSpace(filterContext.HttpContext.Request.Cookies["authcode"]))
            {
                authcode = filterContext.HttpContext.Request.Cookies["authcode"].ToString();
            }
            if (string.IsNullOrWhiteSpace(authcode))
            {
                var query = filterContext.HttpContext.Request.Headers["authcode"];
                if (!string.IsNullOrWhiteSpace(query))
                {
                    authcode = query.ToString();
                }
            }
            if (string.IsNullOrWhiteSpace(authcode))
            {
                var query = filterContext.HttpContext.Request.Query["authcode"];
                if (!string.IsNullOrWhiteSpace(query))
                {
                    authcode = query.ToString();
                }
            }



            if (string.IsNullOrWhiteSpace(authcode))
            {
                var item = new ContentResult();
                item.Content = "没得权限";

                filterContext.Result = new RedirectResult("/Account/Login");
            }
            else
            {
                var pwd = authcode;
                string comkey = AppSettings.Configuration["LoginKey:CommonKey"];
                string superkey = AppSettings.Configuration["LoginKey:SuperKey"];
                if (pwd == comkey || pwd == superkey)
                {

                }
                else
                {
                    var item = new ContentResult();
                    item.Content = "没得权限";

                    filterContext.Result = new RedirectResult("/Account/Login");
                }
            }
            base.OnActionExecuting(filterContext);
        }

        public class NoPermissionRequiredAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                base.OnActionExecuting(filterContext);

            }

        }
    }

}
