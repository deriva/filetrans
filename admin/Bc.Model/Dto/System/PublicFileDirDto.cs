﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
//     author MEIAM
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using System.Text;
using SqlSugar;
using System.Collections.Generic;

namespace Bc.Model
{

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
        /// <summary>
        /// 描述 : 发布目录编号 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "发布目录编号")]
        public string PublicNo { get; set; }
     
        
    }
}