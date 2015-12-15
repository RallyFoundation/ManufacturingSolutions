//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
namespace AudioTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ResetBtn = new System.Windows.Forms.Button();
            this.PassBtn = new System.Windows.Forms.Button();
            this.FailBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.RecordBtn = new System.Windows.Forms.Button();
            this.NextBtn = new System.Windows.Forms.Button();
            this.RecordingNowLbl = new System.Windows.Forms.Label();
            this.PlayBtn = new System.Windows.Forms.Button();
            this.RecordingLbl = new System.Windows.Forms.Label();
            this.PlayAudioLbl = new System.Windows.Forms.Label();
            this.InstructionLbl = new System.Windows.Forms.Label();
            this.Title = new System.Windows.Forms.Label();
            this.Player1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.tableLayoutPanel1.SuspendLayout();
            this.ButtonLayoutPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Player1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Controls.Add(this.ButtonLayoutPanel, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(987, 624);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // ButtonLayoutPanel
            // 
            this.ButtonLayoutPanel.ColumnCount = 3;
            this.ButtonLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ButtonLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.ButtonLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.ButtonLayoutPanel.Controls.Add(this.ResetBtn, 2, 0);
            this.ButtonLayoutPanel.Controls.Add(this.PassBtn, 0, 0);
            this.ButtonLayoutPanel.Controls.Add(this.FailBtn, 1, 0);
            this.ButtonLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonLayoutPanel.Location = new System.Drawing.Point(101, 501);
            this.ButtonLayoutPanel.Name = "ButtonLayoutPanel";
            this.ButtonLayoutPanel.RowCount = 2;
            this.ButtonLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.ButtonLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.ButtonLayoutPanel.Size = new System.Drawing.Size(783, 120);
            this.ButtonLayoutPanel.TabIndex = 8;
            // 
            // ResetBtn
            // 
            this.ResetBtn.BackColor = System.Drawing.Color.Gray;
            this.ResetBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResetBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetBtn.ForeColor = System.Drawing.Color.Transparent;
            this.ResetBtn.Location = new System.Drawing.Point(531, 10);
            this.ResetBtn.Margin = new System.Windows.Forms.Padding(10);
            this.ResetBtn.Name = "ResetBtn";
            this.ResetBtn.Size = new System.Drawing.Size(242, 88);
            this.ResetBtn.TabIndex = 7;
            this.ResetBtn.Text = "Retry";
            this.ResetBtn.UseVisualStyleBackColor = false;
            this.ResetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
            // 
            // PassBtn
            // 
            this.PassBtn.BackColor = System.Drawing.Color.YellowGreen;
            this.PassBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PassBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PassBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PassBtn.ForeColor = System.Drawing.Color.Transparent;
            this.PassBtn.Location = new System.Drawing.Point(10, 10);
            this.PassBtn.Margin = new System.Windows.Forms.Padding(10);
            this.PassBtn.Name = "PassBtn";
            this.PassBtn.Size = new System.Drawing.Size(240, 88);
            this.PassBtn.TabIndex = 4;
            this.PassBtn.Text = "Pass";
            this.PassBtn.UseVisualStyleBackColor = false;
            this.PassBtn.Click += new System.EventHandler(this.PassBtn_Click);
            // 
            // FailBtn
            // 
            this.FailBtn.BackColor = System.Drawing.Color.Crimson;
            this.FailBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FailBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FailBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FailBtn.ForeColor = System.Drawing.Color.Transparent;
            this.FailBtn.Location = new System.Drawing.Point(270, 10);
            this.FailBtn.Margin = new System.Windows.Forms.Padding(10);
            this.FailBtn.Name = "FailBtn";
            this.FailBtn.Size = new System.Drawing.Size(241, 88);
            this.FailBtn.TabIndex = 5;
            this.FailBtn.Text = "Fail";
            this.FailBtn.UseVisualStyleBackColor = false;
            this.FailBtn.Click += new System.EventHandler(this.FailBtn_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.RecordBtn, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.NextBtn, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.RecordingNowLbl, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.PlayBtn, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.RecordingLbl, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.PlayAudioLbl, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.InstructionLbl, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.Title, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.Player1, 0, 5);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(101, 127);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(783, 368);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // RecordBtn
            // 
            this.RecordBtn.AutoSize = true;
            this.RecordBtn.BackColor = System.Drawing.Color.DodgerBlue;
            this.RecordBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RecordBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecordBtn.ForeColor = System.Drawing.Color.Transparent;
            this.RecordBtn.Location = new System.Drawing.Point(263, 205);
            this.RecordBtn.Name = "RecordBtn";
            this.RecordBtn.Size = new System.Drawing.Size(103, 49);
            this.RecordBtn.TabIndex = 6;
            this.RecordBtn.Text = "Start";
            this.RecordBtn.UseVisualStyleBackColor = false;
            this.RecordBtn.Click += new System.EventHandler(this.RecordBtn_Click);
            // 
            // NextBtn
            // 
            this.NextBtn.AutoSize = true;
            this.NextBtn.BackColor = System.Drawing.Color.Gray;
            this.NextBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NextBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextBtn.ForeColor = System.Drawing.Color.Transparent;
            this.NextBtn.Location = new System.Drawing.Point(398, 150);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(122, 49);
            this.NextBtn.TabIndex = 10;
            this.NextBtn.Text = "Next";
            this.NextBtn.UseVisualStyleBackColor = false;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // RecordingNowLbl
            // 
            this.RecordingNowLbl.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.RecordingNowLbl, 3);
            this.RecordingNowLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RecordingNowLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecordingNowLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.RecordingNowLbl.Location = new System.Drawing.Point(3, 257);
            this.RecordingNowLbl.Name = "RecordingNowLbl";
            this.RecordingNowLbl.Size = new System.Drawing.Size(777, 55);
            this.RecordingNowLbl.TabIndex = 7;
            this.RecordingNowLbl.Text = "Recording now ...";
            // 
            // PlayBtn
            // 
            this.PlayBtn.AutoSize = true;
            this.PlayBtn.BackColor = System.Drawing.Color.DodgerBlue;
            this.PlayBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayBtn.ForeColor = System.Drawing.Color.Transparent;
            this.PlayBtn.Location = new System.Drawing.Point(263, 150);
            this.PlayBtn.Name = "PlayBtn";
            this.PlayBtn.Size = new System.Drawing.Size(103, 49);
            this.PlayBtn.TabIndex = 9;
            this.PlayBtn.Text = "Play";
            this.PlayBtn.UseVisualStyleBackColor = false;
            this.PlayBtn.Click += new System.EventHandler(this.PlayBtn_Click);
            // 
            // RecordingLbl
            // 
            this.RecordingLbl.AutoSize = true;
            this.RecordingLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RecordingLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecordingLbl.ForeColor = System.Drawing.Color.White;
            this.RecordingLbl.Location = new System.Drawing.Point(0, 202);
            this.RecordingLbl.Margin = new System.Windows.Forms.Padding(0);
            this.RecordingLbl.Name = "RecordingLbl";
            this.RecordingLbl.Padding = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.RecordingLbl.Size = new System.Drawing.Size(260, 55);
            this.RecordingLbl.TabIndex = 4;
            this.RecordingLbl.Text = "Recording:";
            // 
            // PlayAudioLbl
            // 
            this.PlayAudioLbl.AutoSize = true;
            this.PlayAudioLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayAudioLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayAudioLbl.ForeColor = System.Drawing.Color.White;
            this.PlayAudioLbl.Location = new System.Drawing.Point(0, 147);
            this.PlayAudioLbl.Margin = new System.Windows.Forms.Padding(0);
            this.PlayAudioLbl.Name = "PlayAudioLbl";
            this.PlayAudioLbl.Padding = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.PlayAudioLbl.Size = new System.Drawing.Size(260, 55);
            this.PlayAudioLbl.TabIndex = 8;
            this.PlayAudioLbl.Text = "Play Audio File:";
            // 
            // InstructionLbl
            // 
            this.InstructionLbl.AllowDrop = true;
            this.tableLayoutPanel2.SetColumnSpan(this.InstructionLbl, 3);
            this.InstructionLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InstructionLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstructionLbl.ForeColor = System.Drawing.Color.DodgerBlue;
            this.InstructionLbl.Location = new System.Drawing.Point(0, 92);
            this.InstructionLbl.Margin = new System.Windows.Forms.Padding(0);
            this.InstructionLbl.Name = "InstructionLbl";
            this.InstructionLbl.Size = new System.Drawing.Size(783, 55);
            this.InstructionLbl.TabIndex = 11;
            this.InstructionLbl.Text = "Instructions";
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.Title, 3);
            this.Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.ForeColor = System.Drawing.Color.White;
            this.Title.Location = new System.Drawing.Point(3, 0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(777, 92);
            this.Title.TabIndex = 0;
            this.Title.Text = "Speaker and Mic Test";
            // 
            // Player1
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.Player1, 3);
            this.Player1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Player1.Enabled = true;
            this.Player1.Location = new System.Drawing.Point(3, 315);
            this.Player1.Name = "Player1";
            this.Player1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Player1.OcxState")));
            this.Player1.Size = new System.Drawing.Size(777, 50);
            this.Player1.TabIndex = 12;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(987, 624);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "Audio Test";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ButtonLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Player1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button FailBtn;
        private System.Windows.Forms.Button PassBtn;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label RecordingLbl;
        private System.Windows.Forms.Button RecordBtn;
        private System.Windows.Forms.Label RecordingNowLbl;
        private System.Windows.Forms.Button ResetBtn;
        private System.Windows.Forms.TableLayoutPanel ButtonLayoutPanel;
        private System.Windows.Forms.Label PlayAudioLbl;
        private System.Windows.Forms.Button PlayBtn;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label InstructionLbl;
        private AxWMPLib.AxWindowsMediaPlayer Player1;
    }
}

