namespace PowerShellDataProcessing
{
    partial class FormGetInputPath
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
            this.metroTextBoxInputPath = new System.Windows.Forms.TextBox();
            this.metroButtonBrowse = new System.Windows.Forms.Button();
            this.metroButtonOK = new System.Windows.Forms.Button();
            this.metroLabelMessage = new System.Windows.Forms.Label();
            this.openFileDialogInputPath = new System.Windows.Forms.OpenFileDialog();
            this.metroButtonCancelExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // metroTextBoxInputPath
            // 
            this.metroTextBoxInputPath.BackColor = System.Drawing.SystemColors.Window;
            this.metroTextBoxInputPath.Location = new System.Drawing.Point(11, 56);
            this.metroTextBoxInputPath.Multiline = true;
            this.metroTextBoxInputPath.Name = "metroTextBoxInputPath";
            this.metroTextBoxInputPath.ReadOnly = true;
            this.metroTextBoxInputPath.Size = new System.Drawing.Size(480, 45);
            this.metroTextBoxInputPath.TabIndex = 0;
            // 
            // metroButtonBrowse
            // 
            this.metroButtonBrowse.Location = new System.Drawing.Point(501, 55);
            this.metroButtonBrowse.Name = "metroButtonBrowse";
            this.metroButtonBrowse.Size = new System.Drawing.Size(129, 45);
            this.metroButtonBrowse.TabIndex = 1;
            this.metroButtonBrowse.Text = "Browse..";
            this.metroButtonBrowse.Click += new System.EventHandler(this.metroButtonBrowse_Click);
            // 
            // metroButtonOK
            // 
            this.metroButtonOK.Location = new System.Drawing.Point(329, 139);
            this.metroButtonOK.Name = "metroButtonOK";
            this.metroButtonOK.Size = new System.Drawing.Size(129, 45);
            this.metroButtonOK.TabIndex = 2;
            this.metroButtonOK.Text = "OK";
            this.metroButtonOK.Click += new System.EventHandler(this.metroButtonOK_Click);
            // 
            // metroLabelMessage
            // 
            this.metroLabelMessage.AutoSize = true;
            this.metroLabelMessage.Location = new System.Drawing.Point(24, 19);
            this.metroLabelMessage.Name = "metroLabelMessage";
            this.metroLabelMessage.Size = new System.Drawing.Size(179, 13);
            this.metroLabelMessage.TabIndex = 3;
            this.metroLabelMessage.Text = "Click \"Browse\" to specify input path:";
            // 
            // openFileDialogInputPath
            // 
            this.openFileDialogInputPath.FileName = "openFileDialogInputPath";
            this.openFileDialogInputPath.Filter = "XML files(*.xml)|*.xml|All files(*.*)|*.*";
            // 
            // metroButtonCancelExit
            // 
            this.metroButtonCancelExit.Location = new System.Drawing.Point(161, 139);
            this.metroButtonCancelExit.Name = "metroButtonCancelExit";
            this.metroButtonCancelExit.Size = new System.Drawing.Size(129, 45);
            this.metroButtonCancelExit.TabIndex = 4;
            this.metroButtonCancelExit.Text = "Cancel";
            this.metroButtonCancelExit.Click += new System.EventHandler(this.metroButtonCancelExit_Click);
            // 
            // FormGetInputPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(640, 202);
            this.Controls.Add(this.metroButtonCancelExit);
            this.Controls.Add(this.metroLabelMessage);
            this.Controls.Add(this.metroButtonOK);
            this.Controls.Add(this.metroButtonBrowse);
            this.Controls.Add(this.metroTextBoxInputPath);
            this.MaximizeBox = false;
            this.Name = "FormGetInputPath";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Specify Input Path";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox metroTextBoxInputPath;
        private System.Windows.Forms.Button metroButtonBrowse;
        private System.Windows.Forms.Button metroButtonOK;
        private System.Windows.Forms.Label metroLabelMessage;
        private System.Windows.Forms.OpenFileDialog openFileDialogInputPath;
        private System.Windows.Forms.Button metroButtonCancelExit;
    }
}