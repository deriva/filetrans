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
    [SugarTable("BrowserLog")]
    public class BrowserLog
    {
          public BrowserLog()
          {
          }

           /// <summary>
           /// 描述 : 访问记录 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "访问记录")]           
           [SugarColumn(IsPrimaryKey=true)]
           public int Id {get;set;}

           /// <summary>
           /// 描述 : 月份 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "月份")]           
           public string Month {get;set;}

           /// <summary>
           /// 描述 : 员工姓名 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "员工姓名")]           
           public string UserName {get;set;}

           /// <summary>
           /// 描述 : 编号 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "编号")]           
           public string UserNo {get;set;}

           /// <summary>
           /// 描述 : IP 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "IP")]           
           public string IP {get;set;}

           /// <summary>
           /// 描述 : 用户信息 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "用户信息")]           
           public string UserAgent {get;set;}

           /// <summary>
           /// 描述 : 创建时间 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "创建时间")]           
           public DateTime CreateTime {get;set;}

    }
}