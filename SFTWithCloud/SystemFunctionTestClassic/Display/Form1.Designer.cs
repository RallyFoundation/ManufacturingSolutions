namespace Display
{
    partial class Form1
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
            this.PASS = new System.Windows.Forms.Button();
            this.FAIL = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PASS
            // 
            this.PASS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PASS.AutoSize = true;
            this.PASS.BackColor = System.Drawing.Color.YellowGreen;
            this.PASS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PASS.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PASS.ForeColor = System.Drawing.Color.White;
            this.PASS.Location = new System.Drawing.Point(20, 183);
            this.PASS.Margin = new System.Windows.Forms.Padding(10);
            this.PASS.Name = "PASS";
            this.PASS.Size = new System.Drawing.Size(220, 90);
            this.PASS.TabIndex = 0;
            this.PASS.Text = "PASS";
            this.PASS.UseVisualStyleBackColor = false;
            this.PASS.Visible = false;
            this.PASS.Click += new System.EventHandler(this.PASS_Click);
            // 
            // FAIL
            // 
            this.FAIL.AutoSize = true;
            this.FAIL.BackColor = System.Drawing.Color.Crimson;
            this.FAIL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FAIL.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FAIL.ForeColor = System.Drawing.Color.White;
            this.FAIL.Location = new System.Drawing.Point(260, 183);
            this.FAIL.Margin = new System.Windows.Forms.Padding(10);
            this.FAIL.Name = "FAIL";
            this.FAIL.Size = new System.Drawing.Size(184, 90);
            this.FAIL.TabIndex = 1;
            this.FAIL.Text = "FAIL";
            this.FAIL.UseVisualStyleBackColor = false;
            this.FAIL.Visible = false;
            this.FAIL.Click += new System.EventHandler(this.FAIL_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.FAIL, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.PASS, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(501, 347);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(501, 347);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Click += new System.EventHandler(this.Form1_Click);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button PASS;
        private System.Windows.Forms.Button FAIL;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

