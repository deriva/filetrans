/*
* ==============================================================================
*
* FileName: LogsDto.cs
* Created: 2020/6/11 8:53:35
* Author: Meiam
* Description: 
*
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bc.Model.Dto
{
    /// <summary>
    /// 查询日志
    /// </summary>
    public class Sys_LogsDto : PageParm
    {
        public int ID { get; set; }

        /// <summary>
        /// 描述 : 日志类型 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "日志类型")]
        public string Logger { get; set; }

        /// <summary>
        /// 描述 : 日志等级 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "日志等级")]
        public string Level { get; set; }

        /// <summary>
        /// 描述 : 日志来源 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "日志来源")]
        public string Url { get; set; }

        /// <summary>
        /// 描述 : 主机地址 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "主机地址")]
        public string Host { get; set; }

        /// <summary>
        /// 描述 : 请求方式 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "请求方式")]
        public string Method { get; set; }

        /// <summary>
        /// 描述 : 浏览器标识 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "浏览器标识")]
        public string UserAgent { get; set; }

        /// <summary>
        /// 描述 : Cookie 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "Cookie")]
        public string Cookie { get; set; }

        /// <summary>
        /// 描述 : URL参数 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "URL参数")]
        public string QueryString { get; set; }

        /// <summary>
        /// 描述 : 请求内容 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "请求内容")]
        public string Body { get; set; }

        /// <summary>
        /// 描述 : 日志信息 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "日志信息")]
        public string Message { get; set; }

        /// <summary>
        /// 描述 : 请求耗时 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "请求耗时")]
        public int? Elapsed { get; set; }

        /// <summary>
        /// 描述 : 创建时间 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }
        public DateTime? CreateTime1 { get; set; }
        /// <summary>
        /// 描述 : 用户来源 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "用户来源")]
        public string IPAddress { get; set; }
    }
}
