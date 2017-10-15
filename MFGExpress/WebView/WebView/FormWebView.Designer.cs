namespace WebView
{
    using System.Security.Permissions;

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    partial class FormWebView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWebView));
            this.webBrowserWebView = new System.Windows.Forms.WebBrowser();
            this.saveFileDialogSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // webBrowserWebView
            // 
            this.webBrowserWebView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserWebView.Location = new System.Drawing.Point(0, 0);
            this.webBrowserWebView.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserWebView.Name = "webBrowserWebView";
            this.webBrowserWebView.Size = new System.Drawing.Size(800, 600);
            this.webBrowserWebView.TabIndex = 0;
            this.webBrowserWebView.FileDownload += new System.EventHandler(this.webBrowserReport_FileDownload);
            this.webBrowserWebView.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowserReport_Navigating);
            // 
            // saveFileDialogSaveFile
            // 
            this.saveFileDialogSaveFile.Filter = "HTML Files (*.html)|*.html|HTML Files (*.htm)|*.htm|All Files (*.*)|*.*";
            // 
            // FormWebView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.webBrowserWebView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormWebView";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWebView_FormClosing);
            this.Load += new System.EventHandler(this.FormWebView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowserWebView;
        private System.Windows.Forms.SaveFileDialog saveFileDialogSaveFile;
    }
}