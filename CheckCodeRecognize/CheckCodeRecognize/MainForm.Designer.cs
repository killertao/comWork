namespace CheckCodeRecognize
{
    partial class MainForm
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSetFZ = new System.Windows.Forms.Button();
            this.picbox = new System.Windows.Forms.PictureBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.txtFZ = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEZ = new System.Windows.Forms.Button();
            this.txtEZ = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBJFZ = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDropBG = new System.Windows.Forms.Button();
            this.btnDropDisturb = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.pbNormal = new System.Windows.Forms.PictureBox();
            this.pbSplit1 = new System.Windows.Forms.PictureBox();
            this.pbSplit2 = new System.Windows.Forms.PictureBox();
            this.pbSplit3 = new System.Windows.Forms.PictureBox();
            this.pbSplit4 = new System.Windows.Forms.PictureBox();
            this.pbSplit5 = new System.Windows.Forms.PictureBox();
            this.pbSplit6 = new System.Windows.Forms.PictureBox();
            this.btnSplit = new System.Windows.Forms.Button();
            this.panelCutPic = new System.Windows.Forms.Panel();
            this.txtC1 = new System.Windows.Forms.TextBox();
            this.txtC2 = new System.Windows.Forms.TextBox();
            this.txtC3 = new System.Windows.Forms.TextBox();
            this.txtC4 = new System.Windows.Forms.TextBox();
            this.txtC5 = new System.Windows.Forms.TextBox();
            this.txtC6 = new System.Windows.Forms.TextBox();
            this.btnStudy = new System.Windows.Forms.Button();
            this.btnRecognize = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.picbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNormal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSplit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSplit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSplit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSplit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSplit5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSplit6)).BeginInit();
            this.panelCutPic.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSetFZ
            // 
            this.btnSetFZ.Location = new System.Drawing.Point(174, 281);
            this.btnSetFZ.Name = "btnSetFZ";
            this.btnSetFZ.Size = new System.Drawing.Size(75, 23);
            this.btnSetFZ.TabIndex = 0;
            this.btnSetFZ.Text = "设置阀值";
            this.btnSetFZ.UseVisualStyleBackColor = true;
            this.btnSetFZ.Click += new System.EventHandler(this.btnRecognize_Click);
            // 
            // picbox
            // 
            this.picbox.Location = new System.Drawing.Point(12, 12);
            this.picbox.Name = "picbox";
            this.picbox.Size = new System.Drawing.Size(500, 170);
            this.picbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picbox.TabIndex = 1;
            this.picbox.TabStop = false;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(255, 281);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "获取验证码";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // txtFZ
            // 
            this.txtFZ.Location = new System.Drawing.Point(107, 283);
            this.txtFZ.Name = "txtFZ";
            this.txtFZ.Size = new System.Drawing.Size(48, 21);
            this.txtFZ.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(336, 281);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEZ
            // 
            this.btnEZ.Location = new System.Drawing.Point(336, 313);
            this.btnEZ.Name = "btnEZ";
            this.btnEZ.Size = new System.Drawing.Size(75, 23);
            this.btnEZ.TabIndex = 5;
            this.btnEZ.Text = "二值化";
            this.btnEZ.UseVisualStyleBackColor = true;
            this.btnEZ.Click += new System.EventHandler(this.btnEZ_Click);
            // 
            // txtEZ
            // 
            this.txtEZ.Location = new System.Drawing.Point(107, 310);
            this.txtEZ.Name = "txtEZ";
            this.txtEZ.Size = new System.Drawing.Size(48, 21);
            this.txtEZ.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 286);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "干扰线色差阀值";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 313);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "二值化亮度阀值";
            // 
            // txtBJFZ
            // 
            this.txtBJFZ.Location = new System.Drawing.Point(107, 337);
            this.txtBJFZ.Name = "txtBJFZ";
            this.txtBJFZ.Size = new System.Drawing.Size(48, 21);
            this.txtBJFZ.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 340);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "背景近似度阀值";
            // 
            // btnDropBG
            // 
            this.btnDropBG.Location = new System.Drawing.Point(174, 313);
            this.btnDropBG.Name = "btnDropBG";
            this.btnDropBG.Size = new System.Drawing.Size(75, 23);
            this.btnDropBG.TabIndex = 11;
            this.btnDropBG.Text = "去背景";
            this.btnDropBG.UseVisualStyleBackColor = true;
            this.btnDropBG.Click += new System.EventHandler(this.btnDropBG_Click);
            // 
            // btnDropDisturb
            // 
            this.btnDropDisturb.Location = new System.Drawing.Point(255, 313);
            this.btnDropDisturb.Name = "btnDropDisturb";
            this.btnDropDisturb.Size = new System.Drawing.Size(75, 23);
            this.btnDropDisturb.TabIndex = 12;
            this.btnDropDisturb.Text = "去干扰";
            this.btnDropDisturb.UseVisualStyleBackColor = true;
            this.btnDropDisturb.Click += new System.EventHandler(this.btnDropDisturb_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(417, 281);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 13;
            this.btnReset.Text = "重置";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // pbNormal
            // 
            this.pbNormal.Location = new System.Drawing.Point(174, 343);
            this.pbNormal.Name = "pbNormal";
            this.pbNormal.Size = new System.Drawing.Size(130, 50);
            this.pbNormal.TabIndex = 18;
            this.pbNormal.TabStop = false;
            // 
            // pbSplit1
            // 
            this.pbSplit1.Location = new System.Drawing.Point(0, 3);
            this.pbSplit1.Name = "pbSplit1";
            this.pbSplit1.Size = new System.Drawing.Size(50, 50);
            this.pbSplit1.TabIndex = 19;
            this.pbSplit1.TabStop = false;
            // 
            // pbSplit2
            // 
            this.pbSplit2.Location = new System.Drawing.Point(56, 3);
            this.pbSplit2.Name = "pbSplit2";
            this.pbSplit2.Size = new System.Drawing.Size(50, 50);
            this.pbSplit2.TabIndex = 20;
            this.pbSplit2.TabStop = false;
            // 
            // pbSplit3
            // 
            this.pbSplit3.Location = new System.Drawing.Point(112, 3);
            this.pbSplit3.Name = "pbSplit3";
            this.pbSplit3.Size = new System.Drawing.Size(50, 50);
            this.pbSplit3.TabIndex = 20;
            this.pbSplit3.TabStop = false;
            // 
            // pbSplit4
            // 
            this.pbSplit4.Location = new System.Drawing.Point(168, 3);
            this.pbSplit4.Name = "pbSplit4";
            this.pbSplit4.Size = new System.Drawing.Size(50, 50);
            this.pbSplit4.TabIndex = 20;
            this.pbSplit4.TabStop = false;
            // 
            // pbSplit5
            // 
            this.pbSplit5.Location = new System.Drawing.Point(224, 3);
            this.pbSplit5.Name = "pbSplit5";
            this.pbSplit5.Size = new System.Drawing.Size(50, 50);
            this.pbSplit5.TabIndex = 20;
            this.pbSplit5.TabStop = false;
            // 
            // pbSplit6
            // 
            this.pbSplit6.Location = new System.Drawing.Point(280, 3);
            this.pbSplit6.Name = "pbSplit6";
            this.pbSplit6.Size = new System.Drawing.Size(50, 50);
            this.pbSplit6.TabIndex = 20;
            this.pbSplit6.TabStop = false;
            // 
            // btnSplit
            // 
            this.btnSplit.Location = new System.Drawing.Point(417, 314);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(75, 23);
            this.btnSplit.TabIndex = 21;
            this.btnSplit.Text = "分割";
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // panelCutPic
            // 
            this.panelCutPic.Controls.Add(this.pbSplit2);
            this.panelCutPic.Controls.Add(this.pbSplit1);
            this.panelCutPic.Controls.Add(this.pbSplit6);
            this.panelCutPic.Controls.Add(this.pbSplit3);
            this.panelCutPic.Controls.Add(this.pbSplit5);
            this.panelCutPic.Controls.Add(this.pbSplit4);
            this.panelCutPic.Location = new System.Drawing.Point(12, 186);
            this.panelCutPic.Name = "panelCutPic";
            this.panelCutPic.Size = new System.Drawing.Size(360, 58);
            this.panelCutPic.TabIndex = 22;
            // 
            // txtC1
            // 
            this.txtC1.Location = new System.Drawing.Point(12, 250);
            this.txtC1.Name = "txtC1";
            this.txtC1.Size = new System.Drawing.Size(50, 21);
            this.txtC1.TabIndex = 23;
            // 
            // txtC2
            // 
            this.txtC2.Location = new System.Drawing.Point(66, 250);
            this.txtC2.Name = "txtC2";
            this.txtC2.Size = new System.Drawing.Size(50, 21);
            this.txtC2.TabIndex = 24;
            // 
            // txtC3
            // 
            this.txtC3.Location = new System.Drawing.Point(124, 250);
            this.txtC3.Name = "txtC3";
            this.txtC3.Size = new System.Drawing.Size(50, 21);
            this.txtC3.TabIndex = 25;
            // 
            // txtC4
            // 
            this.txtC4.Location = new System.Drawing.Point(180, 250);
            this.txtC4.Name = "txtC4";
            this.txtC4.Size = new System.Drawing.Size(50, 21);
            this.txtC4.TabIndex = 26;
            // 
            // txtC5
            // 
            this.txtC5.Location = new System.Drawing.Point(236, 250);
            this.txtC5.Name = "txtC5";
            this.txtC5.Size = new System.Drawing.Size(50, 21);
            this.txtC5.TabIndex = 27;
            // 
            // txtC6
            // 
            this.txtC6.Location = new System.Drawing.Point(292, 250);
            this.txtC6.Name = "txtC6";
            this.txtC6.Size = new System.Drawing.Size(50, 21);
            this.txtC6.TabIndex = 28;
            // 
            // btnStudy
            // 
            this.btnStudy.Location = new System.Drawing.Point(348, 248);
            this.btnStudy.Name = "btnStudy";
            this.btnStudy.Size = new System.Drawing.Size(75, 23);
            this.btnStudy.TabIndex = 29;
            this.btnStudy.Text = "学习";
            this.btnStudy.UseVisualStyleBackColor = true;
            this.btnStudy.Click += new System.EventHandler(this.btnStudy_Click);
            // 
            // btnRecognize
            // 
            this.btnRecognize.Location = new System.Drawing.Point(429, 248);
            this.btnRecognize.Name = "btnRecognize";
            this.btnRecognize.Size = new System.Drawing.Size(75, 23);
            this.btnRecognize.TabIndex = 30;
            this.btnRecognize.Text = "识别";
            this.btnRecognize.UseVisualStyleBackColor = true;
            this.btnRecognize.Click += new System.EventHandler(this.btnRecognize_Click_1);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(414, 215);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(0, 12);
            this.lblResult.TabIndex = 31;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 404);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(525, 22);
            this.statusStrip1.TabIndex = 34;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 426);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnRecognize);
            this.Controls.Add(this.btnStudy);
            this.Controls.Add(this.txtC6);
            this.Controls.Add(this.txtC5);
            this.Controls.Add(this.txtC4);
            this.Controls.Add(this.txtC3);
            this.Controls.Add(this.txtC2);
            this.Controls.Add(this.txtC1);
            this.Controls.Add(this.panelCutPic);
            this.Controls.Add(this.btnSplit);
            this.Controls.Add(this.pbNormal);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnDropDisturb);
            this.Controls.Add(this.btnDropBG);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBJFZ);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEZ);
            this.Controls.Add(this.btnEZ);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtFZ);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.picbox);
            this.Controls.Add(this.btnSetFZ);
            this.Name = "MainForm";
            this.Text = "验证码识别";
            ((System.ComponentModel.ISupportInitialize)(this.picbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNormal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSplit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSplit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSplit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSplit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSplit5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSplit6)).EndInit();
            this.panelCutPic.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSetFZ;
        private System.Windows.Forms.PictureBox picbox;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox txtFZ;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEZ;
        private System.Windows.Forms.TextBox txtEZ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBJFZ;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDropBG;
        private System.Windows.Forms.Button btnDropDisturb;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.PictureBox pbNormal;
        private System.Windows.Forms.PictureBox pbSplit1;
        private System.Windows.Forms.PictureBox pbSplit2;
        private System.Windows.Forms.PictureBox pbSplit3;
        private System.Windows.Forms.PictureBox pbSplit4;
        private System.Windows.Forms.PictureBox pbSplit5;
        private System.Windows.Forms.PictureBox pbSplit6;
        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.Panel panelCutPic;
        private System.Windows.Forms.TextBox txtC1;
        private System.Windows.Forms.TextBox txtC2;
        private System.Windows.Forms.TextBox txtC3;
        private System.Windows.Forms.TextBox txtC4;
        private System.Windows.Forms.TextBox txtC5;
        private System.Windows.Forms.TextBox txtC6;
        private System.Windows.Forms.Button btnStudy;
        private System.Windows.Forms.Button btnRecognize;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

