namespace Battery
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
            this.PowerLab = new System.Windows.Forms.Label();
            this.ChargeLab = new System.Windows.Forms.Label();
            this.BatteryLab = new System.Windows.Forms.Label();
            this.PassBtn = new System.Windows.Forms.Button();
            this.FailBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.RetryBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ErrorLbl = new System.Windows.Forms.Label();
            this.CharRate = new System.Windows.Forms.Label();
            this.ChargingRateLab = new System.Windows.Forms.Label();
            this.Title = new System.Windows.Forms.Label();
            this.BatCap = new System.Windows.Forms.Label();
            this.PwrStatus = new System.Windows.Forms.Label();
            this.ChargeStatus = new System.Windows.Forms.Label();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PowerLab
            // 
            this.PowerLab.AutoSize = true;
            this.PowerLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PowerLab.ForeColor = System.Drawing.Color.White;
            this.PowerLab.Location = new System.Drawing.Point(0, 104);
            this.PowerLab.Margin = new System.Windows.Forms.Padding(0);
            this.PowerLab.Name = "PowerLab";
            this.PowerLab.Size = new System.Drawing.Size(349, 55);
            this.PowerLab.TabIndex = 0;
            this.PowerLab.Text = "Power Status : ";
            // 
            // ChargeLab
            // 
            this.ChargeLab.AutoSize = true;
            this.ChargeLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChargeLab.ForeColor = System.Drawing.Color.White;
            this.ChargeLab.Location = new System.Drawing.Point(0, 191);
            this.ChargeLab.Margin = new System.Windows.Forms.Padding(0);
            this.ChargeLab.Name = "ChargeLab";
            this.ChargeLab.Size = new System.Drawing.Size(358, 55);
            this.ChargeLab.TabIndex = 2;
            this.ChargeLab.Text = "Charge Status :";
            // 
            // BatteryLab
            // 
            this.BatteryLab.AutoSize = true;
            this.BatteryLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BatteryLab.ForeColor = System.Drawing.Color.White;
            this.BatteryLab.Location = new System.Drawing.Point(0, 277);
            this.BatteryLab.Margin = new System.Windows.Forms.Padding(0);
            this.BatteryLab.Name = "BatteryLab";
            this.BatteryLab.Size = new System.Drawing.Size(271, 55);
            this.BatteryLab.TabIndex = 4;
            this.BatteryLab.Text = "Battery % : ";
            // 
            // PassBtn
            // 
            this.PassBtn.BackColor = System.Drawing.Color.YellowGreen;
            this.PassBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PassBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PassBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PassBtn.ForeColor = System.Drawing.Color.White;
            this.PassBtn.Location = new System.Drawing.Point(10, 10);
            this.PassBtn.Margin = new System.Windows.Forms.Padding(10);
            this.PassBtn.Name = "PassBtn";
            this.PassBtn.Size = new System.Drawing.Size(289, 90);
            this.PassBtn.TabIndex = 6;
            this.PassBtn.Text = "PASS";
            this.PassBtn.UseVisualStyleBackColor = false;
            this.PassBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // FailBtn
            // 
            this.FailBtn.BackColor = System.Drawing.Color.Crimson;
            this.FailBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FailBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FailBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FailBtn.ForeColor = System.Drawing.Color.White;
            this.FailBtn.Location = new System.Drawing.Point(319, 10);
            this.FailBtn.Margin = new System.Windows.Forms.Padding(10);
            this.FailBtn.Name = "FailBtn";
            this.FailBtn.Size = new System.Drawing.Size(289, 90);
            this.FailBtn.TabIndex = 7;
            this.FailBtn.Text = "FAIL";
            this.FailBtn.UseVisualStyleBackColor = false;
            this.FailBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1169, 642);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel3.Controls.Add(this.FailBtn, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.PassBtn, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.RetryBtn, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(119, 516);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(929, 123);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // RetryBtn
            // 
            this.RetryBtn.BackColor = System.Drawing.Color.Gray;
            this.RetryBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RetryBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RetryBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RetryBtn.ForeColor = System.Drawing.Color.White;
            this.RetryBtn.Location = new System.Drawing.Point(628, 10);
            this.RetryBtn.Margin = new System.Windows.Forms.Padding(10);
            this.RetryBtn.Name = "RetryBtn";
            this.RetryBtn.Size = new System.Drawing.Size(291, 90);
            this.RetryBtn.TabIndex = 8;
            this.RetryBtn.Text = "Retry";
            this.RetryBtn.UseVisualStyleBackColor = false;
            this.RetryBtn.Click += new System.EventHandler(this.RetryBtn_Click);
            // 
            // panel1
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.ErrorLbl);
            this.panel1.Controls.Add(this.CharRate);
            this.panel1.Controls.Add(this.ChargingRateLab);
            this.panel1.Controls.Add(this.Title);
            this.panel1.Controls.Add(this.BatCap);
            this.panel1.Controls.Add(this.PwrStatus);
            this.panel1.Controls.Add(this.ChargeStatus);
            this.panel1.Controls.Add(this.BatteryLab);
            this.panel1.Controls.Add(this.PowerLab);
            this.panel1.Controls.Add(this.ChargeLab);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(119, 3);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel2.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(1047, 507);
            this.panel1.TabIndex = 11;
            // 
            // ErrorLbl
            // 
            this.ErrorLbl.AutoSize = true;
            this.ErrorLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorLbl.ForeColor = System.Drawing.Color.Red;
            this.ErrorLbl.Location = new System.Drawing.Point(13, 440);
            this.ErrorLbl.Margin = new System.Windows.Forms.Padding(0);
            this.ErrorLbl.Name = "ErrorLbl";
            this.ErrorLbl.Size = new System.Drawing.Size(0, 55);
            this.ErrorLbl.TabIndex = 12;
            // 
            // CharRate
            // 
            this.CharRate.AutoSize = true;
            this.CharRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CharRate.ForeColor = System.Drawing.Color.White;
            this.CharRate.Location = new System.Drawing.Point(374, 367);
            this.CharRate.Margin = new System.Windows.Forms.Padding(0);
            this.CharRate.Name = "CharRate";
            this.CharRate.Size = new System.Drawing.Size(37, 55);
            this.CharRate.TabIndex = 11;
            this.CharRate.Text = " ";
            // 
            // ChargingRateLab
            // 
            this.ChargingRateLab.AutoSize = true;
            this.ChargingRateLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChargingRateLab.ForeColor = System.Drawing.Color.White;
            this.ChargingRateLab.Location = new System.Drawing.Point(0, 367);
            this.ChargingRateLab.Margin = new System.Windows.Forms.Padding(0);
            this.ChargingRateLab.Name = "ChargingRateLab";
            this.ChargingRateLab.Size = new System.Drawing.Size(374, 55);
            this.ChargingRateLab.TabIndex = 10;
            this.ChargingRateLab.Text = "Charging Rate : ";
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.ForeColor = System.Drawing.Color.White;
            this.Title.Location = new System.Drawing.Point(-3, 0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(390, 73);
            this.Title.TabIndex = 9;
            this.Title.Text = "Battery Test";
            // 
            // BatCap
            // 
            this.BatCap.AutoSize = true;
            this.BatCap.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BatCap.ForeColor = System.Drawing.Color.White;
            this.BatCap.Location = new System.Drawing.Point(362, 277);
            this.BatCap.Margin = new System.Windows.Forms.Padding(0);
            this.BatCap.Name = "BatCap";
            this.BatCap.Size = new System.Drawing.Size(37, 55);
            this.BatCap.TabIndex = 8;
            this.BatCap.Text = " ";
            // 
            // PwrStatus
            // 
            this.PwrStatus.AutoSize = true;
            this.PwrStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PwrStatus.ForeColor = System.Drawing.Color.White;
            this.PwrStatus.Location = new System.Drawing.Point(362, 104);
            this.PwrStatus.Margin = new System.Windows.Forms.Padding(0);
            this.PwrStatus.Name = "PwrStatus";
            this.PwrStatus.Size = new System.Drawing.Size(37, 55);
            this.PwrStatus.TabIndex = 6;
            this.PwrStatus.Text = " ";
            // 
            // ChargeStatus
            // 
            this.ChargeStatus.AutoSize = true;
            this.ChargeStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChargeStatus.ForeColor = System.Drawing.Color.White;
            this.ChargeStatus.Location = new System.Drawing.Point(362, 191);
            this.ChargeStatus.Margin = new System.Windows.Forms.Padding(0);
            this.ChargeStatus.Name = "ChargeStatus";
            this.ChargeStatus.Size = new System.Drawing.Size(37, 55);
            this.ChargeStatus.TabIndex = 7;
            this.ChargeStatus.Text = " ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 642);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "Form1";
            this.Text = "Battery";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label PowerLab;
        private System.Windows.Forms.Label ChargeLab;
        private System.Windows.Forms.Label BatteryLab;
        private System.Windows.Forms.Button PassBtn;
        private System.Windows.Forms.Button FailBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label BatCap;
        private System.Windows.Forms.Label ChargeStatus;
        private System.Windows.Forms.Label PwrStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Button RetryBtn;
        private System.Windows.Forms.Label CharRate;
        private System.Windows.Forms.Label ChargingRateLab;
        private System.Windows.Forms.Label ErrorLbl;
    }
}

