using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Common.Configs
{
  
    public static class JwtConfig
    {
        /// <summary>
        /// 这里为了演示，写死一个密钥。实际生产环境可以从配置文件读取,这个是用网上工具随便生成的一个密钥
        /// </summary>
        public static string SecurityKey = AppSettings.Configuration["JwtSettings:SecretKey"];
        /// <summary>
        /// 站点地址
        /// </summary>
        public static string Domain = AppSettings.Configuration["JwtSettings:Domain"];

        /// <summary>
        /// 受理人，之所以弄成可变的是为了用接口动态更改这个值以模拟强制Token失效
        /// 真实业务场景可以在数据库或者redis存一个和用户id相关的值，生成token和验证token的时候获取到持久化的值去校验
        /// 如果重新登陆，则刷新这个值
        /// </summary>
        public static string ValidAudience;
    }
}
