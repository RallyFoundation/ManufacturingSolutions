namespace MfgSolutionsDeploymentCenter
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.metroTileInstall = new MetroFramework.Controls.MetroTile();
            this.metroTileAutoScanning = new MetroFramework.Controls.MetroTile();
            this.metroTileAutoOA30SFT = new MetroFramework.Controls.MetroTile();
            this.metroTileSNInjectionTool = new MetroFramework.Controls.MetroTile();
            this.metroToolTipInfo = new MetroFramework.Components.MetroToolTip();
            this.SuspendLayout();
            // 
            // metroTileInstall
            // 
            this.metroTileInstall.ActiveControl = null;
            this.metroTileInstall.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroTileInstall.Location = new System.Drawing.Point(149, 117);
            this.metroTileInstall.Name = "metroTileInstall";
            this.metroTileInstall.Size = new System.Drawing.Size(250, 120);
            this.metroTileInstall.Style = MetroFramework.MetroColorStyle.Orange;
            this.metroTileInstall.TabIndex = 20;
            this.metroTileInstall.Text = "DIS 2.0 Installation";
            this.metroTileInstall.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileInstall.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroToolTipInfo.SetToolTip(this.metroTileInstall, "Preparation: \r\nInstall Windows Server 2008 R2\r\n SP1 + WMF3.0 or Windows Server 20" +
        "12 (R2) \r\nbefore DIS 2.0 Installation");
            this.metroTileInstall.UseSelectable = true;
            this.metroTileInstall.Click += new System.EventHandler(this.metroTileInstall_Click);
            // 
            // metroTileAutoScanning
            // 
            this.metroTileAutoScanning.ActiveControl = null;
            this.metroTileAutoScanning.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroTileAutoScanning.Location = new System.Drawing.Point(415, 117);
            this.metroTileAutoScanning.Name = "metroTileAutoScanning";
            this.metroTileAutoScanning.Size = new System.Drawing.Size(250, 120);
            this.metroTileAutoScanning.Style = MetroFramework.MetroColorStyle.Lime;
            this.metroTileAutoScanning.TabIndex = 21;
            this.metroTileAutoScanning.Text = "Auto Scanning Deployment";
            this.metroTileAutoScanning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileAutoScanning.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroToolTipInfo.SetToolTip(this.metroTileAutoScanning, "Preparation:\r\nConnect Honeywell IS3480 to \r\ncomputer and fix position in \r\nproduc" +
        "tion line. \r\nSample is available for demo ");
            this.metroTileAutoScanning.UseSelectable = true;
            this.metroTileAutoScanning.Click += new System.EventHandler(this.metroTileAutoScanning_Click);
            // 
            // metroTileAutoOA30SFT
            // 
            this.metroTileAutoOA30SFT.ActiveControl = null;
            this.metroTileAutoOA30SFT.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroTileAutoOA30SFT.Location = new System.Drawing.Point(149, 254);
            this.metroTileAutoOA30SFT.Name = "metroTileAutoOA30SFT";
            this.metroTileAutoOA30SFT.Size = new System.Drawing.Size(250, 120);
            this.metroTileAutoOA30SFT.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTileAutoOA30SFT.TabIndex = 22;
            this.metroTileAutoOA30SFT.Text = "Auto OA3.0 Deployment with SFT";
            this.metroTileAutoOA30SFT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileAutoOA30SFT.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroToolTipInfo.SetToolTip(this.metroTileAutoOA30SFT, "Preparation:\r\nA USB is required to download \r\nSFT with integrated Auto OA3.0 scri" +
        "pt");
            this.metroTileAutoOA30SFT.UseSelectable = true;
            this.metroTileAutoOA30SFT.Click += new System.EventHandler(this.metroTileAutoOA30SFT_Click);
            // 
            // metroTileSNInjectionTool
            // 
            this.metroTileSNInjectionTool.ActiveControl = null;
            this.metroTileSNInjectionTool.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroTileSNInjectionTool.Location = new System.Drawing.Point(415, 254);
            this.metroTileSNInjectionTool.Name = "metroTileSNInjectionTool";
            this.metroTileSNInjectionTool.Size = new System.Drawing.Size(250, 120);
            this.metroTileSNInjectionTool.Style = MetroFramework.MetroColorStyle.Yellow;
            this.metroTileSNInjectionTool.TabIndex = 23;
            this.metroTileSNInjectionTool.Text = "SN Injection \r\n(Please contact your BIOS vendor\r\n for serial number injection too" +
    "ls. \r\nSN-DPK bundle requires\r\n SN available in BIOS)";
            this.metroTileSNInjectionTool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileSNInjectionTool.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroToolTipInfo.SetToolTip(this.metroTileSNInjectionTool, "Please contact your BIOS vendor\r\n for serial number injection tools. \r\nSN-DPK bun" +
        "dle requires\r\n SN available in BIOS.");
            this.metroTileSNInjectionTool.UseSelectable = true;
            this.metroTileSNInjectionTool.Click += new System.EventHandler(this.metroTileSNInjectionTool_Click);
            // 
            // metroToolTipInfo
            // 
            this.metroToolTipInfo.AutoPopDelay = 12000;
            this.metroToolTipInfo.InitialDelay = 30;
            this.metroToolTipInfo.ReshowDelay = 20;
            this.metroToolTipInfo.Style = MetroFramework.MetroColorStyle.Pink;
            this.metroToolTipInfo.StyleManager = null;
            this.metroToolTipInfo.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 450);
            this.Controls.Add(this.metroTileSNInjectionTool);
            this.Controls.Add(this.metroTileAutoOA30SFT);
            this.Controls.Add(this.metroTileAutoScanning);
            this.Controls.Add(this.metroTileInstall);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Resizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Manufacturing Solutions Deployment Center";
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTile metroTileInstall;
        private MetroFramework.Controls.MetroTile metroTileAutoScanning;
        private MetroFramework.Controls.MetroTile metroTileAutoOA30SFT;
        private MetroFramework.Controls.MetroTile metroTileSNInjectionTool;
        private MetroFramework.Components.MetroToolTip metroToolTipInfo;


    }
}

