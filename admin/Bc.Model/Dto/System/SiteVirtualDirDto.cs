using System.ComponentModel.DataAnnotations;

namespace Bc.Model.Dto
{
    public class SiteVirtualDirDto : PageParm
    {/// <summary>
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
