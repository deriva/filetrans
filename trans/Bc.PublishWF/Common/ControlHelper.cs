using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bc.PublishWF.Common
{
    /// <summary>
    /// 控件封装
    /// </summary>
    public class ControlHelper
    {
        static ToolStripStatusLabel _labControl;
        static ListBox plstbox;
        static Label _lab;
        public static void InitListBox(ListBox listbox) { plstbox = listbox; }
        public static void InitListBox(Label lab) { _lab = lab; }
        public static void InitToolStripStatusLabel(ToolStripStatusLabel labControl) => _labControl = labControl;
        public static void AddMsg(string msg)
        {

            if (plstbox != null)
            {
                if (plstbox.InvokeRequired)
                {
                    Action<string> myAction = (p) => { AddMsg(p); };
                    plstbox.Invoke(myAction, msg);
                }
                else
                {
                    if (plstbox.Items.Count > 100)
                    {
                        plstbox.Items.Clear();
                    }
                    plstbox.Items.Insert(0, string.Format("{0}->{1}", DateTime.Now.ToStr("MM-dd HH:mm:ss"), msg));
                }
            }
        }
        public static void AddToolStripStatusLabel(string msg)
        {
            if (_labControl != null) _labControl.Text = msg;
        }
        public static void AddLabMsg(string msg)
        {

            if (_lab != null)
            {
                if (_lab.InvokeRequired)
                {
                    Action<string> myAction = (p) => { AddLabMsg(p); };
                }
                else
                {
                    _lab.Text = msg;
                }
            }
        }
    }
}
