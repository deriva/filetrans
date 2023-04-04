
namespace Bc.PublishWF.V3._0
{
    partial class LocalServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labClear = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstMsg = new System.Windows.Forms.ListBox();
            this.btnEnable = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labClear
            // 
            this.labClear.AutoSize = true;
            this.labClear.Location = new System.Drawing.Point(757, 38);
            this.labClear.Name = "labClear";
            this.labClear.Size = new System.Drawing.Size(29, 12);
            this.labClear.TabIndex = 10;
            this.labClear.Text = "清空";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "消息:";
            // 
            // lstMsg
            // 
            this.lstMsg.BackColor = System.Drawing.Color.DarkGreen;
            this.lstMsg.ForeColor = System.Drawing.Color.White;
            this.lstMsg.FormattingEnabled = true;
            this.lstMsg.ItemHeight = 12;
            this.lstMsg.Location = new System.Drawing.Point(1, 56);
            this.lstMsg.Name = "lstMsg";
            this.lstMsg.Size = new System.Drawing.Size(799, 388);
            this.lstMsg.TabIndex = 8;
            this.lstMsg.SelectedIndexChanged += new System.EventHandler(this.lstMsg_SelectedIndexChanged);
            // 
            // btnEnable
            // 
            this.btnEnable.Location = new System.Drawing.Point(80, 7);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(148, 44);
            this.btnEnable.TabIndex = 6;
            this.btnEnable.Text = "启动服务";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // LocalServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.labClear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstMsg);
            this.Controls.Add(this.btnEnable);
            this.Name = "LocalServer";
            this.Text = "本地http服务";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LocalServer_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LocalServer_FormClosed);
            this.Load += new System.EventHandler(this.LocalServer_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstMsg;
        private System.Windows.Forms.Button btnEnable;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}