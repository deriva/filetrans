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
    public partial class Enter : Form
    {
        public Enter()
        {
            InitializeComponent();
        }

        private void btnSocket_Click(object sender, EventArgs e)
        {

        }

        private void Enter_FormClosing(object sender, FormClosingEventArgs e)
        {


            var that = this;
            Thread thread1 = new Thread(() =>
            {
                if (that.InvokeRequired)
                    that.Invoke(new Action(() => { System.Environment.Exit(System.Environment.ExitCode); }));
                else System.Environment.Exit(System.Environment.ExitCode);
            });
            thread1.Start();


        }

        private void Enter_Load(object sender, EventArgs e)
        {

        }

        private void 文件接收器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fmName = "文件接收器";
            var frm = GetForm(fmName);
            if (frm == null || frm.IsDisposed)
            {
                frm = new Main();
                frm.MdiParent = this;
                frm.WindowState = FormWindowState.Maximized;
                frm.Show();
            }
            else
            {
                frm.Activate();
            }
   
        }

        private void 本地http服务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fmName = "本地http服务";
            var frm = GetForm(fmName);
            if (frm == null || frm.IsDisposed)
            {
                frm = new LocalServer();
                frm.WindowState = FormWindowState.Maximized;
                frm.MdiParent = this;
                frm.Show();
            }
            else
            {
                frm.Activate();
            }
   
        }

        private Form GetForm(string name)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (name == f.Name)
                {
                    return f;

                }
            }
            return null;
        }
    }
}
