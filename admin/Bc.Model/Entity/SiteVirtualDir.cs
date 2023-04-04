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
    [SugarTable("SiteVirtualDir")]
    public class SiteVirtualDir
    {
          public SiteVirtualDir()
          {
          }

           /// <summary>
           /// 描述 : 站点的虚拟目录 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "站点的虚拟目录")]           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int Id {get;set;}

           /// <summary>
           /// 描述 : 站点名称 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "站点名称")]           
           public string SiteName {get;set;}

           /// <summary>
           /// 描述 : 站点编号 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "站点编号")]           
           public string SiteNo {get;set;}

           /// <summary>
           /// 描述 : 虚拟目录名称 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "虚拟目录名称")]           
           public string VirtualName {get;set;}

           /// <summary>
           /// 描述 : 虚拟目录路径 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "虚拟目录路径")]           
           public string VirtualDir {get;set;}

    }
}