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
    [SugarTable("PublicFileDir")]
    public class PublicFileDir
    {
          public PublicFileDir()
          {
          }

           /// <summary>
           /// 描述 : 发布文件目录 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "发布文件目录")]           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int Id {get;set;}

           /// <summary>
           /// 描述 : 发布的路径 
           /// 空值 : True
           /// 默认 : 
           /// </summary>
           [Display(Name = "发布的路径")]           
           public string Path {get;set;}

           /// <summary>
           /// 描述 : 文件类型:0目录 1文件 
           /// 空值 : True
           /// 默认 : 
           /// </summary>
           [Display(Name = "文件类型:0目录 1文件")]           
           public int? FileType {get;set;}

   
           /// <summary>
           /// 描述 : 压缩文件名 
           /// 空值 : True
           /// 默认 : 
           /// </summary>
           [Display(Name = "压缩文件名")]           
           public string CompressName {get;set;}

           /// <summary>
           /// 描述 : 状态 
           /// 空值 : True
           /// 默认 : 
           /// </summary>
           [Display(Name = "状态")]           
           public int? Status {get;set;}

           /// <summary>
           /// 描述 : 站点名称 
           /// 空值 : True
           /// 默认 : 
           /// </summary>
           [Display(Name = "站点名称")]           
           public string SiteName {get;set;}

           /// <summary>
           /// 描述 :  
           /// 空值 : True
           /// 默认 : 
           /// </summary>
           [Display(Name = "")]           
           public string PublicNo {get;set;}

    }
}