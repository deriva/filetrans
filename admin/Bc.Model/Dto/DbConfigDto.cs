//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
//     author MEIAM
// </auto-generated>
//------------------------------------------------------------------------------
using System; 
using System.ComponentModel.DataAnnotations;

namespace Bc.Model.Dto
{
    public class DbConfigDto: PageParm
    {
                    /// <summary>
           /// 描述 : 数据库配置 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "数据库配置")]
           public  int ID 
           {get;set;}
           /// <summary>
           /// 描述 : 配置名称 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "配置名称")]
           public  string ConfigName 
           {get;set;}
           /// <summary>
           /// 描述 : 用户编号 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "用户编号")]
           public  string UserNo 
           {get;set;}
           /// <summary>
           /// 描述 : 配置信息 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "配置信息")]
           public  string ConfigInfo 
           {get;set;}
           /// <summary>
           /// 描述 : 配置编号 
           /// 空值 : False
           /// 默认 : 
           /// </summary>
           [Display(Name = "配置编号")]
           public  string ConfigNo 
           {get;set;}


    }
}