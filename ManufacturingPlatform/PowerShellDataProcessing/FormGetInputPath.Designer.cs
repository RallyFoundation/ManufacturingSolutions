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
            this.metroTextBoxInputPath = new MetroFramework.Controls.MetroTextBox();
            this.metroButtonBrowse = new MetroFramework.Controls.MetroButton();
            this.metroButtonOK = new MetroFramework.Controls.MetroButton();
            this.metroLabelMessage = new MetroFramework.Controls.MetroLabel();
            this.openFileDialogInputPath = new System.Windows.Forms.OpenFileDialog();
            this.metroButtonCancelExit = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // metroTextBoxInputPath
            // 
            this.metroTextBoxInputPath.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.metroTextBoxInputPath.Location = new System.Drawing.Point(11, 106);
            this.metroTextBoxInputPath.Name = "metroTextBoxInputPath";
            this.metroTextBoxInputPath.ReadOnly = true;
            this.metroTextBoxInputPath.Size = new System.Drawing.Size(480, 64);
            this.metroTextBoxInputPath.TabIndex = 0;
            // 
            // metroButtonBrowse
            // 
            this.metroButtonBrowse.Location = new System.Drawing.Point(500, 106);
            this.metroButtonBrowse.Name = "metroButtonBrowse";
            this.metroButtonBrowse.Size = new System.Drawing.Size(129, 64);
            this.metroButtonBrowse.TabIndex = 1;
            this.metroButtonBrowse.Text = "Browse..";
            this.metroButtonBrowse.Click += new System.EventHandler(this.metroButtonBrowse_Click);
            // 
            // metroButtonOK
            // 
            this.metroButtonOK.Location = new System.Drawing.Point(311, 186);
            this.metroButtonOK.Name = "metroButtonOK";
            this.metroButtonOK.Size = new System.Drawing.Size(180, 64);
            this.metroButtonOK.TabIndex = 2;
            this.metroButtonOK.Text = "OK";
            this.metroButtonOK.Click += new System.EventHandler(this.metroButtonOK_Click);
            // 
            // metroLabelMessage
            // 
            this.metroLabelMessage.AutoSize = true;
            this.metroLabelMessage.Location = new System.Drawing.Point(24, 65);
            this.metroLabelMessage.Name = "metroLabelMessage";
            this.metroLabelMessage.Size = new System.Drawing.Size(216, 19);
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
            this.metroButtonCancelExit.Location = new System.Drawing.Point(114, 186);
            this.metroButtonCancelExit.Name = "metroButtonCancelExit";
            this.metroButtonCancelExit.Size = new System.Drawing.Size(180, 64);
            this.metroButtonCancelExit.TabIndex = 4;
            this.metroButtonCancelExit.Text = "Cancel";
            this.metroButtonCancelExit.Click += new System.EventHandler(this.metroButtonCancelExit_Click);
            // 
            // FormGetInputPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(640, 260);
            this.Controls.Add(this.metroButtonCancelExit);
            this.Controls.Add(this.metroLabelMessage);
            this.Controls.Add(this.metroButtonOK);
            this.Controls.Add(this.metroButtonBrowse);
            this.Controls.Add(this.metroTextBoxInputPath);
            this.MaximizeBox = false;
            this.Name = "FormGetInputPath";
            this.Resizable = false;
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Specify Input Path";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTextBox metroTextBoxInputPath;
        private MetroFramework.Controls.MetroButton metroButtonBrowse;
        private MetroFramework.Controls.MetroButton metroButtonOK;
        private MetroFramework.Controls.MetroLabel metroLabelMessage;
        private System.Windows.Forms.OpenFileDialog openFileDialogInputPath;
        private MetroFramework.Controls.MetroButton metroButtonCancelExit;
    }
}