using Bc.Common.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Common.Helpers
{
    public class SmsHelper
    {/// <summary>
     /// url
     /// </summary>
        private static string url = "https://yun.tim.qq.com/v5/tlssmssvr/sendmultisms2";
        private static string appkey { get { return AppSettings.Configuration["TencentSms:APPKEY"]; } }
        private static string appId { get { return AppSettings.Configuration["TencentSms:APPID"]; } }
        public static int Send(string phoneNumbers, string templId, List<string> msg)
        {
            long random = new Random().Next(1, 100);
            long curTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var lst = new List<string>() { phoneNumbers };
            // 按照协议组织 post 请求包体
      
            // 按照协议组织 post 请求包体
            JObject data = new JObject();
            data.Add("tel", PhoneNumbersToJSONArray(lst));
            data.Add("type", 0);
            //data.Add("msg", msg);
            data.Add("tpl_id", templId);
            data.Add("params", SmsParamsToJSONArray(msg));
            data.Add("sign", "百川联合");
            data.Add("sig", CalculateSig(appkey, random, curTime, lst));
            data.Add("time", curTime);
            data.Add("extend", "");
            data.Add("ext", "");

            string wholeUrl = url + "?sdkappid=" + appId + "&random=" + random;

            var tt = HTTPHelpers.PostResponse(wholeUrl, JsonConvert.SerializeObject(data));
            LogHelper.Info(tt);
            var result = JObject.Parse(tt)["result"].ToString()=="0"?1:0;
            return result;
        }


        /// <summary>
        /// 发送工资条短信
        /// </summary>
        /// <param name="phoneNumbers"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int SendSalary(string phoneNumbers, string month,string code)
        {
            return Send(phoneNumbers, "860202", new List<string>() {month,code });
        }


        #region 工具方法
        public static string CalculateSig(string appkey, long random, long curTime, List<string> phoneNumbers)
        {
            string phoneNumbersString = phoneNumbers.ElementAt(0);
            for (int i = 1; i < phoneNumbers.Count; i++)
            {
                phoneNumbersString += "," + phoneNumbers.ElementAt(i);
            }
            return StrToHash(String.Format(
                    "appkey={0}&random={1}&time={2}&mobile={3}",
                    appkey, random, curTime, phoneNumbersString));
        }
        /// <summary>
        /// 字符串转SHA256
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StrToHash(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] resultByteArray = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            return ByteArrayToHex(resultByteArray);
        }
        /// <summary>
        /// 将二进制的数值转换为 16 进制字符串，如 "abc" => "616263"
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        private static string ByteArrayToHex(byte[] byteArray)
        {
            string returnStr = "";
            if (byteArray != null)
            {
                for (int i = 0; i < byteArray.Length; i++)
                {
                    returnStr += byteArray[i].ToString("x2");
                }
            }
            return returnStr;
        }
        /// <summary>
        /// List<string>转JArray
        /// </summary>
        /// <param name="templParams"></param>
        /// <returns></returns>
        public static JArray SmsParamsToJSONArray(List<string> templParams)
        {
            JArray smsParams = new JArray();
            foreach (string templParamsElement in templParams)
            {
                smsParams.Add(templParamsElement);
            }
            return smsParams;
        }

        /// <summary>
		/// PhoneNumbersToJSONArray
		/// </summary>
		/// <param name="nationCode"></param>
		/// <param name="phoneNumbers"></param>
		/// <returns></returns>
		public static JArray PhoneNumbersToJSONArray(  List<string> phoneNumbers)
        {
            JArray tel = new JArray();
            int i = 0;
            do
            {
                JObject telElement = new JObject();
                telElement.Add("nationcode", "86");
                telElement.Add("mobile", phoneNumbers.ElementAt(i));
                tel.Add(telElement);
            } while (++i < phoneNumbers.Count);
            return tel;
        }
        #endregion
    }
}
