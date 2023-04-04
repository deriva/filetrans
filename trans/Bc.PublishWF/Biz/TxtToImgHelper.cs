using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bc.PublishWF.Biz
{
    public class TxtToImgHelper
    {
        WebBrowser webBrowser = null;
        private string _mbname = string.Empty;
        private string savefile = string.Empty;
        private string html2 = string.Empty;
        private bool isend = false;
        public string ConvertToImg(string html)
        {
            html2 = html;
                 isend = false;
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadHand));
            thread.TrySetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
            while (!isend)
            {
                if (isend) break;
                System.Threading.Thread.Sleep(100);
            }
            return savefile;
        }
        private void ThreadHand()
        {
            webBrowser = new WebBrowser();//是否显式滚动条 
            webBrowser.ScrollBarsEnabled = false;//加载 html 
            webBrowser.DocumentText = html2;//页面加载完成执行事件 
            webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);
            while (webBrowser.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();
            webBrowser.Dispose();
            isend = true;
        }

        private void webBrowser_DocumentCompleted(object sender, EventArgs e)//这个就是当网页载入完毕后要进行的操作

        {//获取解析后HTML的大小

            System.Drawing.Rectangle rectangle = webBrowser.Document.Body.ScrollRectangle; int width = rectangle.Width;
            int height = rectangle.Height;//设置解析后HTML的可视区域

            webBrowser.Width = width;

            webBrowser.Height = height;

            Bitmap bitmap = new System.Drawing.Bitmap(width, height);

            webBrowser.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, width, height));//设置图片文件保存路径和图片格式，格式可以自定义

            var mm = _mbname;
            var temp = AppDomain.CurrentDomain.BaseDirectory + "temp\\";
            if (!System.IO.Directory.Exists(temp)) System.IO.Directory.CreateDirectory(temp);
            var filename = _mbname + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
            string filePath = temp + filename;

            bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);// 
            savefile = filePath;



        }
    }
}
