namespace SuperRunPlus
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
            this.metroComboBoxOptions = new MetroFramework.Controls.MetroComboBox();
            this.metroButtonOK = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // metroComboBoxOptions
            // 
            this.metroComboBoxOptions.FormattingEnabled = true;
            this.metroComboBoxOptions.ItemHeight = 23;
            this.metroComboBoxOptions.Location = new System.Drawing.Point(24, 37);
            this.metroComboBoxOptions.Name = "metroComboBoxOptions";
            this.metroComboBoxOptions.Size = new System.Drawing.Size(563, 29);
            this.metroComboBoxOptions.TabIndex = 0;
            // 
            // metroButtonOK
            // 
            this.metroButtonOK.Location = new System.Drawing.Point(231, 90);
            this.metroButtonOK.Name = "metroButtonOK";
            this.metroButtonOK.Size = new System.Drawing.Size(150, 36);
            this.metroButtonOK.TabIndex = 1;
            this.metroButtonOK.Text = "OK";
            this.metroButtonOK.Click += new System.EventHandler(this.metroButtonOK_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 139);
            this.Controls.Add(this.metroButtonOK);
            this.Controls.Add(this.metroComboBoxOptions);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Resizable = false;
            this.ShowIcon = false;
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroComboBox metroComboBoxOptions;
        private MetroFramework.Controls.MetroButton metroButtonOK;
    }
}

