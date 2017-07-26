namespace Crawler
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.CurrentIP = new System.Windows.Forms.Label();
            this.btnFindIP = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前ip";
            // 
            // CurrentIP
            // 
            this.CurrentIP.AutoSize = true;
            this.CurrentIP.Location = new System.Drawing.Point(112, 30);
            this.CurrentIP.Name = "CurrentIP";
            this.CurrentIP.Size = new System.Drawing.Size(41, 12);
            this.CurrentIP.TabIndex = 1;
            this.CurrentIP.Text = "当前ip";
            // 
            // btnFindIP
            // 
            this.btnFindIP.Location = new System.Drawing.Point(44, 99);
            this.btnFindIP.Name = "btnFindIP";
            this.btnFindIP.Size = new System.Drawing.Size(75, 23);
            this.btnFindIP.TabIndex = 2;
            this.btnFindIP.Text = "查看ip";
            this.btnFindIP.UseVisualStyleBackColor = true;
            this.btnFindIP.Click += new System.EventHandler(this.btnFindIP_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 384);
            this.Controls.Add(this.btnFindIP);
            this.Controls.Add(this.CurrentIP);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label CurrentIP;
        private System.Windows.Forms.Button btnFindIP;
    }
}

