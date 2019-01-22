namespace VamtXpress
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
            this.metroTileConfigure = new MetroFramework.Controls.MetroTile();
            this.metroTileStart = new MetroFramework.Controls.MetroTile();
            this.metroTilePrepare = new MetroFramework.Controls.MetroTile();
            this.metroTileCleanup = new MetroFramework.Controls.MetroTile();
            this.SuspendLayout();
            // 
            // metroTileConfigure
            // 
            this.metroTileConfigure.Location = new System.Drawing.Point(23, 72);
            this.metroTileConfigure.Name = "metroTileConfigure";
            this.metroTileConfigure.Size = new System.Drawing.Size(134, 117);
            this.metroTileConfigure.Style = MetroFramework.MetroColorStyle.Red;
            this.metroTileConfigure.TabIndex = 3;
            this.metroTileConfigure.Text = "Configure";
            this.metroTileConfigure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileConfigure.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTileConfigure.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileConfigure.Click += new System.EventHandler(this.metroTileConfigure_Click);
            // 
            // metroTileStart
            // 
            this.metroTileStart.Location = new System.Drawing.Point(23, 209);
            this.metroTileStart.Name = "metroTileStart";
            this.metroTileStart.Size = new System.Drawing.Size(134, 117);
            this.metroTileStart.Style = MetroFramework.MetroColorStyle.Green;
            this.metroTileStart.TabIndex = 4;
            this.metroTileStart.Text = "Start";
            this.metroTileStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileStart.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTileStart.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileStart.Click += new System.EventHandler(this.metroTileStart_Click);
            // 
            // metroTilePrepare
            // 
            this.metroTilePrepare.Location = new System.Drawing.Point(176, 72);
            this.metroTilePrepare.Name = "metroTilePrepare";
            this.metroTilePrepare.Size = new System.Drawing.Size(134, 117);
            this.metroTilePrepare.Style = MetroFramework.MetroColorStyle.Yellow;
            this.metroTilePrepare.TabIndex = 5;
            this.metroTilePrepare.Text = "Prepare";
            this.metroTilePrepare.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTilePrepare.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTilePrepare.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTilePrepare.Visible = false;
            this.metroTilePrepare.Click += new System.EventHandler(this.metroTilePrepare_Click);
            // 
            // metroTileCleanup
            // 
            this.metroTileCleanup.Location = new System.Drawing.Point(176, 209);
            this.metroTileCleanup.Name = "metroTileCleanup";
            this.metroTileCleanup.Size = new System.Drawing.Size(134, 117);
            this.metroTileCleanup.Style = MetroFramework.MetroColorStyle.Silver;
            this.metroTileCleanup.TabIndex = 6;
            this.metroTileCleanup.Text = "Cleanup";
            this.metroTileCleanup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileCleanup.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTileCleanup.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileCleanup.Visible = false;
            this.metroTileCleanup.Click += new System.EventHandler(this.metroTileCleanup_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 350);
            this.Controls.Add(this.metroTileCleanup);
            this.Controls.Add(this.metroTilePrepare);
            this.Controls.Add(this.metroTileStart);
            this.Controls.Add(this.metroTileConfigure);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Resizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "VAMT Execution Panel";
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroTile metroTileConfigure;
        private MetroFramework.Controls.MetroTile metroTileStart;
        private MetroFramework.Controls.MetroTile metroTilePrepare;
        private MetroFramework.Controls.MetroTile metroTileCleanup;
    }
}

