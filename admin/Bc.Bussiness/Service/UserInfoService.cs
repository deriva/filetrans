//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
//     author MEIAM
// </auto-generated>
//------------------------------------------------------------------------------
using Bc.Common;
using Bc.Common.Configs;
using Bc.Common.Security;
using Bc.Common.Utilities;
using Bc.Model;
using Bc.Model.View;
using Microsoft.IdentityModel.Tokens;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Bc.Bussiness
{
    public class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {

        #region CustomInterface 

        /// <summary>
        /// 上传员工信息
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int UploadUserInfo(List<string> lst, ref string msg)
        {

            var result = 0;
            var key2 = AppSettings.Configuration["SALPASSKEY:Key"];
            lst.ForEach(x =>
            {
                var info = new UserInfo();
                var str = x.Split(',').ToList();
                info.UserName = str[1].Trim();
                if (info.UserName == "冯超")
                {
                    var s = 1;
                }
                info.DeptName = str[2];
                info.Duty = str[3];
                info.DutyType = str[4];//所属岗位
                var tt = GetFirst(x => x.UserName == info.UserName);
                if (tt != null)
                {
                    info.Id = tt.Id;
                }
                else
                {
                    info.IsSupper = 0;
                    info.CreateTime = DateTime.Now;
                }
                info.UserNo = "";
                info.Sex = str[5];
                info.MingZu = str[6];
                info.CodeID = str[7];
                info.BirthDay = str[8];
                info.Webbing = str[10];
                info.Mobile = str[11];
                info.Status = 1;

                info.UpdateTime = DateTime.Now;

                if (info.Id == 0)
                    result += Add(info);
                else
                    result += Update(info);

            });
            return result;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int Login(string username, string pwd, string scode, ref string msg, ref string code, ref JwtUserInfo jwtInfo)
        {
            var key2 = AppSettings.Configuration["SALPASSKEY:Key"];
            var info = GetFirst(x => x.UserName == username);
            if (info == null)
            {
                msg = "信息填写错误，暂无信息";
                return 0;
            }
            //if (info.Pwd != MD5Encode.MD5Encrypt(pwd))
            //{
            //    msg = "密码错误";
            //    return 0;
            //}
            var expirsDateTime = DateTime.Now.AddMinutes(30);
            //每次登陆动态刷新
            JwtConfig.ValidAudience = info.Id + "_" + DateTime.Now.AddMinutes(5).DateTimeToLongTimeStamp();
            //这里可以随意加入自定义的参数，key可以自己随便起
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(5)).ToUnixTimeSeconds()}"),
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim("UserId", info.Id+""),
                    new Claim("IsSupper", info.IsSupper+""),
                    new Claim("UserName", info.UserName+""),
                    new Claim("CreateTimeTimeStamp", DateTime.Now.DateTimeToLongTimeStamp()+""),
                    new Claim("ExpirsTimeTimeStamp", expirsDateTime.DateTimeToLongTimeStamp()+"")
                };
            //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                //颁发者
                issuer: JwtConfig.Domain,
                //接收者
                audience: JwtConfig.ValidAudience,
                //过期时间
                expires: expirsDateTime,
                //签名证书
                signingCredentials: creds,
                //自定义参数
                claims: claims
                );
            code = info.AuthCode = new JwtSecurityTokenHandler().WriteToken(token);

            var jwtInfo2 = new JwtUserInfo() { UserId = info.Id, UserName = info.UserName, ExpirsTimeTimeStamp = expirsDateTime.DateTimeToLongTimeStamp(), IsSupper = info.IsSupper };
            jwtInfo = jwtInfo2;
            //Update(info);
            //  code = info.AuthCode;
            return 1;
        }
        #endregion

    }
}
