/*
* ==============================================================================
*
* FileName: RolePowersDto.cs
* Created: 2020/6/3 10:52:45
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
    public class Test007Dto
    {
        /// <summary>
        /// 描述 : 007日志请求 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "007日志请求")]
        public int Id { get; set; }

        /// <summary>
        /// 描述 : 请求的路径 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "请求的路径")]
        public string Url { get; set; }

        /// <summary>
        /// 描述 : 创建时间 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 描述 : 来源:0是自主调用1是正时调用 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "来源:0是自主调用1是正时调用")]
        public int Type { get; set; }

        /// <summary>
        /// 描述 : 分类：vin,分组，子组，oem 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "分类：vin,分组，子组，oem")]
        public int ClassType { get; set; }

        /// <summary>
        /// 描述 : 请求的结果：1成功0 失败 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "请求的结果：1成功0 失败")]
        public int Result { get; set; }

        /// <summary>
        /// 描述 : 后台请求人员 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "后台请求人员")]
        public int AdminId { get; set; }

        /// <summary>
        /// 描述 : 客户请求的 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "客户请求的")]
        public int MbId { get; set; }

        /// <summary>
        /// 描述 : 响应的结果 
        /// 空值 : True
        /// 默认 : 
        /// </summary>
        [Display(Name = "响应的结果")]
        public string Response { get; set; }
    }


    public class RoleUsersCreateDto
    {
        /// <summary>
        /// 描述 : 角色id 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "角色id")]
        [Required(ErrorMessage = "roleId 不能为空")]
        public string RoleId { get; set; }

        /// <summary>
        /// 描述 : 用户编码 [1,2,3,4] 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "用户编码 [1,2,3,4]")]
        public List<string> UserIds { get; set; }
    }

    public class RoleUsersDeleteDto
    {
        /// <summary>
        /// 描述 : 角色id 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "角色id")]
        [Required(ErrorMessage = "roleId 不能为空")]
        public string RoleId { get; set; }

        /// <summary>
        /// 描述 : 权限编码 [1,2,3,4] 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "权限编码 [1,2,3,4]")]
        public List<string> UserIds { get; set; }
    }
}
