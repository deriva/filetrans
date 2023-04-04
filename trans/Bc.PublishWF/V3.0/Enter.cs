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
    public partial class Enter : Form
    {
        public Enter()
        {
            InitializeComponent();
        }

        private void btnLocalServer_Click(object sender, EventArgs e)
        {
            new LocalServer().Show(); 
        }

        private void btnSocket_Click(object sender, EventArgs e)
        {
            new Main().Show(); 
        }

        private void Enter_FormClosing(object sender, FormClosingEventArgs e)
        {
            Task.Factory.StartNew(() => { 
                System.Environment.Exit(System.Environment.ExitCode); 
            }); 
        }

        private void Enter_Load(object sender, EventArgs e)
        {

        }
    }
}
