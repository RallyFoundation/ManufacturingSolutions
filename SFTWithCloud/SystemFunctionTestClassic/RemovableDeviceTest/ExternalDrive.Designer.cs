namespace RemovableDeviceTest
{
    partial class ExternalDrive
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
            this.DriveLbl2 = new System.Windows.Forms.Label();
            this.DriveLbl1 = new System.Windows.Forms.Label();
            this.DriveType = new System.Windows.Forms.Label();
            this.DriveTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // DriveLbl2
            // 
            this.DriveLbl2.AutoSize = true;
            this.DriveLbl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DriveLbl2.Location = new System.Drawing.Point(35, 156);
            this.DriveLbl2.Name = "DriveLbl2";
            this.DriveLbl2.Size = new System.Drawing.Size(106, 39);
            this.DriveLbl2.TabIndex = 10;
            this.DriveLbl2.Text = "Temp";
            // 
            // DriveLbl1
            // 
            this.DriveLbl1.AutoSize = true;
            this.DriveLbl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DriveLbl1.Location = new System.Drawing.Point(35, 114);
            this.DriveLbl1.Name = "DriveLbl1";
            this.DriveLbl1.Size = new System.Drawing.Size(106, 39);
            this.DriveLbl1.TabIndex = 9;
            this.DriveLbl1.Text = "Temp";
            // 
            // DriveType
            // 
            this.DriveType.AutoSize = true;
            this.DriveType.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DriveType.Location = new System.Drawing.Point(35, 72);
            this.DriveType.Name = "DriveType";
            this.DriveType.Size = new System.Drawing.Size(106, 39);
            this.DriveType.TabIndex = 7;
            this.DriveType.Text = "Temp";
            // 
            // DriveTitle
            // 
            this.DriveTitle.AutoSize = true;
            this.DriveTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DriveTitle.ForeColor = System.Drawing.Color.Turquoise;
            this.DriveTitle.Location = new System.Drawing.Point(16, 14);
            this.DriveTitle.Name = "DriveTitle";
            this.DriveTitle.Size = new System.Drawing.Size(106, 39);
            this.DriveTitle.TabIndex = 6;
            this.DriveTitle.Text = "Temp";
            // 
            // ExternalDrive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.DriveLbl2);
            this.Controls.Add(this.DriveLbl1);
            this.Controls.Add(this.DriveType);
            this.Controls.Add(this.DriveTitle);
            this.Name = "ExternalDrive";
            this.Size = new System.Drawing.Size(165, 212);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DriveLbl2;
        private System.Windows.Forms.Label DriveLbl1;
        private System.Windows.Forms.Label DriveType;
        private System.Windows.Forms.Label DriveTitle;
    }
}
