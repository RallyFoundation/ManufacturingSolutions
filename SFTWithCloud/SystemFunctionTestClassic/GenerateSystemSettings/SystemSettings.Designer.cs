//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
namespace GenerateSystemSettings
{
    partial class SystemSettings
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
            this.SystemInfoConfig = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.CameraConfig = new System.Windows.Forms.TextBox();
            this.SystemInfoLbl = new System.Windows.Forms.Label();
            this.CameraInfoLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SystemInfoConfig
            // 
            this.SystemInfoConfig.BackColor = System.Drawing.Color.DimGray;
            this.SystemInfoConfig.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SystemInfoConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SystemInfoConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SystemInfoConfig.ForeColor = System.Drawing.Color.White;
            this.SystemInfoConfig.Location = new System.Drawing.Point(132, 113);
            this.SystemInfoConfig.Multiline = true;
            this.SystemInfoConfig.Name = "SystemInfoConfig";
            this.SystemInfoConfig.ReadOnly = true;
            this.SystemInfoConfig.Size = new System.Drawing.Size(598, 166);
            this.SystemInfoConfig.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Controls.Add(this.SystemInfoConfig, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.CameraConfig, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.SystemInfoLbl, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.CameraInfoLbl, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(863, 564);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // CameraConfig
            // 
            this.CameraConfig.BackColor = System.Drawing.Color.DimGray;
            this.CameraConfig.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CameraConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CameraConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CameraConfig.ForeColor = System.Drawing.Color.White;
            this.CameraConfig.Location = new System.Drawing.Point(132, 385);
            this.CameraConfig.Multiline = true;
            this.CameraConfig.Name = "CameraConfig";
            this.CameraConfig.ReadOnly = true;
            this.CameraConfig.Size = new System.Drawing.Size(598, 166);
            this.CameraConfig.TabIndex = 1;
            // 
            // SystemInfoLbl
            // 
            this.SystemInfoLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SystemInfoLbl.AutoSize = true;
            this.SystemInfoLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SystemInfoLbl.ForeColor = System.Drawing.Color.White;
            this.SystemInfoLbl.Location = new System.Drawing.Point(132, 71);
            this.SystemInfoLbl.Name = "SystemInfoLbl";
            this.SystemInfoLbl.Size = new System.Drawing.Size(201, 39);
            this.SystemInfoLbl.TabIndex = 2;
            this.SystemInfoLbl.Text = "System Info";
            // 
            // CameraInfoLbl
            // 
            this.CameraInfoLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CameraInfoLbl.AutoSize = true;
            this.CameraInfoLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CameraInfoLbl.ForeColor = System.Drawing.Color.White;
            this.CameraInfoLbl.Location = new System.Drawing.Point(132, 343);
            this.CameraInfoLbl.Name = "CameraInfoLbl";
            this.CameraInfoLbl.Size = new System.Drawing.Size(207, 39);
            this.CameraInfoLbl.TabIndex = 3;
            this.CameraInfoLbl.Text = "Camera Info";
            // 
            // SystemSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(863, 564);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SystemSettings";
            this.Text = "System Settings";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox SystemInfoConfig;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox CameraConfig;
        private System.Windows.Forms.Label SystemInfoLbl;
        private System.Windows.Forms.Label CameraInfoLbl;
    }
}

