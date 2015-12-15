//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
namespace TouchpadTest
{
    partial class MainForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.DoubleLbl = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.LeftLbl2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.RightLbl2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.RightLbl1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LeftLbl1 = new System.Windows.Forms.Label();
            this.ExitBtn = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.RightLbl2, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.RightLbl1, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.LeftLbl2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Title, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.LeftLbl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ExitBtn, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.DoubleLbl, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1216, 657);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(489, 167);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(237, 158);
            this.panel5.TabIndex = 1;
            this.panel5.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Mouse_DoubleClick);
            // 
            // DoubleLbl
            // 
            this.DoubleLbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DoubleLbl.AutoSize = true;
            this.DoubleLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoubleLbl.ForeColor = System.Drawing.Color.White;
            this.DoubleLbl.Location = new System.Drawing.Point(505, 328);
            this.DoubleLbl.Name = "DoubleLbl";
            this.DoubleLbl.Padding = new System.Windows.Forms.Padding(5, 10, 0, 0);
            this.DoubleLbl.Size = new System.Drawing.Size(205, 94);
            this.DoubleLbl.TabIndex = 0;
            this.DoubleLbl.Text = "DOUBLE  CLICK";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.GreenYellow;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 495);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(237, 159);
            this.panel4.TabIndex = 1;
            this.panel4.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Left_MouseClick);
            // 
            // LeftLbl2
            // 
            this.LeftLbl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LeftLbl2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.LeftLbl2, 2);
            this.LeftLbl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeftLbl2.ForeColor = System.Drawing.Color.White;
            this.LeftLbl2.Location = new System.Drawing.Point(0, 440);
            this.LeftLbl2.Margin = new System.Windows.Forms.Padding(0);
            this.LeftLbl2.Name = "LeftLbl2";
            this.LeftLbl2.Padding = new System.Windows.Forms.Padding(5, 10, 0, 0);
            this.LeftLbl2.Size = new System.Drawing.Size(244, 52);
            this.LeftLbl2.TabIndex = 1;
            this.LeftLbl2.Text = "LEFT CLICK";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Crimson;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(975, 495);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(238, 159);
            this.panel3.TabIndex = 1;
            this.panel3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Right_MouseClick);
            // 
            // RightLbl2
            // 
            this.RightLbl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RightLbl2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.RightLbl2, 2);
            this.RightLbl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RightLbl2.ForeColor = System.Drawing.Color.White;
            this.RightLbl2.Location = new System.Drawing.Point(948, 440);
            this.RightLbl2.Margin = new System.Windows.Forms.Padding(0);
            this.RightLbl2.Name = "RightLbl2";
            this.RightLbl2.Padding = new System.Windows.Forms.Padding(5, 10, 0, 0);
            this.RightLbl2.Size = new System.Drawing.Size(268, 52);
            this.RightLbl2.TabIndex = 3;
            this.RightLbl2.Text = "RIGHT CLICK";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Crimson;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(975, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(238, 158);
            this.panel2.TabIndex = 1;
            this.panel2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Right_MouseClick);
            // 
            // RightLbl1
            // 
            this.RightLbl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RightLbl1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.RightLbl1, 2);
            this.RightLbl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RightLbl1.ForeColor = System.Drawing.Color.White;
            this.RightLbl1.Location = new System.Drawing.Point(948, 164);
            this.RightLbl1.Margin = new System.Windows.Forms.Padding(0);
            this.RightLbl1.Name = "RightLbl1";
            this.RightLbl1.Padding = new System.Windows.Forms.Padding(5, 10, 0, 0);
            this.RightLbl1.Size = new System.Drawing.Size(268, 52);
            this.RightLbl1.TabIndex = 2;
            this.RightLbl1.Text = "RIGHT CLICK";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.GreenYellow;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(237, 158);
            this.panel1.TabIndex = 0;
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Left_MouseClick);
            // 
            // LeftLbl1
            // 
            this.LeftLbl1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.LeftLbl1, 2);
            this.LeftLbl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeftLbl1.ForeColor = System.Drawing.Color.White;
            this.LeftLbl1.Location = new System.Drawing.Point(0, 164);
            this.LeftLbl1.Margin = new System.Windows.Forms.Padding(0);
            this.LeftLbl1.Name = "LeftLbl1";
            this.LeftLbl1.Padding = new System.Windows.Forms.Padding(5, 10, 0, 0);
            this.LeftLbl1.Size = new System.Drawing.Size(244, 52);
            this.LeftLbl1.TabIndex = 0;
            this.LeftLbl1.Text = "LEFT CLICK";
            // 
            // ExitBtn
            // 
            this.ExitBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ExitBtn.BackColor = System.Drawing.Color.LightSlateGray;
            this.ExitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitBtn.ForeColor = System.Drawing.Color.White;
            this.ExitBtn.Location = new System.Drawing.Point(535, 495);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(144, 100);
            this.ExitBtn.TabIndex = 2;
            this.ExitBtn.Text = "EXIT";
            this.ExitBtn.UseVisualStyleBackColor = false;
            this.ExitBtn.Click += new System.EventHandler(this.ExitBtn_Click);
            // 
            // Title
            // 
            this.Title.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Title.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.Title, 3);
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.ForeColor = System.Drawing.Color.White;
            this.Title.Location = new System.Drawing.Point(370, 45);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(474, 73);
            this.Title.TabIndex = 1;
            this.Title.Text = "Touchpad Test";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1216, 657);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "TouchpadTest";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LeftLbl2;
        private System.Windows.Forms.Label RightLbl2;
        private System.Windows.Forms.Label RightLbl1;
        private System.Windows.Forms.Label LeftLbl1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label DoubleLbl;
        private System.Windows.Forms.Button ExitBtn;
        private System.Windows.Forms.Label Title;
    }
}

