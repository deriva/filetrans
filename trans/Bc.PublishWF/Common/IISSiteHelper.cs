using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.PublishWF.Common
{
    public class IISSiteHelper
    {
        /// <summary>
        /// 创建站点
        /// </summary>
        /// <param name="webSite"></param>
        /// <returns></returns>
        public static int Create(IISWebSite webSite)
        {
            var r = 0;
            try
            {
                using (var sm = new ServerManager())
                {
                    //创建应用程序池
                    var appPool = sm.ApplicationPools.FirstOrDefault(ap => ap.Name.Equals(webSite.PoolName));

                    if (appPool == null)
                    {
                        r += CreateAppPool(sm.ApplicationPools, webSite.PoolName, webSite.CLRVeersion);
                    }

                    //创建Web站点
                    var site = sm.Sites.FirstOrDefault(s => s.Name.Equals(webSite.SiteName));
                    if (site == null)
                    {
                        r += CreateWebSite(sm.Sites, webSite, webSite.InstallPath);
                    }

                    sm.CommitChanges();
                }
                //创建虚拟目录
                CreateVirtual(webSite.SiteName, webSite.SiteVirtualDirs);
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

            try
            {
               
                Task.Factory.StartNew(() =>
                {
                    ObjectState objectState;
                    var r = 0;
                    using (var sm = new ServerManager(@"C:\Windows\System32\inetsrv\config\applicationHost.config"))
                    {
                        //创建应用程序池
                        var appPool = sm.ApplicationPools.FirstOrDefault(ap => ap.Name.Equals(poolname));
                        if (appPool != null)
                        {
                            if (type == 1)
                            {
                                objectState= appPool.Start();
                                ControlHelper.AddMsg(poolname+" 启动应用程序池:" + objectState.ToStr()   );
                            }
                            else if (type == 2)
                            {
         
                                objectState = appPool.Stop();
                                ControlHelper.AddMsg(poolname + " 停止应用程序池:" + objectState.ToStr() );
                            }
                            else if (type == 3)
                            {
                                ControlHelper.AddMsg(poolname + " 移除应用程序池:" );
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
                                objectState = site.Start();
                                ControlHelper.AddMsg(sitename+"启动站点:" + objectState.ToStr() );
                          
                            }
                            else if (type == 2)
                            {
                               objectState = site.Stop();
                                ControlHelper.AddMsg(sitename + "停止站点:" +objectState.ToStr() );

                            }
                            else if (type == 3)
                            {
                                ControlHelper.AddMsg(sitename + "移除站点:" );
                                sm.Sites.Remove(site);
                            }
                            r += 1;
                        }
                        else
                        {
                            LogHelper.Info($"{string.Join(",", sm.Sites.Select(x => x.Name).ToList())}");
                            ControlHelper.AddMsg($"站点:{sitename}不存在");
                        }

                        if (r > 0)
                        {
                            sm.CommitChanges();
                        }
                    }

                });

            }
            catch (Exception ex)
            {
                LogHelper.Error("EnableSite:" + ex.ToStr());
                ControlHelper.AddMsg($"站点:{sitename},操作:{type}(1启动 2停止 3移除),异常:{ex.Message}");
                return 0;
            }
            return 1;
        }
        /// <summary>
        /// 创建虚拟目录
        /// </summary>
        /// <param name="sitename"></param>
        /// <param name="virtualname"></param>
        /// <param name="virtualpath"></param>
        /// <returns></returns>
        public static int CreateVirtual(string sitename, List<SiteVirtualDir> lst)
        {
            var r = 0;
            try
            {
                ServerManager serverManager = new ServerManager();
                var app = serverManager.Sites[sitename].Applications[0];
                lst.ForEach(x =>
                {
                    app.VirtualDirectories.Add($"/{x.VirtualName}", x.VirtualDir);
                });
                serverManager.CommitChanges();
                r = 1;
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
        private static int CreateAppPool(ApplicationPoolCollection appPools, string appPoolName, string managedRuntimeVersion = null)
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
                appPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;

                if (managedRuntimeVersion != null)//"v2.0,v4.0";
                    appPool.ManagedRuntimeVersion = managedRuntimeVersion;
                else appPool.ManagedRuntimeVersion = "";
                //启动模式
                appPool.StartMode = StartMode.AlwaysRunning;
                //回收时间间隔
                appPool.Recycling.PeriodicRestart.Time = new TimeSpan();
                //闲置超时
                appPool.ProcessModel.IdleTimeout = new TimeSpan();
                //最大工作进程数
                appPool.ProcessModel.MaxProcesses = 1;
                appPool.ProcessModel.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;
                ControlHelper.AddMsg("创建应用程序池:" + appPoolName);
                r = 1;
            }
            catch (Exception ex) { LogHelper.Error("CreateAppPool：" + ex.ToStr()); }
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
            var siteName = webSite.SiteName;
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
                ControlHelper.AddMsg("创建站点:" + webSite.PoolName);
            }
            catch (Exception ex)
            {
                LogHelper.Error("CreateWebSite：" + ex.ToStr());

                ControlHelper.AddMsg($"siteName:{ex.ToStr()}");
            }
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

            sb.Append($":{binding.Port}:");
            return sb.ToString();
        }
    }

    /// <summary>
    /// 站点
    /// </summary>
    public class IISWebSite
    {
        public string PoolName { get; set; }
        public string InstallPath { get; set; }
        public string SiteName { get; set; }

        /// <summary>
        /// 版本:,v2.0,v4.0
        /// </summary>
        public string CLRVeersion { get; set; }

        public List<WebSiteBinding> Bindings { get; set; }

        public List<SiteVirtualDir> SiteVirtualDirs { get; set; }
    }
    /// <summary>
    /// 站点IP绑定
    /// </summary>
    public class WebSiteBinding
    {
        public string ServerIP { get; set; }
        public string HostName { get; set; }
        public string Port { get; set; }
        //http还是https
        public string Protocol { get; set; }
    }
    /// <summary>
    /// 站点虚拟目录
    /// </summary>
    public class SiteVirtualDir
    {

        /// <summary>
        /// 描述 : 站点的虚拟目录 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "站点的虚拟目录")]
        public int Id { get; set; }

        /// <summary>
        /// 描述 : 站点名称 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "站点名称")]
        public string SiteName { get; set; }

        /// <summary>
        /// 描述 : 站点编号 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "站点编号")]
        public string SiteNo { get; set; }

        /// <summary>
        /// 描述 : 虚拟目录名称 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "虚拟目录名称")]
        public string VirtualName { get; set; }

        /// <summary>
        /// 描述 : 虚拟目录路径 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "虚拟目录路径")]
        public string VirtualDir { get; set; }

    }
}
