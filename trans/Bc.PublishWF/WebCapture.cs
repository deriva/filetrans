using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bc.PublishWF
{
    public partial class WebCapture : Form
    {
        public WebCapture()
        {
            InitializeComponent();
        }
        WebBrowser webBrowser = null;
        public void ConvertToImg()
        {
            string html = @"
<style > tr th {
    border: 1px solid #ddd;
    font-size: 14px;
    color: #4f4f4f;
    line-height: 22px;
    padding: 8px;
    text-align: left;
}</style>
<table style='width:500px;'><tbody><tr><th>字段1</th><th>字段2</th><th>字段3</th></tr><tr><td>小明</td><td>22</td><td>185</td></tr><tr><td>小青</td><td>21</td><td>170</td></tr></tbody></table>";
            webBrowser = new WebBrowser();//是否显式滚动条

            webBrowser.ScrollBarsEnabled = false;//加载 html

            webBrowser.DocumentText = html;//页面加载完成执行事件

            webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);
            while (webBrowser.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();
            webBrowser.Dispose();
            var tt = "sasa";
        }
        private void webBrowser_DocumentCompleted(object sender, EventArgs e)//这个就是当网页载入完毕后要进行的操作

        {//获取解析后HTML的大小

            System.Drawing.Rectangle rectangle = webBrowser.Document.Body.ScrollRectangle; int width = rectangle.Width;
            int height = rectangle.Height;//设置解析后HTML的可视区域

            webBrowser.Width = width;

            webBrowser.Height = height;

            Bitmap bitmap = new System.Drawing.Bitmap(width, height);

            webBrowser.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, width, height));//设置图片文件保存路径和图片格式，格式可以自定义

            string filePath = AppDomain.CurrentDomain.BaseDirectory  + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";

            bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);//创建PDF

            

        }

        private void WebCapture_Load(object sender, EventArgs e)
        {
            ConvertToImg();
        }
    }
}
