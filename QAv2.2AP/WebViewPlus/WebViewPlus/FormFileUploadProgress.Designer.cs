namespace WebViewPlus
{
    partial class FormFileUploadProgress
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
            this.metroProgressBarUploadProgress = new System.Windows.Forms.ProgressBar();
            this.metroButtonOK = new System.Windows.Forms.Button();
            this.metroLabelUploadStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // metroProgressBarUploadProgress
            // 
            //this.metroProgressBarUploadProgress.HideProgressText = false;
            this.metroProgressBarUploadProgress.Location = new System.Drawing.Point(16, 73);
            this.metroProgressBarUploadProgress.Name = "metroProgressBarUploadProgress";
            this.metroProgressBarUploadProgress.Size = new System.Drawing.Size(748, 48);
            this.metroProgressBarUploadProgress.TabIndex = 0;
            //this.metroProgressBarUploadProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.metroLabelUploadStatus.AutoSize = true;
            this.metroLabelUploadStatus.Location = new System.Drawing.Point(17, 42);
            this.metroLabelUploadStatus.Name = "metroLabelUploadStatus";
            this.metroLabelUploadStatus.Size = new System.Drawing.Size(79, 19);
            this.metroLabelUploadStatus.TabIndex = 2;
            this.metroLabelUploadStatus.Text = "Uploading...";
            // 
            // FormFileUploadProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 194);
            this.Controls.Add(this.metroLabelUploadStatus);
            this.Controls.Add(this.metroButtonOK);
            this.Controls.Add(this.metroProgressBarUploadProgress);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFileUploadProgress";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFileUploadProgress_FormClosing);
            this.Load += new System.EventHandler(this.FormFileUploadProgress_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar metroProgressBarUploadProgress;
        private System.Windows.Forms.Button metroButtonOK;
        private System.Windows.Forms.Label metroLabelUploadStatus;
    }
}