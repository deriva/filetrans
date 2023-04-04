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
    [SugarTable("SitePublicExcludeDir")]
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
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int Id {get;set;}

           /// <summary>
           /// 描述 : 站点编号 
           /// 空值 : True
           /// 默认 : 
           /// </summary>
           [Display(Name = "站点编号")]           
           public string No {get;set;}

           /// <summary>
           /// 描述 : 排除的目录 
           /// 空值 : True
           /// 默认 : 
           /// </summary>
           [Display(Name = "排除的目录")]           
           public string Dir {get;set;}

           /// <summary>
           /// 描述 :  
           /// 空值 : True
           /// 默认 : 
           /// </summary>
           [Display(Name = "")]           
           public string PublicNo {get;set;}

    }
}