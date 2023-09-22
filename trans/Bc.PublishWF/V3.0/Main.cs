using Bc.PublishWF.Biz;
using Bc.PublishWF.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bc.PublishWF.V3._0
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnable_Click(object sender, EventArgs e)
        {
            Bc.PublishWF.Biz.V3.FileServerV3.Init();
            btnEnable.Enabled = false;
            btnStop.Enabled = true;
        }
        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            Bc.PublishWF.Biz.V3.FileServerV3.Exit();
            btnStop.Enabled = false;
            btnEnable.Enabled = true;
        }

        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="msg"></param>
        public void AddMsg(string msg)
        {

            if (lstMsg.InvokeRequired)
            {
                Action<string> myAction = (p) => { AddMsg(p); };
                lstMsg.Invoke(myAction, msg);
            }
            else
            {
                if (lstMsg.Items.Count > 50)
                {
                    lstMsg.Items.Clear();
                }
                lstMsg.Items.Insert(0, string.Format("{0}->{1}", DateTime.Now.ToStr("yy-MM-dd HH:mm:ss"), msg));
            }

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            new TestFm().Show();
            //   this.Hide();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ControlHelper.InitListBox(lstMsg);
            ControlHelper.InitToolStripStatusLabel(toolStripStatusLabel1);
        }

        private void lstMsg_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void labClear_Click(object sender, EventArgs e)
        {

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            var that = this;
            Thread thread1 = new Thread(() =>
            {
                try
                {
                    if (that.InvokeRequired)
                        that.Invoke(new Action(() =>
                        {

                            if (!that.IsDisposed) that.Close();
                        }));
                    else that.Close();
                }
                catch { }
            });
            thread1.Start();

        }
    }
}
