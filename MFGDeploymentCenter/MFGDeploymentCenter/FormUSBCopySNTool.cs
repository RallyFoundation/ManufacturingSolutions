using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;

namespace MfgSolutionsDeploymentCenter
{
    public partial class FormUSBCopySNTool : MetroForm
    {
        public FormUSBCopySNTool(MetroForm Sender, string[] RemovableDisks, string Source)
        {
            InitializeComponent();

            this.sender = Sender;
            this.source = Source;

            this.metroComboBoxRemovableDisks.DataSource = RemovableDisks;
            this.metroComboBoxRemovableDisks.SelectedIndex = 0;

            string[] vendors = Directory.GetDirectories(this.source, "*", SearchOption.TopDirectoryOnly);

            if (vendors.Length > 0)
            {
                List<string> vendorNames = new List<string>();

                foreach (string vendor in vendors)
                {
                    vendorNames.Add(new DirectoryInfo(vendor).Name);
                }

                this.metroComboBoxFirmwareVendors.DataSource = vendorNames;
                this.metroComboBoxFirmwareVendors.SelectedIndex = 0;
            }
        }

        private string source;
        private MetroForm sender;

        private void setControls(bool isCopying, string sourcePath) 
        {
            this.metroProgressBarCopyProgress.Visible = isCopying;
            this.metroLabelCurrentItem.Visible = isCopying;
            this.metroLinkBack.Enabled = isCopying;
            this.metroButtonBeginCopy.Enabled = isCopying;
            this.metroComboBoxRemovableDisks.Enabled = isCopying;
            this.metroComboBoxFirmwareVendors.Enabled = isCopying;

            if (isCopying)
            {
                string[] items = System.IO.Directory.GetFileSystemEntries(sourcePath, "*", SearchOption.AllDirectories);

                this.metroProgressBarCopyProgress.Maximum = items.Length;
                this.metroProgressBarCopyProgress.Minimum = 0;
                this.metroProgressBarCopyProgress.Value = 0;
            }
            else
            {
                this.metroProgressBarCopyProgress.Maximum = 0;
                this.metroProgressBarCopyProgress.Minimum = 0;
                this.metroProgressBarCopyProgress.Value = 0;
            }
        }


        private void CopyDirectory(string Source, string Destination)
        {
            String[] Files;

            if (Destination[Destination.Length - 1] != Path.DirectorySeparatorChar)
            {
                Destination += Path.DirectorySeparatorChar;
            }

            if (!Directory.Exists(Destination)) 
            {
                Directory.CreateDirectory(Destination);

                this.Invoke(new Action(() =>
                {
                    this.metroProgressBarCopyProgress.PerformStep();
                    this.metroLabelCurrentItem.Text = Source;
                    this.metroLabelCurrentItem.Refresh();
                }));
            }

            Files = Directory.GetFileSystemEntries(Source);

            foreach (string Element in Files)
            {
                // Sub directories
                if (Directory.Exists(Element))
                {
                    CopyDirectory(Element, Destination + Path.GetFileName(Element));
                }
                // Files in directory
                else
                {
                    File.Copy(Element, Destination + Path.GetFileName(Element), true);

                    //this.Invoke(new Action(() =>
                    //{
                    //    this.metroProgressBarCopyProgress.PerformStep();
                    //    this.metroLabelCurrentItem.Text = Element;
                    //}));
                }

                this.Invoke(new Action(() =>
                {
                    this.metroProgressBarCopyProgress.PerformStep();
                    this.metroLabelCurrentItem.Text = Element;
                    this.metroLabelCurrentItem.Refresh();
                }));
            }
        }

        private void metroButtonBeginCopy_Click(object sender, EventArgs e)
        {
            if (this.metroComboBoxFirmwareVendors.SelectedIndex < 0)
            {
                MetroMessageBox.Show(this, "Please select a firmware vendor!", "Firmware Vendor Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            string path = this.source;

            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }

            path += this.metroComboBoxFirmwareVendors.SelectedValue;

            string[] items = Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);

            if ((items == null) || (items.Length <= 0))
            {
                MetroMessageBox.Show(this, "Please contact your firmware vendor!", "Vendor Approval Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            try
            {
                //Utility.CopyDirectory(this.source, this.metroComboBoxRemovableDisks.SelectedValue.ToString());

                this.setControls(true, path);

                //this.CopyDirectory(this.source, this.metroComboBoxRemovableDisks.SelectedValue.ToString());

                this.CopyDirectory(path, this.metroComboBoxRemovableDisks.SelectedValue.ToString());

                if (MetroMessageBox.Show(this, String.Format("All files from {0} successfully depolyed to {1} !", path, this.metroComboBoxRemovableDisks.SelectedValue.ToString()), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                {
                    this.setControls(false, path);
                    this.sender.Visible = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, String.Format("Error(s) encountered: {0} ", ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }

        private void metroLinkBack_Click(object sender, EventArgs e)
        {
            if (this.sender != null)
            {
                this.sender.Visible = true;
                this.Close();
            }
        }

        private void FormUSBCopy_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.sender != null)
            {
                this.sender.Visible = true;
            }
        }
    }
}
