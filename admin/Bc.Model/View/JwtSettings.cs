using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Model.View
{
    public class JwtSettings
    {
        ///使用者
        public string Issuer { get; set; }
        ///颁发者
        public string Audience { get; set; }
        ///秘钥必须大于16个字符
        public string SecretKey { get; set; }
    }

    public class JwtUserInfo
    {
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public int IsSupper { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        ///创建时间戳
        public long CreateTimeTimeStamp { get; set; }
        ///过期时间戳
        public long ExpirsTimeTimeStamp { get; set; }


    }
}
