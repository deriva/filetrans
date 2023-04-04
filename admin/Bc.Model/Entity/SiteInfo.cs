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


namespace Bc.Model
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("SiteInfo")]
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
        [Display(Name = "站点信息")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
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

        /// <summary>
        /// 描述 : 端口 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "端口")]
        public string Port { get; set; }

        /// <summary>
        /// 描述 : 协议:http/https 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "协议:http/https")]
        public string Protocol { get; set; }

        /// <summary>
        /// 描述 : 生成环境:1生成环境 2测试环境 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "生成环境:1生成环境 2测试环境")]
        public int? Env { get; set; }

        /// <summary>
        /// 版本:,v2.0,v4.0
        /// </summary>
        public string CLRVeersion { get; set; }

    }
}