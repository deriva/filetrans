using System;
using System.ComponentModel.DataAnnotations;

namespace Bc.Model.Dto.System
{
    public class ServerAppDto : PageParm
    {   /// <summary>
        /// 描述 : 发布文件目录 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "发布文件目录")]
        public int Id { get; set; }

        /// <summary>
        /// 描述 : 站点编号 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "站点编号")]
        public string No { get; set; }

        /// <summary>
        /// 描述 :  
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "")]
        public string ServerName { get; set; }

        /// <summary>
        /// 描述 :  
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "")]
        public string IP { get; set; }

        /// <summary>
        /// 描述 :  
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "")]
        public string Port { get; set; }

        /// <summary>
        /// 描述 : 1生产端 2测试端 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "1生产端 2测试端")]
        public int Env { get; set; }

        /// <summary>
        /// 描述 : 更新实际 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "更新实际")]
        public DateTime UpdateTime { get; set; }
    }
}
