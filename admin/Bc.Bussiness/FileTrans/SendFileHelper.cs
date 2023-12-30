using Bc.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Bc.Bussiness.FileTrans
{
    public class SendFileHelper
    {
        public static int SendFile(PublicFileDirDto info,  ref string errmsg)
        {
            var r = 0;
            //var obj = JObject.Parse(msg.ToStr());
            //var publicno = obj["no"];
            //var env = obj["env"].ToStr();
            var dir =  (AppDomain.CurrentDomain.BaseDirectory + @"\Resources\");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            //var dt = HttpSiteHelper.DoGet("/PublicFileDir/GetPublicFileDirByNoV2", $"no={publicno}&env={env}");
            //var re = JObject.Parse(dt)["attr"].ToStr();
            //var info = JsonConvert.DeserializeObject<PublicFileDirDto>(re);
            if (info.LstSiteInfo.Count == 0)
            {
                errmsg = "该站点未配置目标服务器";
                return 0;
            }
            var compressfilename = info.CompressName;//压缩包文件名
            var compressdir = info.Path;//压缩路径
            if (info.LstSitePublicExcludeDir != null && info.LstSitePublicExcludeDir.Count > 0)
            {
                ///清空排除的目录和文件
                info.LstSitePublicExcludeDir.ForEach(x =>
                {
                    if (File.Exists(x.Dir))
                        File.Delete(x.Dir);
                    if (Directory.Exists(x.Dir))
                        Directory.Delete(x.Dir, true);
                });
            }


            if (info.LstSiteInfo != null && info.LstSiteInfo.Count > 0)
            {
                info.LstSiteInfo.ForEach(x =>
                {
                    var serverip = x.ServerIP.Split(':');
                    var flag = FileClientV3.Init(serverip[0], serverip[1]);
                    if (flag)
                    {
                        var ff = FileClientV3.SendFileV2(x.No, info.Path);
                        r += ff ? 1 : 0;
                    }
                });

            }
            return r;

        }
    }
}
