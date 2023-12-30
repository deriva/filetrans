
namespace Bc.PublishWF.V3._0
{
    partial class Enter
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.本地http服务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文件接收器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.本地http服务ToolStripMenuItem,
            this.文件接收器ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1067, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 本地http服务ToolStripMenuItem
            // 
            this.本地http服务ToolStripMenuItem.Name = "本地http服务ToolStripMenuItem";
            this.本地http服务ToolStripMenuItem.Size = new System.Drawing.Size(114, 24);
            this.本地http服务ToolStripMenuItem.Text = "本地http服务";
            this.本地http服务ToolStripMenuItem.Click += new System.EventHandler(this.本地http服务ToolStripMenuItem_Click);
            // 
            // 文件接收器ToolStripMenuItem
            // 
            this.文件接收器ToolStripMenuItem.Name = "文件接收器ToolStripMenuItem";
            this.文件接收器ToolStripMenuItem.Size = new System.Drawing.Size(98, 24);
            this.文件接收器ToolStripMenuItem.Text = "文件接收器";
            this.文件接收器ToolStripMenuItem.Click += new System.EventHandler(this.文件接收器ToolStripMenuItem_Click);
            // 
            // Enter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 562);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Enter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Devop服务入口";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Enter_FormClosing);
            this.Load += new System.EventHandler(this.Enter_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 本地http服务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文件接收器ToolStripMenuItem;
    }
}