
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
            this.btnLocalServer = new System.Windows.Forms.Button();
            this.btnSocket = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLocalServer
            // 
            this.btnLocalServer.Location = new System.Drawing.Point(55, 40);
            this.btnLocalServer.Name = "btnLocalServer";
            this.btnLocalServer.Size = new System.Drawing.Size(154, 51);
            this.btnLocalServer.TabIndex = 0;
            this.btnLocalServer.Text = "本地http服务";
            this.btnLocalServer.UseVisualStyleBackColor = true;
            this.btnLocalServer.Click += new System.EventHandler(this.btnLocalServer_Click);
            // 
            // btnSocket
            // 
            this.btnSocket.Location = new System.Drawing.Point(316, 40);
            this.btnSocket.Name = "btnSocket";
            this.btnSocket.Size = new System.Drawing.Size(201, 51);
            this.btnSocket.TabIndex = 1;
            this.btnSocket.Text = "文件接收服务";
            this.btnSocket.UseVisualStyleBackColor = true;
            this.btnSocket.Click += new System.EventHandler(this.btnSocket_Click);
            // 
            // Enter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSocket);
            this.Controls.Add(this.btnLocalServer);
            this.Name = "Enter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Devop服务入口";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Enter_FormClosing);
            this.Load += new System.EventHandler(this.Enter_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLocalServer;
        private System.Windows.Forms.Button btnSocket;
    }
}