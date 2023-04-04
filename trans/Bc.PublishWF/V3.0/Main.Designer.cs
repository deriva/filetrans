
namespace Bc.PublishWF.V3._0
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btnEnable = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lstMsg = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labClear = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEnable
            // 
            this.btnEnable.Location = new System.Drawing.Point(82, 12);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(148, 44);
            this.btnEnable.TabIndex = 0;
            this.btnEnable.Text = "启动Socket";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(300, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(148, 44);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "停止Socket";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lstMsg
            // 
            this.lstMsg.BackColor = System.Drawing.Color.DarkGreen;
            this.lstMsg.ForeColor = System.Drawing.Color.White;
            this.lstMsg.FormattingEnabled = true;
            this.lstMsg.ItemHeight = 12;
            this.lstMsg.Location = new System.Drawing.Point(3, 61);
            this.lstMsg.Name = "lstMsg";
            this.lstMsg.Size = new System.Drawing.Size(799, 364);
            this.lstMsg.TabIndex = 2;
            this.lstMsg.SelectedIndexChanged += new System.EventHandler(this.lstMsg_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "消息:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // labClear
            // 
            this.labClear.AutoSize = true;
            this.labClear.Location = new System.Drawing.Point(759, 43);
            this.labClear.Name = "labClear";
            this.labClear.Size = new System.Drawing.Size(29, 12);
            this.labClear.TabIndex = 4;
            this.labClear.Text = "清空";
            this.labClear.Click += new System.EventHandler(this.labClear_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(490, 32);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 5;
            this.btnTest.Text = "模拟连接";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel1.Text = "进度";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.labClear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstMsg);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnEnable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "站点文件接收器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEnable;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ListBox lstMsg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labClear;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}