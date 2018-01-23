namespace OA3ToolConfGen
{
    partial class UCParameter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBoxSelection = new System.Windows.Forms.CheckBox();
            this.labelParameterName = new System.Windows.Forms.Label();
            this.comboBoxParameterValue = new System.Windows.Forms.ComboBox();
            this.buttonGet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBoxSelection
            // 
            this.checkBoxSelection.AutoSize = true;
            this.checkBoxSelection.Location = new System.Drawing.Point(346, 7);
            this.checkBoxSelection.Name = "checkBoxSelection";
            this.checkBoxSelection.Size = new System.Drawing.Size(15, 14);
            this.checkBoxSelection.TabIndex = 0;
            this.checkBoxSelection.UseVisualStyleBackColor = true;
            this.checkBoxSelection.CheckedChanged += new System.EventHandler(this.checkBoxSelection_CheckedChanged);
            // 
            // labelParameterName
            // 
            this.labelParameterName.Location = new System.Drawing.Point(3, 7);
            this.labelParameterName.Name = "labelParameterName";
            this.labelParameterName.Size = new System.Drawing.Size(148, 13);
            this.labelParameterName.TabIndex = 1;
            this.labelParameterName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxParameterValue
            // 
            this.comboBoxParameterValue.Enabled = false;
            this.comboBoxParameterValue.FormattingEnabled = true;
            this.comboBoxParameterValue.Location = new System.Drawing.Point(158, 3);
            this.comboBoxParameterValue.Name = "comboBoxParameterValue";
            this.comboBoxParameterValue.Size = new System.Drawing.Size(141, 21);
            this.comboBoxParameterValue.TabIndex = 4;
            // 
            // buttonGet
            // 
            this.buttonGet.Enabled = false;
            this.buttonGet.Location = new System.Drawing.Point(305, 1);
            this.buttonGet.Name = "buttonGet";
            this.buttonGet.Size = new System.Drawing.Size(36, 23);
            this.buttonGet.TabIndex = 5;
            this.buttonGet.Text = "Get";
            this.buttonGet.UseVisualStyleBackColor = true;
            this.buttonGet.Click += new System.EventHandler(this.buttonGet_Click);
            // 
            // UCParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonGet);
            this.Controls.Add(this.comboBoxParameterValue);
            this.Controls.Add(this.labelParameterName);
            this.Controls.Add(this.checkBoxSelection);
            this.Name = "UCParameter";
            this.Size = new System.Drawing.Size(370, 31);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxSelection;
        private System.Windows.Forms.Label labelParameterName;
        private System.Windows.Forms.ComboBox comboBoxParameterValue;
        private System.Windows.Forms.Button buttonGet;
    }
}
