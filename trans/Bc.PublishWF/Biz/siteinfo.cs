using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.PublishWF.Biz
{
   public class siteinfo
    {
        public int id { get; set; }
        public string sitename { get; set; }
        public string compressname { get; set; }
        public string sitedir { get; set; }
        public string targetdir { get; set; }
        public string targetbackupdir { get; set; }
        public string targetserver { get; set; }
        public string targetserver2 { get; set; }
        public string targetserver3 { get; set; }

        public string iscompress { get; set; }
    }
    public class excludedirinfo
    {
        public int id { get; set; }
        public string sitename { get; set; }
        public string excludedir { get; set; }
     
    }


    public class PageParm
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页总条数
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public string Sort { get; set; }

    }

    public class PublicFileDirDto : PageParm
    {
        /// <summary>
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

    }
    public class SitePublicExcludeDir
    {
        public SitePublicExcludeDir()
        {
        }

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

    }
    public class SiteInfo
    {
        public SiteInfo()
        {
        }

        /// <summary>
        /// 描述 : 站点信息 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
 
        public int Id { get; set; }

        /// <summary>
        /// 描述 : 站点信息 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "站点信息")]
        public string SiteName { get; set; }

        /// <summary>
        /// 描述 : 服务器 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "服务器")]
        public string ServerIP { get; set; }

        /// <summary>
        /// 描述 : 编号 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "编号")]
        public string No { get; set; }

        /// <summary>
        /// 描述 : 站点目录 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "站点目录")]
        public string SiteDir { get; set; }

        /// <summary>
        /// 描述 : 状态:1正常  0停止 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "状态:1正常  0停止")]
        public int? Status { get; set; }

        /// <summary>
        /// 描述 : 更新时间 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 描述 : 发布时间 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "发布时间")]
        public DateTime? PublicTime { get; set; }

        /// <summary>
        /// 描述 : 是否压缩 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "是否压缩")]
        public int? IsCompress { get; set; }

    }
}
