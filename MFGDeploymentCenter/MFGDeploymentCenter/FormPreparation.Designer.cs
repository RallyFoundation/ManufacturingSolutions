namespace MfgSolutionsDeploymentCenter
{
    partial class FormPreparation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPreparation));
            this.metroLinkBack = new MetroFramework.Controls.MetroLink();
            this.metroTileStart = new MetroFramework.Controls.MetroTile();
            this.metroTextBoxPreparationDetail = new MetroFramework.Controls.MetroTextBox();
            this.metroTileFinish = new MetroFramework.Controls.MetroTile();
            this.metroTileConfigSecPol = new MetroFramework.Controls.MetroTile();
            this.SuspendLayout();
            // 
            // metroLinkBack
            // 
            this.metroLinkBack.BackgroundImage = global::MfgSolutionsDeploymentCenter.Properties.Resources.MB_0006_back;
            this.metroLinkBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.metroLinkBack.Location = new System.Drawing.Point(20, 60);
            this.metroLinkBack.Name = "metroLinkBack";
            this.metroLinkBack.Size = new System.Drawing.Size(45, 45);
            this.metroLinkBack.TabIndex = 0;
            this.metroLinkBack.UseSelectable = true;
            this.metroLinkBack.Click += new System.EventHandler(this.metroLinkBack_Click);
            // 
            // metroTileStart
            // 
            this.metroTileStart.ActiveControl = null;
            this.metroTileStart.Location = new System.Drawing.Point(24, 136);
            this.metroTileStart.Name = "metroTileStart";
            this.metroTileStart.Size = new System.Drawing.Size(120, 120);
            this.metroTileStart.Style = MetroFramework.MetroColorStyle.Green;
            this.metroTileStart.TabIndex = 1;
            this.metroTileStart.Text = "Start";
            this.metroTileStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileStart.UseSelectable = true;
            this.metroTileStart.Click += new System.EventHandler(this.metroTileStart_Click);
            // 
            // metroTextBoxPreparationDetail
            // 
            this.metroTextBoxPreparationDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroTextBoxPreparationDetail.Lines = new string[0];
            this.metroTextBoxPreparationDetail.Location = new System.Drawing.Point(159, 136);
            this.metroTextBoxPreparationDetail.MaxLength = 32767;
            this.metroTextBoxPreparationDetail.Multiline = true;
            this.metroTextBoxPreparationDetail.Name = "metroTextBoxPreparationDetail";
            this.metroTextBoxPreparationDetail.PasswordChar = '\0';
            this.metroTextBoxPreparationDetail.ReadOnly = true;
            this.metroTextBoxPreparationDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.metroTextBoxPreparationDetail.SelectedText = "";
            this.metroTextBoxPreparationDetail.Size = new System.Drawing.Size(648, 291);
            this.metroTextBoxPreparationDetail.TabIndex = 3;
            this.metroTextBoxPreparationDetail.UseSelectable = true;
            this.metroTextBoxPreparationDetail.Visible = false;
            // 
            // metroTileFinish
            // 
            this.metroTileFinish.ActiveControl = null;
            this.metroTileFinish.Enabled = false;
            this.metroTileFinish.Location = new System.Drawing.Point(87, 261);
            this.metroTileFinish.Name = "metroTileFinish";
            this.metroTileFinish.Size = new System.Drawing.Size(58, 58);
            this.metroTileFinish.Style = MetroFramework.MetroColorStyle.Orange;
            this.metroTileFinish.TabIndex = 4;
            this.metroTileFinish.Text = "Finish";
            this.metroTileFinish.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.metroTileFinish.UseSelectable = true;
            this.metroTileFinish.Visible = false;
            this.metroTileFinish.Click += new System.EventHandler(this.metroTileFinish_Click);
            // 
            // metroTileConfigSecPol
            // 
            this.metroTileConfigSecPol.ActiveControl = null;
            this.metroTileConfigSecPol.Location = new System.Drawing.Point(24, 261);
            this.metroTileConfigSecPol.Name = "metroTileConfigSecPol";
            this.metroTileConfigSecPol.Size = new System.Drawing.Size(58, 58);
            this.metroTileConfigSecPol.Style = MetroFramework.MetroColorStyle.Purple;
            this.metroTileConfigSecPol.TabIndex = 5;
            this.metroTileConfigSecPol.Text = "Set";
            this.metroTileConfigSecPol.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.metroTileConfigSecPol.UseSelectable = true;
            this.metroTileConfigSecPol.Visible = false;
            this.metroTileConfigSecPol.Click += new System.EventHandler(this.metroTileConfigSecPol_Click);
            // 
            // FormPreparation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 450);
            this.Controls.Add(this.metroTileConfigSecPol);
            this.Controls.Add(this.metroTileFinish);
            this.Controls.Add(this.metroTextBoxPreparationDetail);
            this.Controls.Add(this.metroTileStart);
            this.Controls.Add(this.metroLinkBack);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormPreparation";
            this.Resizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Server Preparation";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPreparation_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroLink metroLinkBack;
        private MetroFramework.Controls.MetroTile metroTileStart;
        private MetroFramework.Controls.MetroTextBox metroTextBoxPreparationDetail;
        private MetroFramework.Controls.MetroTile metroTileFinish;
        private MetroFramework.Controls.MetroTile metroTileConfigSecPol;
    }
}