namespace Cooshu.Spider
{
    partial class WinMain
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
            this.BtnStart = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.TbxLog = new System.Windows.Forms.TextBox();
            this.DgvSites = new System.Windows.Forms.DataGridView();
            this.SiteName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoginData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThreadCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Waiting = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Error = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Start = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnProcessor = new System.Windows.Forms.DataGridViewButtonColumn();
            this.RetryError = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DgvSites)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnStart
            // 
            this.BtnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnStart.Location = new System.Drawing.Point(875, 12);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(95, 23);
            this.BtnStart.TabIndex = 0;
            this.BtnStart.Text = "全部开始";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStartClick);
            // 
            // BtnStop
            // 
            this.BtnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnStop.Location = new System.Drawing.Point(772, 11);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 23);
            this.BtnStop.TabIndex = 1;
            this.BtnStop.Text = "停止";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStopClick);
            // 
            // BtnExit
            // 
            this.BtnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnExit.Location = new System.Drawing.Point(978, 12);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(95, 23);
            this.BtnExit.TabIndex = 2;
            this.BtnExit.Text = "退出";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // TbxLog
            // 
            this.TbxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbxLog.Location = new System.Drawing.Point(0, 258);
            this.TbxLog.Multiline = true;
            this.TbxLog.Name = "TbxLog";
            this.TbxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbxLog.Size = new System.Drawing.Size(1110, 405);
            this.TbxLog.TabIndex = 3;
            // 
            // DgvSites
            // 
            this.DgvSites.AllowUserToAddRows = false;
            this.DgvSites.AllowUserToDeleteRows = false;
            this.DgvSites.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvSites.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvSites.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SiteName,
            this.LoginData,
            this.ThreadCount,
            this.Waiting,
            this.Error,
            this.Start,
            this.btnProcessor,
            this.RetryError});
            this.DgvSites.Location = new System.Drawing.Point(0, 40);
            this.DgvSites.Name = "DgvSites";
            this.DgvSites.RowTemplate.Height = 23;
            this.DgvSites.Size = new System.Drawing.Size(1110, 213);
            this.DgvSites.TabIndex = 11;
            this.DgvSites.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSites_CellClick);
            // 
            // SiteName
            // 
            this.SiteName.DataPropertyName = "SiteName";
            this.SiteName.FillWeight = 200F;
            this.SiteName.HeaderText = "网站名称";
            this.SiteName.MaxInputLength = 50;
            this.SiteName.Name = "SiteName";
            this.SiteName.ReadOnly = true;
            this.SiteName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SiteName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SiteName.Width = 200;
            // 
            // LoginData
            // 
            this.LoginData.FillWeight = 200F;
            this.LoginData.HeaderText = "登录数据";
            this.LoginData.Name = "LoginData";
            this.LoginData.Width = 200;
            // 
            // ThreadCount
            // 
            this.ThreadCount.DataPropertyName = "ThreadCount";
            this.ThreadCount.HeaderText = "线程数";
            this.ThreadCount.MaxInputLength = 3;
            this.ThreadCount.Name = "ThreadCount";
            this.ThreadCount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ThreadCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ThreadCount.ToolTipText = "同时抓取数据的线程数量";
            // 
            // Waiting
            // 
            this.Waiting.DataPropertyName = "Waiting";
            this.Waiting.HeaderText = "等待处理数";
            this.Waiting.MaxInputLength = 20;
            this.Waiting.Name = "Waiting";
            this.Waiting.ReadOnly = true;
            this.Waiting.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Waiting.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Waiting.ToolTipText = "正在等待处理的网页数";
            // 
            // Error
            // 
            this.Error.DataPropertyName = "Error";
            this.Error.HeaderText = "错误数";
            this.Error.MaxInputLength = 20;
            this.Error.Name = "Error";
            this.Error.ReadOnly = true;
            this.Error.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Error.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Error.ToolTipText = "无法正确采集数据的网页数";
            // 
            // Start
            // 
            this.Start.DataPropertyName = "State";
            this.Start.HeaderText = "开始";
            this.Start.Name = "Start";
            this.Start.Text = "开始";
            this.Start.Width = 94;
            // 
            // btnProcessor
            // 
            this.btnProcessor.DataPropertyName = "btnProcessor";
            this.btnProcessor.HeaderText = "附加处理";
            this.btnProcessor.Name = "btnProcessor";
            this.btnProcessor.Text = "附加处理";
            this.btnProcessor.UseColumnTextForButtonValue = true;
            // 
            // RetryError
            // 
            this.RetryError.HeaderText = "重试错误任务";
            this.RetryError.Name = "RetryError";
            this.RetryError.Text = "重试错误任务";
            this.RetryError.UseColumnTextForButtonValue = true;
            // 
            // WinMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1110, 664);
            this.Controls.Add(this.DgvSites);
            this.Controls.Add(this.TbxLog);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnStart);
            this.Name = "WinMain";
            this.Text = "采集器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WinMain_FormClosing);
            this.Load += new System.EventHandler(this.WinMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvSites)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.TextBox TbxLog;
        private System.Windows.Forms.DataGridView DgvSites;
        private System.Windows.Forms.DataGridViewTextBoxColumn SiteName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LoginData;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThreadCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Waiting;
        private System.Windows.Forms.DataGridViewTextBoxColumn Error;
        private System.Windows.Forms.DataGridViewButtonColumn Start;
        private System.Windows.Forms.DataGridViewButtonColumn btnProcessor;
        private System.Windows.Forms.DataGridViewButtonColumn RetryError;
    }
}

