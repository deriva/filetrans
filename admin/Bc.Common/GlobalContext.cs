using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Common
{
    public class GlobalContext
    {
        /// <summary>
        /// All registered service and class instance container. Which are used for dependency injection.
        /// </summary>
        public static IServiceCollection Services { get; set; }

        /// <summary>
        /// Configured service provider.
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }


        public static IConfiguration Configuration { get; set; }

       
        /// <summary>
        /// 时间类型默认值
        /// </summary>
        public static DateTime DefDateTime { get { return new DateTime(1970, 1, 1); } }

        public static string GetVersion()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            return version.Major + "." + version.Minor;
        }
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <returns></returns>
        public static HttpContext Context
        {
            get
            {
                var hca = ServiceProvider.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));
                HttpContext context = ((IHttpContextAccessor)hca).HttpContext;
                return context;
            }
        }
        /// <summary>
        /// 检测是否是异步
        /// </summary>
        /// <returns></returns>
        public static bool IsAjaxRequest()
        {
            return Context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
        /// <summary>
        /// 程序启动时，记录目录
        /// </summary>
        /// <param name="env"></param>
        //public static void LogWhenStart(IWebHostEnvironment env)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("程序启动");
        //    //sb.Append(" |ContentRootPath:" + env.ContentRootPath);
        //    //sb.Append(" |WebRootPath:" + env.WebRootPath);
        //    //sb.Append(" |IsDevelopment:" + env.IsDevelopment());
        // //   LogHelper.Debug(sb.ToString());
        //}

       
    }
    public static class ServiceProviderServiceExtensions
    {
        public static T GetService<T>(this IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }

        public static T GetRequiredService<T>(this IServiceProvider provider)
        {
            return (T)provider.GetRequiredService(typeof(T));
        }

    }
}
