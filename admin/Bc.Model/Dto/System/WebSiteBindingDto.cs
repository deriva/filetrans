using System.ComponentModel.DataAnnotations;

namespace Bc.Model.Dto.System
{
    public class WebSiteBindingDto: PageParm
    { /// <summary>
      /// 描述 : 站点绑定 
      /// 空值 : False
      /// 默认 : 
      /// </summary>
        [Display(Name = "站点绑定")] 
        public int Id { get; set; }

        /// <summary>
        /// 描述 : 编号 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "编号")]
        public string No { get; set; }

        /// <summary>
        /// 描述 : 端口 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "端口")]
        public string Port { get; set; }

        /// <summary>
        /// 描述 : 服务器 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "服务器")]
        public string ServerIP { get; set; }

        /// <summary>
        /// 描述 : 协议:http/https 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "协议:http/https")]
        public string Protocol { get; set; }

        /// <summary>
        /// 描述 : 主机名 
        /// 空值 : False
        /// 默认 : 
        /// </summary>
        [Display(Name = "主机名")]
        public string HostName { get; set; }

    }
}
