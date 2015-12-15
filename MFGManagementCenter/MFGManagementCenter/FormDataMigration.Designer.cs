namespace DISManagementCenter
{
    partial class FormDataMigration
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDataMigration));
            this.metroLinkBack = new MetroFramework.Controls.MetroLink();
            this.metroLabelDataPackage = new MetroFramework.Controls.MetroLabel();
            this.metroTextBoxDataPackage = new MetroFramework.Controls.MetroTextBox();
            this.metroButtonBrowse = new MetroFramework.Controls.MetroButton();
            this.metroGridDataPreview = new MetroFramework.Controls.MetroGrid();
            this.metroButtonContinue = new MetroFramework.Controls.MetroButton();
            this.folderBrowserDialogDataPackage = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.metroGridDataPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // metroLinkBack
            // 
            this.metroLinkBack.BackgroundImage = global::MFGManagementCenter.Properties.Resources.MB_0006_back;
            this.metroLinkBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.metroLinkBack.Location = new System.Drawing.Point(24, 64);
            this.metroLinkBack.Name = "metroLinkBack";
            this.metroLinkBack.Size = new System.Drawing.Size(46, 46);
            this.metroLinkBack.TabIndex = 0;
            this.metroLinkBack.UseSelectable = true;
            this.metroLinkBack.Click += new System.EventHandler(this.metroLinkBack_Click);
            // 
            // metroLabelDataPackage
            // 
            this.metroLabelDataPackage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroLabelDataPackage.AutoSize = true;
            this.metroLabelDataPackage.Location = new System.Drawing.Point(57, 125);
            this.metroLabelDataPackage.Name = "metroLabelDataPackage";
            this.metroLabelDataPackage.Size = new System.Drawing.Size(91, 19);
            this.metroLabelDataPackage.TabIndex = 1;
            this.metroLabelDataPackage.Text = "Data Package:";
            // 
            // metroTextBoxDataPackage
            // 
            this.metroTextBoxDataPackage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.metroTextBoxDataPackage.Lines = new string[0];
            this.metroTextBoxDataPackage.Location = new System.Drawing.Point(149, 125);
            this.metroTextBoxDataPackage.MaxLength = 32767;
            this.metroTextBoxDataPackage.Name = "metroTextBoxDataPackage";
            this.metroTextBoxDataPackage.PasswordChar = '\0';
            this.metroTextBoxDataPackage.ReadOnly = true;
            this.metroTextBoxDataPackage.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxDataPackage.SelectedText = "";
            this.metroTextBoxDataPackage.Size = new System.Drawing.Size(548, 23);
            this.metroTextBoxDataPackage.TabIndex = 2;
            this.metroTextBoxDataPackage.UseSelectable = true;
            // 
            // metroButtonBrowse
            // 
            this.metroButtonBrowse.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroButtonBrowse.Location = new System.Drawing.Point(703, 125);
            this.metroButtonBrowse.Name = "metroButtonBrowse";
            this.metroButtonBrowse.Size = new System.Drawing.Size(75, 23);
            this.metroButtonBrowse.TabIndex = 3;
            this.metroButtonBrowse.Text = "Browse";
            this.metroButtonBrowse.UseSelectable = true;
            this.metroButtonBrowse.Click += new System.EventHandler(this.metroButtonBrowse_Click);
            // 
            // metroGridDataPreview
            // 
            this.metroGridDataPreview.AllowUserToAddRows = false;
            this.metroGridDataPreview.AllowUserToDeleteRows = false;
            this.metroGridDataPreview.AllowUserToResizeRows = false;
            this.metroGridDataPreview.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroGridDataPreview.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroGridDataPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.metroGridDataPreview.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.metroGridDataPreview.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metroGridDataPreview.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.metroGridDataPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.metroGridDataPreview.DefaultCellStyle = dataGridViewCellStyle2;
            this.metroGridDataPreview.EnableHeadersVisualStyles = false;
            this.metroGridDataPreview.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.metroGridDataPreview.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroGridDataPreview.Location = new System.Drawing.Point(57, 154);
            this.metroGridDataPreview.Name = "metroGridDataPreview";
            this.metroGridDataPreview.ReadOnly = true;
            this.metroGridDataPreview.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metroGridDataPreview.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.metroGridDataPreview.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.metroGridDataPreview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.metroGridDataPreview.Size = new System.Drawing.Size(721, 371);
            this.metroGridDataPreview.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroGridDataPreview.TabIndex = 4;
            this.metroGridDataPreview.UseStyleColors = true;
            // 
            // metroButtonContinue
            // 
            this.metroButtonContinue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroButtonContinue.Location = new System.Drawing.Point(703, 541);
            this.metroButtonContinue.Name = "metroButtonContinue";
            this.metroButtonContinue.Size = new System.Drawing.Size(75, 23);
            this.metroButtonContinue.TabIndex = 5;
            this.metroButtonContinue.Text = "Continue";
            this.metroButtonContinue.UseSelectable = true;
            // 
            // folderBrowserDialogDataPackage
            // 
            this.folderBrowserDialogDataPackage.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialogDataPackage.ShowNewFolderButton = false;
            // 
            // FormDataMigration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.metroButtonContinue);
            this.Controls.Add(this.metroGridDataPreview);
            this.Controls.Add(this.metroButtonBrowse);
            this.Controls.Add(this.metroTextBoxDataPackage);
            this.Controls.Add(this.metroLabelDataPackage);
            this.Controls.Add(this.metroLinkBack);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDataMigration";
            this.Text = "Data Migration";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDataMigration_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.metroGridDataPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLink metroLinkBack;
        private MetroFramework.Controls.MetroLabel metroLabelDataPackage;
        private MetroFramework.Controls.MetroTextBox metroTextBoxDataPackage;
        private MetroFramework.Controls.MetroButton metroButtonBrowse;
        private MetroFramework.Controls.MetroGrid metroGridDataPreview;
        private MetroFramework.Controls.MetroButton metroButtonContinue;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogDataPackage;
    }
}