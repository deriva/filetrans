
using Bc.Model;
using Bc.Model.Api;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Common.Helpers
{
    public class IISSiteHelper
    {
        public static int Create(IISWebSite webSite)
        {
            // var webSite = new WebSite();
            var r = 0;
            try
            {
                using (var sm = new ServerManager())
                {
                    //创建应用程序池
                    var appPool = sm.ApplicationPools.FirstOrDefault(ap => ap.Name.Equals(webSite.PoolName));

                    if (appPool == null)
                    {
                        r += CreateAppPool(sm.ApplicationPools, webSite.PoolName);
                    }

                    //创建Web站点
                    var site = sm.Sites.FirstOrDefault(s => s.Name.Equals(webSite.SiteName));

                    if (site == null)
                    {
                        r += CreateWebSite(sm.Sites, webSite, webSite.InstallPath);
                    }

                    sm.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToStr());
            }
            return r;
        }

        /// <summary>
        /// 启动/暂停/移除站点
        /// </summary>
        /// <param name="sitename"></param>
        /// <param name="poolname"></param>
        /// <param name="type">1启动 2停止 3移除</param>
        public static int EnableSite(string sitename, string poolname, int type)
        {
            var r = 0;
            try
            {
                using (var sm = new ServerManager())
                {
                    //创建应用程序池
                    var appPool = sm.ApplicationPools.FirstOrDefault(ap => ap.Name.Equals(poolname));

                    if (appPool != null)
                    {
                        if (type == 1)
                        {
                            appPool.Start();
                            Console.WriteLine("启动应用程序池:" + poolname);
                        }
                        else if (type == 2)
                        {
                            Console.WriteLine("停止应用程序池:" + poolname);
                            appPool.Stop();
                        }
                        else if (type == 3)
                        {
                            Console.WriteLine("移除应用程序池:" + poolname);
                            sm.ApplicationPools.Remove(appPool);
                        }
                        r += 1;
                    }

                    //创建Web站点
                    var site = sm.Sites.FirstOrDefault(s => s.Name.Equals(sitename));

                    if (site != null)
                    {
                        if (type == 1)
                        {
                            Console.WriteLine("启动站点:" + sitename);
                            site.Start();
                        }
                        else if (type == 2)
                        {
                            Console.WriteLine("停止站点:" + sitename);
                            site.Stop();
                        }
                        else if (type == 3)
                        {
                            Console.WriteLine("移除站点:" + sitename);
                            sm.Sites.Remove(site);
                        }
                        r += 1;
                    }
                 

                    sm.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToStr());
            }
            return r;
        }
        /// <summary>
        /// 创建虚拟目录
        /// </summary>
        /// <param name="sitename"></param>
        /// <param name="virtualname"></param>
        /// <param name="virtualpath"></param>
        /// <returns></returns>
        public static int CreateVirtual(string sitename, string virtualname, string virtualpath)
        {
            var r = 0;
            try
            {
                ServerManager serverManager = new ServerManager();
                var app = serverManager.Sites[sitename].Applications[0];
                app.VirtualDirectories.Add($"/{virtualname}", virtualpath);
                serverManager.CommitChanges();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToStr());
            }
            return r;

        }
        /// <summary>
        /// 创建应用程序池
        /// </summary>
        /// <param name="appPools"></param>
        /// <param name="appPoolName"></param>
        private static int CreateAppPool(ApplicationPoolCollection appPools, string appPoolName)
        {
            var r = 0;
            try
            {
                var appPool = appPools.Add(appPoolName);
                //   appPools.Remove(appPools.GetChildElement(appP);
                //是否自启动
                appPool.AutoStart = true;
                //队列长度
                appPool.QueueLength = 10000;
                //启动模式
                appPool.StartMode = StartMode.AlwaysRunning;
                //回收时间间隔
                appPool.Recycling.PeriodicRestart.Time = new TimeSpan();
                //闲置超时
                appPool.ProcessModel.IdleTimeout = new TimeSpan();
                //最大工作进程数
                appPool.ProcessModel.MaxProcesses = 1;
                appPool.ProcessModel.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;
                Console.WriteLine("创建应用程序池:" + appPoolName);
                r = 1;
            }
            catch (Exception ex) { LogHelper.Error(ex.ToStr()); }
            return r;
        }

        /// <summary>
        /// 创建Web站点
        /// </summary>
        /// <param name="sites"></param>
        /// <param name="webSite"></param>
        /// <param name="physicalPath"></param>
        private static int CreateWebSite(SiteCollection sites, IISWebSite webSite, string physicalPath)
        {
            var r = 0;
            try
            {
                Site site = null;
                bool isSiteCreated = false;

                foreach (var binding in webSite.Bindings)
                {
                    var bingdingInfo = ConstructBindingInfo(binding);

                    if (!isSiteCreated)
                    {
                        site = sites.Add(webSite.SiteName, binding.Protocol, bingdingInfo, physicalPath);

                        //是否自启动
                        site.ServerAutoStart = true;

                        isSiteCreated = true;
                    }
                    else
                    {
                        site.Bindings.Add(bingdingInfo, binding.Protocol);
                    }

                }

                var root = site.Applications["/"];

                //设置应用程序池
                root.ApplicationPoolName = webSite.PoolName;
                //设置虚拟目录
                //  root.VirtualDirectories["/"].PhysicalPath = pathToRoot;
                //预加载
                root.SetAttributeValue("preloadEnabled", true);
                Console.WriteLine("创建站点:" + webSite.PoolName);
            }
            catch (Exception ex) { LogHelper.Error(ex.ToStr()); }
            return r;
        }
        /// <summary>
        /// 构建绑定信息
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        private static string ConstructBindingInfo(WebSiteBinding binding)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(binding.ServerIP))
            {
                sb.Append(binding.ServerIP);
            }
            else
            {
                sb.Append("*");
            }

            sb.Append(":");

            sb.Append(binding.Port);

            sb.Append(":");

            if (!string.IsNullOrEmpty(binding.HostName))
            {
                sb.Append(binding.HostName);
            }
            else
            {
                sb.Append(string.Empty);
            }

            return sb.ToString();
        }
    }


    //public class WebSite
    //{
    //    public string PoolName { get; set; }
    //    public string InstallPath { get; set; }
    //    public string SiteName { get; set; }

    //    public List<WebSiteBinding> Bindings { get; set; }
    //}

    //public class WebSiteBinding
    //{
    //    public string IP { get; set; }
    //    public string HostName { get; set; }
    //    public string Port { get; set; }
    //    //http还是https
    //    public string BindingType { get; set; }
    //}
}
