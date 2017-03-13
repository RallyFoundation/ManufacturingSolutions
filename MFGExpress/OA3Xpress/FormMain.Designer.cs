namespace OA3Xpress
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
            this.metroRadioButtonSysArchX86 = new MetroFramework.Controls.MetroRadioButton();
            this.metroRadioButtonSysArchAmd64 = new MetroFramework.Controls.MetroRadioButton();
            this.metroLabelSysArch = new MetroFramework.Controls.MetroLabel();
            this.metroTileConfigure = new MetroFramework.Controls.MetroTile();
            this.metroTileStart = new MetroFramework.Controls.MetroTile();
            this.metroTileErase = new MetroFramework.Controls.MetroTile();
            this.metroTileValidate = new MetroFramework.Controls.MetroTile();
            this.SuspendLayout();
            // 
            // metroRadioButtonSysArchX86
            // 
            this.metroRadioButtonSysArchX86.AutoSize = true;
            this.metroRadioButtonSysArchX86.Checked = true;
            this.metroRadioButtonSysArchX86.Location = new System.Drawing.Point(166, 77);
            this.metroRadioButtonSysArchX86.Name = "metroRadioButtonSysArchX86";
            this.metroRadioButtonSysArchX86.Size = new System.Drawing.Size(40, 15);
            this.metroRadioButtonSysArchX86.TabIndex = 0;
            this.metroRadioButtonSysArchX86.TabStop = true;
            this.metroRadioButtonSysArchX86.Text = "x86";
            this.metroRadioButtonSysArchX86.UseVisualStyleBackColor = true;
            this.metroRadioButtonSysArchX86.CheckedChanged += new System.EventHandler(this.metroRadioButtonSysArchX86_CheckedChanged);
            // 
            // metroRadioButtonSysArchAmd64
            // 
            this.metroRadioButtonSysArchAmd64.AutoSize = true;
            this.metroRadioButtonSysArchAmd64.Location = new System.Drawing.Point(237, 77);
            this.metroRadioButtonSysArchAmd64.Name = "metroRadioButtonSysArchAmd64";
            this.metroRadioButtonSysArchAmd64.Size = new System.Drawing.Size(59, 15);
            this.metroRadioButtonSysArchAmd64.TabIndex = 1;
            this.metroRadioButtonSysArchAmd64.Text = "amd64";
            this.metroRadioButtonSysArchAmd64.UseVisualStyleBackColor = true;
            // 
            // metroLabelSysArch
            // 
            this.metroLabelSysArch.AutoSize = true;
            this.metroLabelSysArch.Location = new System.Drawing.Point(24, 73);
            this.metroLabelSysArch.Name = "metroLabelSysArch";
            this.metroLabelSysArch.Size = new System.Drawing.Size(127, 19);
            this.metroLabelSysArch.TabIndex = 2;
            this.metroLabelSysArch.Text = "System Architecture:";
            // 
            // metroTileConfigure
            // 
            this.metroTileConfigure.Location = new System.Drawing.Point(25, 125);
            this.metroTileConfigure.Name = "metroTileConfigure";
            this.metroTileConfigure.Size = new System.Drawing.Size(151, 111);
            this.metroTileConfigure.Style = MetroFramework.MetroColorStyle.Orange;
            this.metroTileConfigure.TabIndex = 3;
            this.metroTileConfigure.Text = "Configure";
            this.metroTileConfigure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileConfigure.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTileConfigure.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileConfigure.Click += new System.EventHandler(this.metroTileConfigure_Click);
            // 
            // metroTileStart
            // 
            this.metroTileStart.Location = new System.Drawing.Point(197, 125);
            this.metroTileStart.Name = "metroTileStart";
            this.metroTileStart.Size = new System.Drawing.Size(151, 111);
            this.metroTileStart.Style = MetroFramework.MetroColorStyle.Lime;
            this.metroTileStart.TabIndex = 4;
            this.metroTileStart.Text = "Start";
            this.metroTileStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileStart.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTileStart.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileStart.Click += new System.EventHandler(this.metroTileStart_Click);
            // 
            // metroTileErase
            // 
            this.metroTileErase.Location = new System.Drawing.Point(197, 258);
            this.metroTileErase.Name = "metroTileErase";
            this.metroTileErase.Size = new System.Drawing.Size(151, 111);
            this.metroTileErase.Style = MetroFramework.MetroColorStyle.Yellow;
            this.metroTileErase.TabIndex = 6;
            this.metroTileErase.Text = "Erase";
            this.metroTileErase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileErase.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTileErase.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileErase.Click += new System.EventHandler(this.metroTileErase_Click);
            // 
            // metroTileValidate
            // 
            this.metroTileValidate.Location = new System.Drawing.Point(25, 258);
            this.metroTileValidate.Name = "metroTileValidate";
            this.metroTileValidate.Size = new System.Drawing.Size(151, 111);
            this.metroTileValidate.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTileValidate.TabIndex = 5;
            this.metroTileValidate.Text = "Validate";
            this.metroTileValidate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileValidate.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTileValidate.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileValidate.Click += new System.EventHandler(this.metroTileValidate_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 400);
            this.Controls.Add(this.metroTileErase);
            this.Controls.Add(this.metroTileValidate);
            this.Controls.Add(this.metroTileStart);
            this.Controls.Add(this.metroTileConfigure);
            this.Controls.Add(this.metroLabelSysArch);
            this.Controls.Add(this.metroRadioButtonSysArchAmd64);
            this.Controls.Add(this.metroRadioButtonSysArchX86);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Resizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "OA3.0 Process Execution Panel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroRadioButton metroRadioButtonSysArchX86;
        private MetroFramework.Controls.MetroRadioButton metroRadioButtonSysArchAmd64;
        private MetroFramework.Controls.MetroLabel metroLabelSysArch;
        private MetroFramework.Controls.MetroTile metroTileConfigure;
        private MetroFramework.Controls.MetroTile metroTileStart;
        private MetroFramework.Controls.MetroTile metroTileErase;
        private MetroFramework.Controls.MetroTile metroTileValidate;
    }
}

