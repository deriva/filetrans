using Bc.PublishWF.Common;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Bc.PublishWF.Biz
{
    public class HttpSiteHelper
    {
        private static string root = ConfigUtils.GetValue<string>("DataApiSite");
        private static string token { get { return ConfigUtils.GetValue<string>("DataApiSiteSecret"); } }

        #region http请求
        /// <summary>
        /// 处理GET请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="getParam">请求参数</param>
        /// <returns>流信息</returns>
        public static string DoGet(string url, string getParam, int timeout = 500000)
        {
            getParam += "&authcode=" + token;

            var path = root + url;
            if (url.Contains("http")) path = url;
            if (path.IndexOf("?") == -1) path += "?";
            try
            {
                System.GC.Collect();
                System.Net.ServicePointManager.DefaultConnectionLimit = 500;
                if (!string.IsNullOrWhiteSpace(getParam))
                { url = path+ getParam; }
                var req = WebRequest.Create(url) as HttpWebRequest;
                req.Method = "Get";
                if (!string.IsNullOrWhiteSpace(token))
                    req.Headers.Add("authcode", token);
                req.KeepAlive = false;
                req.Timeout = timeout;
               var tt= req.RequestUri.Port;
                using (var response = (HttpWebResponse)req.GetResponse())
                {
                    var stream = response.GetResponseStream();
                    if (stream == null)
                        return string.Empty;
                    var reader = new StreamReader(stream);
                    var data = reader.ReadToEnd();
                    response.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                // LogHelper.Error(ex.ToString());
            }
            return "";
        }
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // 总是接受  
            return true;
        }
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postParam">请求参数</param>
        /// <param name="contentType"></param>
        /// <returns>流信息</returns>
        public static string DoPost(string url, string postParam, string encode = "utf-8", string contentType = "application/json", int timeout = 100000)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                var param = Encoding.GetEncoding(encode).GetBytes(postParam);
                HttpWebRequest req = null;
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                    req = WebRequest.Create(url) as HttpWebRequest;
                    req.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    req = WebRequest.Create(url) as HttpWebRequest;
                }
                if (!string.IsNullOrWhiteSpace(token))
                    req.Headers.Add("authcode", token);
                req.Method = "POST";
                var tt = req.RequestUri.Port;
                req.ContentType = contentType;
                req.ContentLength = param.Length;
                req.KeepAlive = false;
                req.Timeout = timeout;
                req.ProtocolVersion = HttpVersion.Version10;
                using (var reqstream = req.GetRequestStream())
                {
                    reqstream.Write(param, 0, param.Length);
                }
                using (var response = (HttpWebResponse)req.GetResponse())
                {
                    var stream = response.GetResponseStream();
                    if (stream == null)
                        return string.Empty;
                    var reader = new StreamReader(stream);
                    var data = reader.ReadToEnd();
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString() + ",url:" + url + "parm：" + postParam);
            }
            return "";
        }
        #endregion


        #region 业务请求
        public static string GetIp()
        {
            var tt = DoGet("http://pv.sohu.com/cityjson?ie=utf-8", "");
           var str= tt.Replace("var returnCitySN = ", "").Replace(";", "");
            return ((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(str))["cip"].ToStr();

        }


        #region 上报请求
        /// <summary>
        /// 上报请求连接
        /// </summary>
        public static void ReportConection()
        {
            try
            {
                var sppServer = ConfigUtils.GetValue<string>("AppServer");
                var serverip = ConfigUtils.GetValue<string>("serverip").Split(':');
                var myProt = serverip[1].ToInt2();
                var no = sppServer.Split('|')[0];
                var servername = sppServer.Split('|')[1];
                var env = sppServer.Split('|')[2];
                var obj = new JObject();
                obj["No"] = no;
                obj["Id"] = 0;
                obj["ServerName"] = servername;
                obj["IP"] = GetIp();
                obj["Port"] = myProt + "";
                obj["Env"] = env;
                obj["UpdateTime"] = "2022-01-01 14:52:20";
                DoPost(root + "/ServerApp/save", obj.ToStr());
            }
            catch { }

        }

        /// <summary>
        /// 上报断开连接
        /// </summary>
        public static void ReportDisConection()
        {
            try
            {
                var serverip = ConfigUtils.GetValue<string>("serverip").Split(':');
            var myProt = serverip[1].ToInt2();
            var ip = GetIp();
           var port = myProt; 
            DoPost(root + "/ServerApp/Delete?ip="+ ip+"&port="+port, "");
            }
            catch { }

        }
        #endregion


        #region 站点信息
        /// <summary>
        /// 获取获取发布信息的目前/排除目录/站点信息
        /// </summary>
        /// <param name="siteno"></param>
        /// <returns></returns>
        public static string ApiGetPublicDir(string sitename)
        {
           return DoGet("/PublicFileDir/ApiGetPublicDir?sitename=" + sitename, "");
        }

        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="siteno"></param>
        /// <returns></returns>
        public static string GetIISWebSite(string siteno)
        {
            return DoGet("/site/GetIISWebSite?no=" + siteno, "");
        }


        #endregion



        #endregion
    }
}
