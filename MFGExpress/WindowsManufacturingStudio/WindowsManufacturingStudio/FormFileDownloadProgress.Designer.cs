namespace WindowsManufacturingStudio
{
    partial class FormFileDownloadProgress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFileUploadProgress));
            this.metroProgressBarDownloadProgress = new MetroFramework.Controls.MetroProgressBar();
            this.metroButtonOK = new MetroFramework.Controls.MetroButton();
            this.metroLabelDownloadStatus = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroProgressBarUploadProgress
            // 
            this.metroProgressBarDownloadProgress.HideProgressText = false;
            this.metroProgressBarDownloadProgress.Location = new System.Drawing.Point(16, 73);
            this.metroProgressBarDownloadProgress.Name = "metroProgressBarDownloadProgress";
            this.metroProgressBarDownloadProgress.Size = new System.Drawing.Size(748, 48);
            this.metroProgressBarDownloadProgress.TabIndex = 0;
            this.metroProgressBarDownloadProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // metroButtonOK
            // 
            this.metroButtonOK.Enabled = false;
            this.metroButtonOK.Location = new System.Drawing.Point(316, 146);
            this.metroButtonOK.Name = "metroButtonOK";
            this.metroButtonOK.Size = new System.Drawing.Size(120, 36);
            this.metroButtonOK.TabIndex = 1;
            this.metroButtonOK.Text = "OK";
            this.metroButtonOK.Click += new System.EventHandler(this.metroButtonOK_Click);
            // 
            // metroLabelUploadStatus
            // 
            this.metroLabelDownloadStatus.AutoSize = true;
            this.metroLabelDownloadStatus.Location = new System.Drawing.Point(17, 42);
            this.metroLabelDownloadStatus.Name = "metroLabelDownloadStatus";
            this.metroLabelDownloadStatus.Size = new System.Drawing.Size(79, 19);
            this.metroLabelDownloadStatus.TabIndex = 2;
            this.metroLabelDownloadStatus.Text = "Downloading...";
            // 
            // FormFileUploadProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 194);
            this.Controls.Add(this.metroLabelDownloadStatus);
            this.Controls.Add(this.metroButtonOK);
            this.Controls.Add(this.metroProgressBarDownloadProgress);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFileDownloadProgress";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFileUploadProgress_FormClosing);
            this.Load += new System.EventHandler(this.FormFileUploadProgress_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroProgressBar metroProgressBarDownloadProgress;
        private MetroFramework.Controls.MetroButton metroButtonOK;
        private MetroFramework.Controls.MetroLabel metroLabelDownloadStatus;
    }
}