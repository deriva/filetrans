using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.PublishWF.Model
{
    /// <summary>
    /// 发布的站点
    /// </summary>
   public class PublicSiteInfo
    {  /// <summary>
       /// 发布的站点
       /// </summary>
        public List<SiteInfo> LstSiteInfo { get; set; }

        /// <summary>
        /// 发布排除的目录
        /// </summary>
        public List<SitePublicExcludeDir> LstSitePublicExcludeDir { get; set; }

        /// <summary>
        /// 描述 : 发布文件目录 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "发布文件目录")]

        public int Id { get; set; }

        /// <summary>
        /// 描述 : 发布的路径 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "发布的路径")]
        public string Path { get; set; }

        /// <summary>
        /// 描述 : 文件类型:0目录 1文件 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "文件类型:0目录 1文件")]
        public int? FileType { get; set; }

        /// <summary>
        /// 描述 : 目标的站点 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "目标的站点")]
        public string DestNo { get; set; }
        public string CompressName { get; set; }

        public string SiteName { get; set; }

        public int Status { get; set; }
        /// <summary>
        /// 描述 : 发布目录编号 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "发布目录编号")]
        public string PublicNo { get; set; } 
        public string PublicDir { get; set; }
       // public List<PublicToServerInfo> LstPublicToServerInfo { get; set; }
    }
    /// <summary>
    /// 发布的站点to服务器
    /// </summary>
    public class PublicToServerInfo
    {
        public string ServIP { get; set; }

        public string IP { get; set; }
        public string Port { get; set; }

        public string SiteName { get; set; }

    }
    /// <summary>
    /// 排除目录
    /// </summary>
    public class SitePublicExcludeDir
    {
      
        /// <summary>
        /// 描述 : 站点发布排除的目录 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "站点发布排除的目录")]
      
        public int Id { get; set; }

        /// <summary>
        /// 描述 : 站点编号 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "站点编号")]
        public string No { get; set; }

        /// <summary>
        /// 描述 : 排除的目录 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "排除的目录")]
        public string Dir { get; set; }

        /// <summary>
        /// 描述 :  
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "")]
        public string PublicNo { get; set; }

    }
}
