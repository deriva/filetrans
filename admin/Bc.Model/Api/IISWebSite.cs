using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Model.Api
{
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
}
