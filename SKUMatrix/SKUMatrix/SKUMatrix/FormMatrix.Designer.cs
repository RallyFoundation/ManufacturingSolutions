namespace SKUMatrix
{
    partial class FormMatrix
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMatrix));
            this.metroTabControlMain = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPageHome = new MetroFramework.Controls.MetroTabPage();
            this.metroTileOnlineCheck = new MetroFramework.Controls.MetroTile();
            this.metroTileOfflineCheck = new MetroFramework.Controls.MetroTile();
            this.metroTabPageResult = new MetroFramework.Controls.MetroTabPage();
            this.metroTabPageReference = new MetroFramework.Controls.MetroTabPage();
            this.openFileDialogDecoded4KHH = new System.Windows.Forms.OpenFileDialog();
            this.metroTabControlMain.SuspendLayout();
            this.metroTabPageHome.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroTabControlMain
            // 
            this.metroTabControlMain.Controls.Add(this.metroTabPageHome);
            this.metroTabControlMain.Controls.Add(this.metroTabPageResult);
            this.metroTabControlMain.Controls.Add(this.metroTabPageReference);
            this.metroTabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControlMain.Location = new System.Drawing.Point(20, 60);
            this.metroTabControlMain.Name = "metroTabControlMain";
            this.metroTabControlMain.SelectedIndex = 1;
            this.metroTabControlMain.Size = new System.Drawing.Size(749, 393);
            this.metroTabControlMain.TabIndex = 0;
            // 
            // metroTabPageHome
            // 
            this.metroTabPageHome.Controls.Add(this.metroTileOnlineCheck);
            this.metroTabPageHome.Controls.Add(this.metroTileOfflineCheck);
            this.metroTabPageHome.HorizontalScrollbarBarColor = true;
            this.metroTabPageHome.Location = new System.Drawing.Point(4, 36);
            this.metroTabPageHome.Name = "metroTabPageHome";
            this.metroTabPageHome.Size = new System.Drawing.Size(741, 353);
            this.metroTabPageHome.TabIndex = 0;
            this.metroTabPageHome.Text = "Home";
            this.metroTabPageHome.VerticalScrollbarBarColor = true;
            // 
            // metroTileOnlineCheck
            // 
            this.metroTileOnlineCheck.Location = new System.Drawing.Point(316, 19);
            this.metroTileOnlineCheck.Name = "metroTileOnlineCheck";
            this.metroTileOnlineCheck.Size = new System.Drawing.Size(285, 143);
            this.metroTileOnlineCheck.Style = MetroFramework.MetroColorStyle.Pink;
            this.metroTileOnlineCheck.TabIndex = 3;
            this.metroTileOnlineCheck.Text = "Check This Computer";
            this.metroTileOnlineCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileOnlineCheck.Visible = false;
            // 
            // metroTileOfflineCheck
            // 
            this.metroTileOfflineCheck.Location = new System.Drawing.Point(3, 19);
            this.metroTileOfflineCheck.Name = "metroTileOfflineCheck";
            this.metroTileOfflineCheck.Size = new System.Drawing.Size(285, 143);
            this.metroTileOfflineCheck.Style = MetroFramework.MetroColorStyle.Lime;
            this.metroTileOfflineCheck.TabIndex = 2;
            this.metroTileOfflineCheck.Text = "Open Decoded 4K Hardware Hash";
            this.metroTileOfflineCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileOfflineCheck.Click += new System.EventHandler(this.metroTileOfflineCheck_Click);
            // 
            // metroTabPageResult
            // 
            this.metroTabPageResult.HorizontalScrollbarBarColor = true;
            this.metroTabPageResult.Location = new System.Drawing.Point(4, 36);
            this.metroTabPageResult.Name = "metroTabPageResult";
            this.metroTabPageResult.Size = new System.Drawing.Size(741, 353);
            this.metroTabPageResult.TabIndex = 1;
            this.metroTabPageResult.Text = "Result";
            this.metroTabPageResult.VerticalScrollbarBarColor = true;
            // 
            // metroTabPageReference
            // 
            this.metroTabPageReference.HorizontalScrollbarBarColor = true;
            this.metroTabPageReference.Location = new System.Drawing.Point(4, 36);
            this.metroTabPageReference.Name = "metroTabPageReference";
            this.metroTabPageReference.Size = new System.Drawing.Size(741, 353);
            this.metroTabPageReference.TabIndex = 2;
            this.metroTabPageReference.Text = "Lookup";
            this.metroTabPageReference.VerticalScrollbarBarColor = true;
            // 
            // openFileDialogDecoded4KHH
            // 
            this.openFileDialogDecoded4KHH.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            this.openFileDialogDecoded4KHH.Title = "Open Decoded 4K Hardware Hash";
            // 
            // FormMatrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 473);
            this.Controls.Add(this.metroTabControlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMatrix";
            this.Text = "SKU Matrix";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.metroTabControlMain.ResumeLayout(false);
            this.metroTabPageHome.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTabControl metroTabControlMain;
        private MetroFramework.Controls.MetroTabPage metroTabPageHome;
        private MetroFramework.Controls.MetroTabPage metroTabPageResult;
        private MetroFramework.Controls.MetroTile metroTileOfflineCheck;
        private MetroFramework.Controls.MetroTabPage metroTabPageReference;
        private System.Windows.Forms.OpenFileDialog openFileDialogDecoded4KHH;
        private MetroFramework.Controls.MetroTile metroTileOnlineCheck;
    }
}

