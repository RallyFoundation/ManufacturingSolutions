namespace MfgSolutionsDeploymentCenter
{
    partial class FormConfirmPassword
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
            this.metroLabelDescription = new MetroFramework.Controls.MetroLabel();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.metroToggleShowPassword = new MetroFramework.Controls.MetroToggle();
            this.metroLabelShowPassword = new MetroFramework.Controls.MetroLabel();
            this.metroButtonOK = new MetroFramework.Controls.MetroButton();
            this.metroLabelWarning = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroLabelDescription
            // 
            this.metroLabelDescription.AutoSize = true;
            this.metroLabelDescription.Location = new System.Drawing.Point(20, 58);
            this.metroLabelDescription.Name = "metroLabelDescription";
            this.metroLabelDescription.Size = new System.Drawing.Size(285, 19);
            this.metroLabelDescription.TabIndex = 0;
            this.metroLabelDescription.Text = "Supply the password of your Windows account ";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(23, 86);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(404, 20);
            this.textBoxPassword.TabIndex = 1;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // metroToggleShowPassword
            // 
            this.metroToggleShowPassword.AutoSize = true;
            this.metroToggleShowPassword.Location = new System.Drawing.Point(137, 112);
            this.metroToggleShowPassword.Name = "metroToggleShowPassword";
            this.metroToggleShowPassword.Size = new System.Drawing.Size(80, 17);
            this.metroToggleShowPassword.TabIndex = 2;
            this.metroToggleShowPassword.Text = "Off";
            this.metroToggleShowPassword.UseSelectable = true;
            this.metroToggleShowPassword.CheckedChanged += new System.EventHandler(this.metroToggleShowPassword_CheckedChanged);
            // 
            // metroLabelShowPassword
            // 
            this.metroLabelShowPassword.AutoSize = true;
            this.metroLabelShowPassword.Location = new System.Drawing.Point(20, 112);
            this.metroLabelShowPassword.Name = "metroLabelShowPassword";
            this.metroLabelShowPassword.Size = new System.Drawing.Size(111, 19);
            this.metroLabelShowPassword.TabIndex = 3;
            this.metroLabelShowPassword.Text = "Display Password:";
            // 
            // metroButtonOK
            // 
            this.metroButtonOK.Location = new System.Drawing.Point(352, 142);
            this.metroButtonOK.Name = "metroButtonOK";
            this.metroButtonOK.Size = new System.Drawing.Size(75, 23);
            this.metroButtonOK.TabIndex = 4;
            this.metroButtonOK.Text = "OK";
            this.metroButtonOK.UseSelectable = true;
            this.metroButtonOK.Click += new System.EventHandler(this.metroButtonOK_Click);
            // 
            // metroLabelWarning
            // 
            this.metroLabelWarning.AutoSize = true;
            this.metroLabelWarning.ForeColor = System.Drawing.Color.Red;
            this.metroLabelWarning.Location = new System.Drawing.Point(20, 142);
            this.metroLabelWarning.Name = "metroLabelWarning";
            this.metroLabelWarning.Size = new System.Drawing.Size(122, 19);
            this.metroLabelWarning.TabIndex = 5;
            this.metroLabelWarning.Text = "Incorrect Password!";
            this.metroLabelWarning.UseStyleColors = true;
            this.metroLabelWarning.Visible = false;
            // 
            // FormConfirmPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 180);
            this.Controls.Add(this.metroLabelWarning);
            this.Controls.Add(this.metroButtonOK);
            this.Controls.Add(this.metroLabelShowPassword);
            this.Controls.Add(this.metroToggleShowPassword);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.metroLabelDescription);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfirmPassword";
            this.Resizable = false;
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Your Windows Password Needed";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabelDescription;
        private System.Windows.Forms.TextBox textBoxPassword;
        private MetroFramework.Controls.MetroToggle metroToggleShowPassword;
        private MetroFramework.Controls.MetroLabel metroLabelShowPassword;
        private MetroFramework.Controls.MetroButton metroButtonOK;
        private MetroFramework.Controls.MetroLabel metroLabelWarning;
    }
}