using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bc.PublishWF.V3._0
{
    public partial class TestFm : Form
    {
        public TestFm()
        {
            InitializeComponent();
        }

        private void btnContect_Click(object sender, EventArgs e)
        {

            Bc.PublishWF.Biz.V3.FileClientV3.Init(this.txtIP.Text,this.txtPort.Text);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Bc.PublishWF.Biz.V3.FileClientV3.SendMsg(txtMsg.Text);
        }
    }
}
