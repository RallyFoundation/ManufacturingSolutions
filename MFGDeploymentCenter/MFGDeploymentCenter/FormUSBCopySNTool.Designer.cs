namespace MfgSolutionsDeploymentCenter
{
    partial class FormUSBCopySNTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUSBCopySNTool));
            this.metroLinkBack = new MetroFramework.Controls.MetroLink();
            this.metroComboBoxRemovableDisks = new MetroFramework.Controls.MetroComboBox();
            this.metroButtonBeginCopy = new MetroFramework.Controls.MetroButton();
            this.metroLabelChoose = new MetroFramework.Controls.MetroLabel();
            this.metroProgressBarCopyProgress = new MetroFramework.Controls.MetroProgressBar();
            this.metroLabelCurrentItem = new MetroFramework.Controls.MetroLabel();
            this.metroLabelFirmwareVendors = new MetroFramework.Controls.MetroLabel();
            this.metroComboBoxFirmwareVendors = new MetroFramework.Controls.MetroComboBox();
            this.SuspendLayout();
            // 
            // metroLinkBack
            // 
            this.metroLinkBack.BackgroundImage = global::MfgSolutionsDeploymentCenter.Properties.Resources.MB_0006_back;
            this.metroLinkBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.metroLinkBack.Location = new System.Drawing.Point(20, 60);
            this.metroLinkBack.Name = "metroLinkBack";
            this.metroLinkBack.Size = new System.Drawing.Size(45, 45);
            this.metroLinkBack.TabIndex = 3;
            this.metroLinkBack.UseSelectable = true;
            this.metroLinkBack.Click += new System.EventHandler(this.metroLinkBack_Click);
            // 
            // metroComboBoxRemovableDisks
            // 
            this.metroComboBoxRemovableDisks.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroComboBoxRemovableDisks.FormattingEnabled = true;
            this.metroComboBoxRemovableDisks.ItemHeight = 23;
            this.metroComboBoxRemovableDisks.Location = new System.Drawing.Point(158, 221);
            this.metroComboBoxRemovableDisks.Name = "metroComboBoxRemovableDisks";
            this.metroComboBoxRemovableDisks.Size = new System.Drawing.Size(417, 29);
            this.metroComboBoxRemovableDisks.TabIndex = 4;
            this.metroComboBoxRemovableDisks.UseSelectable = true;
            // 
            // metroButtonBeginCopy
            // 
            this.metroButtonBeginCopy.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroButtonBeginCopy.Location = new System.Drawing.Point(581, 221);
            this.metroButtonBeginCopy.Name = "metroButtonBeginCopy";
            this.metroButtonBeginCopy.Size = new System.Drawing.Size(86, 29);
            this.metroButtonBeginCopy.TabIndex = 5;
            this.metroButtonBeginCopy.Text = "Deploy";
            this.metroButtonBeginCopy.UseSelectable = true;
            this.metroButtonBeginCopy.Click += new System.EventHandler(this.metroButtonBeginCopy_Click);
            // 
            // metroLabelChoose
            // 
            this.metroLabelChoose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroLabelChoose.AutoSize = true;
            this.metroLabelChoose.Location = new System.Drawing.Point(158, 183);
            this.metroLabelChoose.Name = "metroLabelChoose";
            this.metroLabelChoose.Size = new System.Drawing.Size(160, 19);
            this.metroLabelChoose.TabIndex = 6;
            this.metroLabelChoose.Text = "Choose a removable disk:";
            // 
            // metroProgressBarCopyProgress
            // 
            this.metroProgressBarCopyProgress.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroProgressBarCopyProgress.Location = new System.Drawing.Point(158, 276);
            this.metroProgressBarCopyProgress.Name = "metroProgressBarCopyProgress";
            this.metroProgressBarCopyProgress.Size = new System.Drawing.Size(509, 26);
            this.metroProgressBarCopyProgress.Step = 1;
            this.metroProgressBarCopyProgress.TabIndex = 7;
            this.metroProgressBarCopyProgress.Visible = false;
            // 
            // metroLabelCurrentItem
            // 
            this.metroLabelCurrentItem.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroLabelCurrentItem.AutoSize = true;
            this.metroLabelCurrentItem.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabelCurrentItem.Location = new System.Drawing.Point(158, 321);
            this.metroLabelCurrentItem.Name = "metroLabelCurrentItem";
            this.metroLabelCurrentItem.Size = new System.Drawing.Size(0, 0);
            this.metroLabelCurrentItem.TabIndex = 8;
            this.metroLabelCurrentItem.Visible = false;
            this.metroLabelCurrentItem.WrapToLine = true;
            // 
            // metroLabelFirmwareVendors
            // 
            this.metroLabelFirmwareVendors.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroLabelFirmwareVendors.AutoSize = true;
            this.metroLabelFirmwareVendors.Location = new System.Drawing.Point(158, 103);
            this.metroLabelFirmwareVendors.Name = "metroLabelFirmwareVendors";
            this.metroLabelFirmwareVendors.Size = new System.Drawing.Size(169, 19);
            this.metroLabelFirmwareVendors.TabIndex = 9;
            this.metroLabelFirmwareVendors.Text = "Choose a firmware vendor:";
            // 
            // metroComboBoxFirmwareVendors
            // 
            this.metroComboBoxFirmwareVendors.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroComboBoxFirmwareVendors.FormattingEnabled = true;
            this.metroComboBoxFirmwareVendors.ItemHeight = 23;
            this.metroComboBoxFirmwareVendors.Location = new System.Drawing.Point(158, 137);
            this.metroComboBoxFirmwareVendors.Name = "metroComboBoxFirmwareVendors";
            this.metroComboBoxFirmwareVendors.Size = new System.Drawing.Size(509, 29);
            this.metroComboBoxFirmwareVendors.TabIndex = 10;
            this.metroComboBoxFirmwareVendors.UseSelectable = true;
            // 
            // FormUSBCopySNTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 450);
            this.Controls.Add(this.metroComboBoxFirmwareVendors);
            this.Controls.Add(this.metroLabelFirmwareVendors);
            this.Controls.Add(this.metroLabelCurrentItem);
            this.Controls.Add(this.metroProgressBarCopyProgress);
            this.Controls.Add(this.metroLabelChoose);
            this.Controls.Add(this.metroButtonBeginCopy);
            this.Controls.Add(this.metroComboBoxRemovableDisks);
            this.Controls.Add(this.metroLinkBack);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormUSBCopySNTool";
            this.Resizable = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUSBCopy_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLink metroLinkBack;
        private MetroFramework.Controls.MetroComboBox metroComboBoxRemovableDisks;
        private MetroFramework.Controls.MetroButton metroButtonBeginCopy;
        private MetroFramework.Controls.MetroLabel metroLabelChoose;
        private MetroFramework.Controls.MetroProgressBar metroProgressBarCopyProgress;
        private MetroFramework.Controls.MetroLabel metroLabelCurrentItem;
        private MetroFramework.Controls.MetroLabel metroLabelFirmwareVendors;
        private MetroFramework.Controls.MetroComboBox metroComboBoxFirmwareVendors;
    }
}