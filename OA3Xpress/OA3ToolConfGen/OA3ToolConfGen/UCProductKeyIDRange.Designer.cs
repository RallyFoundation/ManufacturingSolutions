namespace OA3ToolConfGen
{
    partial class UCProductKeyIDRange
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
            this.labelTotalKeyCount = new System.Windows.Forms.Label();
            this.textBoxTotalKeys = new System.Windows.Forms.TextBox();
            this.textBoxProductKeyIDFrom = new System.Windows.Forms.TextBox();
            this.labelProductKeyIDFrom = new System.Windows.Forms.Label();
            this.textBoxProductKeyIDTo = new System.Windows.Forms.TextBox();
            this.labelProductKeyIDTo = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.groupBoxKeyRange = new System.Windows.Forms.GroupBox();
            this.groupBoxParameters = new System.Windows.Forms.GroupBox();
            this.labelParamValueOEMPONumber = new System.Windows.Forms.Label();
            this.labelParamNameOEMPONumber = new System.Windows.Forms.Label();
            this.labelParamValueOEMPartNumber = new System.Windows.Forms.Label();
            this.labelParamNameOEMPartNumber = new System.Windows.Forms.Label();
            this.labelParamValueKeyType = new System.Windows.Forms.Label();
            this.labelParamNameKeyType = new System.Windows.Forms.Label();
            this.labelParamValueLicensablePartNumber = new System.Windows.Forms.Label();
            this.labelParamNameLicensablePartNumber = new System.Windows.Forms.Label();
            this.labelParamValueBusinessID = new System.Windows.Forms.Label();
            this.labelParamNameBusinessID = new System.Windows.Forms.Label();
            this.listBoxSearchResult = new System.Windows.Forms.ListBox();
            this.groupBoxKeyRange.SuspendLayout();
            this.groupBoxParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTotalKeyCount
            // 
            this.labelTotalKeyCount.AutoSize = true;
            this.labelTotalKeyCount.Location = new System.Drawing.Point(9, 123);
            this.labelTotalKeyCount.Name = "labelTotalKeyCount";
            this.labelTotalKeyCount.Size = new System.Drawing.Size(86, 13);
            this.labelTotalKeyCount.TabIndex = 0;
            this.labelTotalKeyCount.Text = "Total Key Count:";
            // 
            // textBoxTotalKeys
            // 
            this.textBoxTotalKeys.Location = new System.Drawing.Point(101, 120);
            this.textBoxTotalKeys.MaxLength = 6;
            this.textBoxTotalKeys.Name = "textBoxTotalKeys";
            this.textBoxTotalKeys.Size = new System.Drawing.Size(69, 20);
            this.textBoxTotalKeys.TabIndex = 1;
            this.textBoxTotalKeys.Text = "-1";
            // 
            // textBoxProductKeyIDFrom
            // 
            this.textBoxProductKeyIDFrom.AllowDrop = true;
            this.textBoxProductKeyIDFrom.Location = new System.Drawing.Point(45, 25);
            this.textBoxProductKeyIDFrom.MaxLength = 9;
            this.textBoxProductKeyIDFrom.Name = "textBoxProductKeyIDFrom";
            this.textBoxProductKeyIDFrom.Size = new System.Drawing.Size(131, 20);
            this.textBoxProductKeyIDFrom.TabIndex = 3;
            this.textBoxProductKeyIDFrom.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxProductKeyIDFrom_DragDrop);
            this.textBoxProductKeyIDFrom.DragOver += new System.Windows.Forms.DragEventHandler(this.textBoxProductKeyIDFrom_DragOver);
            // 
            // labelProductKeyIDFrom
            // 
            this.labelProductKeyIDFrom.AutoSize = true;
            this.labelProductKeyIDFrom.Location = new System.Drawing.Point(6, 28);
            this.labelProductKeyIDFrom.Name = "labelProductKeyIDFrom";
            this.labelProductKeyIDFrom.Size = new System.Drawing.Size(33, 13);
            this.labelProductKeyIDFrom.TabIndex = 2;
            this.labelProductKeyIDFrom.Text = "From:";
            // 
            // textBoxProductKeyIDTo
            // 
            this.textBoxProductKeyIDTo.AllowDrop = true;
            this.textBoxProductKeyIDTo.Location = new System.Drawing.Point(214, 25);
            this.textBoxProductKeyIDTo.MaxLength = 9;
            this.textBoxProductKeyIDTo.Name = "textBoxProductKeyIDTo";
            this.textBoxProductKeyIDTo.Size = new System.Drawing.Size(131, 20);
            this.textBoxProductKeyIDTo.TabIndex = 5;
            this.textBoxProductKeyIDTo.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxProductKeyIDTo_DragDrop);
            this.textBoxProductKeyIDTo.DragOver += new System.Windows.Forms.DragEventHandler(this.textBoxProductKeyIDTo_DragOver);
            // 
            // labelProductKeyIDTo
            // 
            this.labelProductKeyIDTo.AutoSize = true;
            this.labelProductKeyIDTo.Location = new System.Drawing.Point(185, 28);
            this.labelProductKeyIDTo.Name = "labelProductKeyIDTo";
            this.labelProductKeyIDTo.Size = new System.Drawing.Size(23, 13);
            this.labelProductKeyIDTo.TabIndex = 4;
            this.labelProductKeyIDTo.Text = "To:";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(178, 118);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(52, 23);
            this.buttonSearch.TabIndex = 6;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // groupBoxKeyRange
            // 
            this.groupBoxKeyRange.Controls.Add(this.labelProductKeyIDFrom);
            this.groupBoxKeyRange.Controls.Add(this.textBoxProductKeyIDFrom);
            this.groupBoxKeyRange.Controls.Add(this.labelProductKeyIDTo);
            this.groupBoxKeyRange.Controls.Add(this.textBoxProductKeyIDTo);
            this.groupBoxKeyRange.Location = new System.Drawing.Point(8, 149);
            this.groupBoxKeyRange.Name = "groupBoxKeyRange";
            this.groupBoxKeyRange.Size = new System.Drawing.Size(372, 57);
            this.groupBoxKeyRange.TabIndex = 8;
            this.groupBoxKeyRange.TabStop = false;
            this.groupBoxKeyRange.Text = "Product Key ID Range";
            // 
            // groupBoxParameters
            // 
            this.groupBoxParameters.Controls.Add(this.labelParamValueOEMPONumber);
            this.groupBoxParameters.Controls.Add(this.labelParamNameOEMPONumber);
            this.groupBoxParameters.Controls.Add(this.labelParamValueOEMPartNumber);
            this.groupBoxParameters.Controls.Add(this.labelParamNameOEMPartNumber);
            this.groupBoxParameters.Controls.Add(this.labelParamValueKeyType);
            this.groupBoxParameters.Controls.Add(this.labelParamNameKeyType);
            this.groupBoxParameters.Controls.Add(this.labelParamValueLicensablePartNumber);
            this.groupBoxParameters.Controls.Add(this.labelParamNameLicensablePartNumber);
            this.groupBoxParameters.Controls.Add(this.labelParamValueBusinessID);
            this.groupBoxParameters.Controls.Add(this.labelParamNameBusinessID);
            this.groupBoxParameters.Location = new System.Drawing.Point(8, 4);
            this.groupBoxParameters.Name = "groupBoxParameters";
            this.groupBoxParameters.Size = new System.Drawing.Size(372, 106);
            this.groupBoxParameters.TabIndex = 9;
            this.groupBoxParameters.TabStop = false;
            this.groupBoxParameters.Text = "Parameters";
            // 
            // labelParamValueOEMPONumber
            // 
            this.labelParamValueOEMPONumber.AutoSize = true;
            this.labelParamValueOEMPONumber.Location = new System.Drawing.Point(109, 82);
            this.labelParamValueOEMPONumber.Name = "labelParamValueOEMPONumber";
            this.labelParamValueOEMPONumber.Size = new System.Drawing.Size(0, 13);
            this.labelParamValueOEMPONumber.TabIndex = 9;
            // 
            // labelParamNameOEMPONumber
            // 
            this.labelParamNameOEMPONumber.AutoSize = true;
            this.labelParamNameOEMPONumber.Location = new System.Drawing.Point(7, 82);
            this.labelParamNameOEMPONumber.Name = "labelParamNameOEMPONumber";
            this.labelParamNameOEMPONumber.Size = new System.Drawing.Size(92, 13);
            this.labelParamNameOEMPONumber.TabIndex = 8;
            this.labelParamNameOEMPONumber.Text = "OEM PO Number:";
            // 
            // labelParamValueOEMPartNumber
            // 
            this.labelParamValueOEMPartNumber.AutoSize = true;
            this.labelParamValueOEMPartNumber.Location = new System.Drawing.Point(111, 60);
            this.labelParamValueOEMPartNumber.Name = "labelParamValueOEMPartNumber";
            this.labelParamValueOEMPartNumber.Size = new System.Drawing.Size(0, 13);
            this.labelParamValueOEMPartNumber.TabIndex = 7;
            // 
            // labelParamNameOEMPartNumber
            // 
            this.labelParamNameOEMPartNumber.AutoSize = true;
            this.labelParamNameOEMPartNumber.Location = new System.Drawing.Point(7, 60);
            this.labelParamNameOEMPartNumber.Name = "labelParamNameOEMPartNumber";
            this.labelParamNameOEMPartNumber.Size = new System.Drawing.Size(96, 13);
            this.labelParamNameOEMPartNumber.TabIndex = 6;
            this.labelParamNameOEMPartNumber.Text = "OEM Part Number:";
            // 
            // labelParamValueKeyType
            // 
            this.labelParamValueKeyType.AutoSize = true;
            this.labelParamValueKeyType.Location = new System.Drawing.Point(215, 19);
            this.labelParamValueKeyType.Name = "labelParamValueKeyType";
            this.labelParamValueKeyType.Size = new System.Drawing.Size(0, 13);
            this.labelParamValueKeyType.TabIndex = 5;
            // 
            // labelParamNameKeyType
            // 
            this.labelParamNameKeyType.AutoSize = true;
            this.labelParamNameKeyType.Location = new System.Drawing.Point(157, 19);
            this.labelParamNameKeyType.Name = "labelParamNameKeyType";
            this.labelParamNameKeyType.Size = new System.Drawing.Size(55, 13);
            this.labelParamNameKeyType.TabIndex = 4;
            this.labelParamNameKeyType.Text = "Key Type:";
            // 
            // labelParamValueLicensablePartNumber
            // 
            this.labelParamValueLicensablePartNumber.AutoSize = true;
            this.labelParamValueLicensablePartNumber.Location = new System.Drawing.Point(137, 39);
            this.labelParamValueLicensablePartNumber.Name = "labelParamValueLicensablePartNumber";
            this.labelParamValueLicensablePartNumber.Size = new System.Drawing.Size(0, 13);
            this.labelParamValueLicensablePartNumber.TabIndex = 3;
            // 
            // labelParamNameLicensablePartNumber
            // 
            this.labelParamNameLicensablePartNumber.AutoSize = true;
            this.labelParamNameLicensablePartNumber.Location = new System.Drawing.Point(7, 39);
            this.labelParamNameLicensablePartNumber.Name = "labelParamNameLicensablePartNumber";
            this.labelParamNameLicensablePartNumber.Size = new System.Drawing.Size(123, 13);
            this.labelParamNameLicensablePartNumber.TabIndex = 2;
            this.labelParamNameLicensablePartNumber.Text = "Licensable Part Number:";
            // 
            // labelParamValueBusinessID
            // 
            this.labelParamValueBusinessID.AutoSize = true;
            this.labelParamValueBusinessID.Location = new System.Drawing.Point(78, 19);
            this.labelParamValueBusinessID.Name = "labelParamValueBusinessID";
            this.labelParamValueBusinessID.Size = new System.Drawing.Size(0, 13);
            this.labelParamValueBusinessID.TabIndex = 1;
            // 
            // labelParamNameBusinessID
            // 
            this.labelParamNameBusinessID.AutoSize = true;
            this.labelParamNameBusinessID.Location = new System.Drawing.Point(7, 19);
            this.labelParamNameBusinessID.Name = "labelParamNameBusinessID";
            this.labelParamNameBusinessID.Size = new System.Drawing.Size(66, 13);
            this.labelParamNameBusinessID.TabIndex = 0;
            this.labelParamNameBusinessID.Text = "Business ID:";
            // 
            // listBoxSearchResult
            // 
            this.listBoxSearchResult.FormattingEnabled = true;
            this.listBoxSearchResult.Location = new System.Drawing.Point(8, 212);
            this.listBoxSearchResult.Name = "listBoxSearchResult";
            this.listBoxSearchResult.Size = new System.Drawing.Size(372, 186);
            this.listBoxSearchResult.TabIndex = 10;
            this.listBoxSearchResult.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxSearchResult_MouseDown);
            // 
            // UCProductKeyIDRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.listBoxSearchResult);
            this.Controls.Add(this.groupBoxParameters);
            this.Controls.Add(this.groupBoxKeyRange);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.textBoxTotalKeys);
            this.Controls.Add(this.labelTotalKeyCount);
            this.Name = "UCProductKeyIDRange";
            this.Size = new System.Drawing.Size(388, 420);
            this.groupBoxKeyRange.ResumeLayout(false);
            this.groupBoxKeyRange.PerformLayout();
            this.groupBoxParameters.ResumeLayout(false);
            this.groupBoxParameters.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTotalKeyCount;
        private System.Windows.Forms.TextBox textBoxTotalKeys;
        private System.Windows.Forms.TextBox textBoxProductKeyIDFrom;
        private System.Windows.Forms.Label labelProductKeyIDFrom;
        private System.Windows.Forms.TextBox textBoxProductKeyIDTo;
        private System.Windows.Forms.Label labelProductKeyIDTo;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.GroupBox groupBoxKeyRange;
        private System.Windows.Forms.GroupBox groupBoxParameters;
        private System.Windows.Forms.Label labelParamValueOEMPartNumber;
        private System.Windows.Forms.Label labelParamNameOEMPartNumber;
        private System.Windows.Forms.Label labelParamValueKeyType;
        private System.Windows.Forms.Label labelParamNameKeyType;
        private System.Windows.Forms.Label labelParamValueLicensablePartNumber;
        private System.Windows.Forms.Label labelParamNameLicensablePartNumber;
        private System.Windows.Forms.Label labelParamValueBusinessID;
        private System.Windows.Forms.Label labelParamNameBusinessID;
        private System.Windows.Forms.Label labelParamValueOEMPONumber;
        private System.Windows.Forms.Label labelParamNameOEMPONumber;
        private System.Windows.Forms.ListBox listBoxSearchResult;
    }
}
