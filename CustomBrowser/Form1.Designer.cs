namespace CustomBrowser
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbBack = new System.Windows.Forms.ToolStripButton();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbForward = new System.Windows.Forms.ToolStripButton();
            this.tstUrl = new System.Windows.Forms.ToolStripTextBox();
            this.tsbRequest = new System.Windows.Forms.ToolStripButton();
            this.CustomBrowser = new System.Windows.Forms.WebBrowser();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbBack,
            this.tsbRefresh,
            this.tsbForward,
            this.tstUrl,
            this.tsbRequest});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(958, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbBack
            // 
            this.tsbBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbBack.Image = ((System.Drawing.Image)(resources.GetObject("tsbBack.Image")));
            this.tsbBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBack.Name = "tsbBack";
            this.tsbBack.Size = new System.Drawing.Size(36, 22);
            this.tsbBack.Text = "后退";
            this.tsbBack.Click += new System.EventHandler(this.tsbBack_Click);
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tsbRefresh.Image")));
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(36, 22);
            this.tsbRefresh.Text = "刷新";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // tsbForward
            // 
            this.tsbForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbForward.Image = ((System.Drawing.Image)(resources.GetObject("tsbForward.Image")));
            this.tsbForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbForward.Name = "tsbForward";
            this.tsbForward.Size = new System.Drawing.Size(36, 22);
            this.tsbForward.Text = "前进";
            this.tsbForward.Click += new System.EventHandler(this.tsbForward_Click);
            // 
            // tstUrl
            // 
            this.tstUrl.Name = "tstUrl";
            this.tstUrl.Size = new System.Drawing.Size(200, 25);
            // 
            // tsbRequest
            // 
            this.tsbRequest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRequest.Image = ((System.Drawing.Image)(resources.GetObject("tsbRequest.Image")));
            this.tsbRequest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRequest.Name = "tsbRequest";
            this.tsbRequest.Size = new System.Drawing.Size(30, 22);
            this.tsbRequest.Text = "=>";
            this.tsbRequest.Click += new System.EventHandler(this.tsbRequest_Click);
            // 
            // CustomBrowser
            // 
            this.CustomBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CustomBrowser.Location = new System.Drawing.Point(0, 25);
            this.CustomBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.CustomBrowser.Name = "CustomBrowser";
            this.CustomBrowser.Size = new System.Drawing.Size(958, 529);
            this.CustomBrowser.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 554);
            this.Controls.Add(this.CustomBrowser);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "我的浏览器";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbBack;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripButton tsbForward;
        private System.Windows.Forms.ToolStripTextBox tstUrl;
        private System.Windows.Forms.ToolStripButton tsbRequest;
        private System.Windows.Forms.WebBrowser CustomBrowser;
    }
}

