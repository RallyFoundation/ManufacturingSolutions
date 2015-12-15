namespace MfgSolutionsDeploymentCenter
{
    partial class FormCloudInstallation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCloudInstallation));
            this.metroLinkBack = new MetroFramework.Controls.MetroLink();
            this.metroTileConfigSecPol = new MetroFramework.Controls.MetroTile();
            this.metroTileFinish = new MetroFramework.Controls.MetroTile();
            this.metroTileStart = new MetroFramework.Controls.MetroTile();
            this.metroTabControlCloudInstallation = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPageArguments = new MetroFramework.Controls.MetroTabPage();
            this.metroTabPageResults = new MetroFramework.Controls.MetroTabPage();
            this.metroTextBoxResults = new MetroFramework.Controls.MetroTextBox();
            this.metroTabControlCloudInstallation.SuspendLayout();
            this.metroTabPageResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroLinkBack
            // 
            this.metroLinkBack.BackgroundImage = global::MfgSolutionsDeploymentCenter.Properties.Resources.MB_0006_back;
            this.metroLinkBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.metroLinkBack.Location = new System.Drawing.Point(20, 60);
            this.metroLinkBack.Name = "metroLinkBack";
            this.metroLinkBack.Size = new System.Drawing.Size(45, 45);
            this.metroLinkBack.TabIndex = 2;
            this.metroLinkBack.UseSelectable = true;
            this.metroLinkBack.Click += new System.EventHandler(this.metroLinkBack_Click);
            // 
            // metroTileConfigSecPol
            // 
            this.metroTileConfigSecPol.ActiveControl = null;
            this.metroTileConfigSecPol.Location = new System.Drawing.Point(24, 261);
            this.metroTileConfigSecPol.Name = "metroTileConfigSecPol";
            this.metroTileConfigSecPol.Size = new System.Drawing.Size(58, 58);
            this.metroTileConfigSecPol.Style = MetroFramework.MetroColorStyle.Purple;
            this.metroTileConfigSecPol.TabIndex = 8;
            this.metroTileConfigSecPol.Text = "Set";
            this.metroTileConfigSecPol.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.metroTileConfigSecPol.UseSelectable = true;
            this.metroTileConfigSecPol.Visible = false;
            // 
            // metroTileFinish
            // 
            this.metroTileFinish.ActiveControl = null;
            this.metroTileFinish.Enabled = false;
            this.metroTileFinish.Location = new System.Drawing.Point(87, 261);
            this.metroTileFinish.Name = "metroTileFinish";
            this.metroTileFinish.Size = new System.Drawing.Size(58, 58);
            this.metroTileFinish.Style = MetroFramework.MetroColorStyle.Orange;
            this.metroTileFinish.TabIndex = 7;
            this.metroTileFinish.Text = "Finish";
            this.metroTileFinish.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.metroTileFinish.UseSelectable = true;
            this.metroTileFinish.Visible = false;
            // 
            // metroTileStart
            // 
            this.metroTileStart.ActiveControl = null;
            this.metroTileStart.Location = new System.Drawing.Point(24, 136);
            this.metroTileStart.Name = "metroTileStart";
            this.metroTileStart.Size = new System.Drawing.Size(120, 120);
            this.metroTileStart.Style = MetroFramework.MetroColorStyle.Green;
            this.metroTileStart.TabIndex = 6;
            this.metroTileStart.Text = "Start";
            this.metroTileStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileStart.UseSelectable = true;
            this.metroTileStart.Click += new System.EventHandler(this.metroTileStart_Click);
            // 
            // metroTabControlCloudInstallation
            // 
            this.metroTabControlCloudInstallation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroTabControlCloudInstallation.Controls.Add(this.metroTabPageArguments);
            this.metroTabControlCloudInstallation.Controls.Add(this.metroTabPageResults);
            this.metroTabControlCloudInstallation.Location = new System.Drawing.Point(152, 100);
            this.metroTabControlCloudInstallation.Name = "metroTabControlCloudInstallation";
            this.metroTabControlCloudInstallation.SelectedIndex = 1;
            this.metroTabControlCloudInstallation.Size = new System.Drawing.Size(666, 336);
            this.metroTabControlCloudInstallation.TabIndex = 9;
            this.metroTabControlCloudInstallation.UseSelectable = true;
            // 
            // metroTabPageArguments
            // 
            this.metroTabPageArguments.HorizontalScrollbarBarColor = true;
            this.metroTabPageArguments.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPageArguments.HorizontalScrollbarSize = 10;
            this.metroTabPageArguments.Location = new System.Drawing.Point(4, 38);
            this.metroTabPageArguments.Name = "metroTabPageArguments";
            this.metroTabPageArguments.Size = new System.Drawing.Size(658, 294);
            this.metroTabPageArguments.TabIndex = 0;
            this.metroTabPageArguments.Text = "Arguments";
            this.metroTabPageArguments.VerticalScrollbarBarColor = true;
            this.metroTabPageArguments.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPageArguments.VerticalScrollbarSize = 10;
            // 
            // metroTabPageResults
            // 
            this.metroTabPageResults.Controls.Add(this.metroTextBoxResults);
            this.metroTabPageResults.HorizontalScrollbarBarColor = true;
            this.metroTabPageResults.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPageResults.HorizontalScrollbarSize = 10;
            this.metroTabPageResults.Location = new System.Drawing.Point(4, 38);
            this.metroTabPageResults.Name = "metroTabPageResults";
            this.metroTabPageResults.Size = new System.Drawing.Size(658, 294);
            this.metroTabPageResults.TabIndex = 1;
            this.metroTabPageResults.Text = "Results";
            this.metroTabPageResults.VerticalScrollbarBarColor = true;
            this.metroTabPageResults.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPageResults.VerticalScrollbarSize = 10;
            // 
            // metroTextBoxResults
            // 
            this.metroTextBoxResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTextBoxResults.Lines = new string[0];
            this.metroTextBoxResults.Location = new System.Drawing.Point(0, 0);
            this.metroTextBoxResults.MaxLength = 32767;
            this.metroTextBoxResults.Multiline = true;
            this.metroTextBoxResults.Name = "metroTextBoxResults";
            this.metroTextBoxResults.PasswordChar = '\0';
            this.metroTextBoxResults.ReadOnly = true;
            this.metroTextBoxResults.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxResults.SelectedText = "";
            this.metroTextBoxResults.Size = new System.Drawing.Size(658, 294);
            this.metroTextBoxResults.TabIndex = 2;
            this.metroTextBoxResults.UseSelectable = true;
            // 
            // FormCloudInstallation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 450);
            this.Controls.Add(this.metroTabControlCloudInstallation);
            this.Controls.Add(this.metroTileConfigSecPol);
            this.Controls.Add(this.metroTileFinish);
            this.Controls.Add(this.metroTileStart);
            this.Controls.Add(this.metroLinkBack);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormCloudInstallation";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Installing DIS Configuration Cloud";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCloudInstallation_FormClosing);
            this.metroTabControlCloudInstallation.ResumeLayout(false);
            this.metroTabPageResults.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroLink metroLinkBack;
        private MetroFramework.Controls.MetroTile metroTileConfigSecPol;
        private MetroFramework.Controls.MetroTile metroTileFinish;
        private MetroFramework.Controls.MetroTile metroTileStart;
        private MetroFramework.Controls.MetroTabControl metroTabControlCloudInstallation;
        private MetroFramework.Controls.MetroTabPage metroTabPageArguments;
        private MetroFramework.Controls.MetroTabPage metroTabPageResults;
        private MetroFramework.Controls.MetroTextBox metroTextBoxResults;
    }
}