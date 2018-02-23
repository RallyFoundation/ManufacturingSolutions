using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Contract;

namespace OA3ToolConfGen
{
    public partial class UCProductKeyIDRange : UserControl
    {
        public UCProductKeyIDRange()
        {
            InitializeComponent();
        }

        private List<QueryItem> keyQueryItems = null;
        private long[] keyQueryResults = null;

        public string FFKIAPIUrl { get; set; }

        public string ConfigurationID { get; set; }

        public long[] GetKeyRange()
        {
            if (!String.IsNullOrEmpty(this.textBoxProductKeyIDFrom.Text) && !String.IsNullOrEmpty(this.textBoxProductKeyIDTo.Text))
            {
                long keyIDStart, keyIDTo;

                if (long.TryParse(this.textBoxProductKeyIDFrom.Text, out keyIDStart) && long.TryParse(this.textBoxProductKeyIDTo.Text, out keyIDTo))
                {
                    return new long[] { keyIDStart, keyIDTo};
                }
            }

            return null;
        }

        public void Populate(DISAdapter.OA3ServerBasedParametersParameter[] Parameters)
        {
            if (Parameters != null)
            {
                Dictionary<string, object> paramDict = new Dictionary<string, object>();

                foreach (var parameter in Parameters)
                {
                    paramDict.Add(parameter.name, parameter.value);
                }

                this.SetKeyRange(paramDict);
            }
        }
        public void SetKeyRange(Dictionary<string, object> Data)
        {
            this.clearControls();

            this.labelParamValueBusinessID.Text = this.ConfigurationID;

            int keyTypeID = ModuleConfiguration.KeyTypeID;

            switch (keyTypeID)
            {
                case 1: this.labelParamValueKeyType.Text = "Standard"; break;
                case 2: this.labelParamValueKeyType.Text = "MBR"; break;
                case 4: this.labelParamValueKeyType.Text = "MAT"; break;
            }

            if (Data != null)
            {
                this.keyQueryItems = new List<QueryItem>();

                foreach (string key in Data.Keys)
                {
                    if (key == ModuleConfiguration.ParameterKey_LicensablePartNumber)
                    {
                        this.labelParamValueLicensablePartNumber.Text = Data[key].ToString();
                        this.keyQueryItems.Add(new QueryItem() { Name = key, Value = Data[key].ToString()});
                    }
                    else if (key == ModuleConfiguration.ParameterKey_OEMPartNumber)
                    {
                        this.labelParamValueOEMPartNumber.Text = Data[key].ToString();
                        this.keyQueryItems.Add(new QueryItem() { Name = key, Value = Data[key].ToString()});
                    }
                    else if (key == ModuleConfiguration.ParameterKey_OEMPONumber)
                    {
                        this.labelParamValueOEMPONumber.Text = Data[key].ToString();
                        this.keyQueryItems.Add(new QueryItem() { Name = key, Value = Data[key].ToString()});
                    }
                    else if (key == ModuleConfiguration.ParameterKey_ProductKeyIDFrom)
                    {
                        this.textBoxProductKeyIDFrom.Text = Data[key].ToString();
                    }
                    else if (key == ModuleConfiguration.ParameterKey_ProductKeyIDTo)
                    {
                        this.textBoxProductKeyIDTo.Text = Data[key].ToString();
                    }
                }
            }
        }

        private void clearControls()
        {
            this.labelParamValueBusinessID.Text = "";
            this.labelParamValueKeyType.Text = "";
            this.labelParamValueLicensablePartNumber.Text = "";
            this.labelParamValueOEMPartNumber.Text = "";
            this.labelParamValueOEMPONumber.Text = "";
            //this.textBoxProductKeyIDFrom.Text = "";
            //this.textBoxProductKeyIDTo.Text = "";
            //this.textBoxTotalKeys.Text = "-1";
        }

        private void getValueFromFFKIAPI(int keyCount)
        {
            List<long> result = ModuleConfiguration.GetProductKeyIDRange(this.FFKIAPIUrl, "", "", keyCount, this.ConfigurationID, this.keyQueryItems.ToArray());

            if ((result != null) && (result.Count > 0))
            {
                this.textBoxProductKeyIDTo.Text = result[0].ToString();
                this.textBoxProductKeyIDFrom.Text = result[result.Count - 1].ToString();

                this.keyQueryResults = result.ToArray();
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int keyCount = -1;

                if (!int.TryParse(this.textBoxTotalKeys.Text, out keyCount))
                {
                    keyCount = -9;
                }

                this.getValueFromFFKIAPI(keyCount);

                this.listBoxSearchResult.DataSource = this.keyQueryResults;
            }
            catch (Exception ex)
            {
                string errorMessage = String.Format("Error(s) occurred: {0}", ex.ToString());
                MessageBox.Show(errorMessage, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBoxProductKeyIDFrom_DragDrop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(typeof(long));
            this.textBoxProductKeyIDFrom.Text = data.ToString();
        }

        private void textBoxProductKeyIDTo_DragDrop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(typeof(long));
            this.textBoxProductKeyIDTo.Text = data.ToString();
        }

        private void listBoxSearchResult_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.listBoxSearchResult.SelectedItem == null)
            {
                return;
            }

            this.listBoxSearchResult.DoDragDrop(this.listBoxSearchResult.SelectedItem, DragDropEffects.Move);
        }

        private void textBoxProductKeyIDFrom_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void textBoxProductKeyIDTo_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
    }
}
