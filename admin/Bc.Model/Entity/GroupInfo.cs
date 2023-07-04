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
    [SugarTable("GroupInfo")]
    public class GroupInfo
    {
          public GroupInfo()
          {
          }

           /// <summary>
           /// 描述 : 组信息 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "组信息")]           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int ID {get;set;}

           /// <summary>
           /// 描述 : 组编号 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "组编号")]           
           public string GroupNo {get;set;}

           /// <summary>
           /// 描述 : 组名 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "组名")]           
           public string GroupName {get;set;}

           /// <summary>
           /// 描述 : 用户 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "用户")]           
           public string UserNo {get;set;}

           /// <summary>
           /// 描述 : 配置编号 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "配置编号")]           
           public string ConfigNo {get;set;}

    }
}