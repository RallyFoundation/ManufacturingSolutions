using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace OA3ToolConfGen
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))] 
    public partial class UCOHR : UserControl
    {
        public UCOHR()
        {
            InitializeComponent();

            this.comboBoxZFRM_FACTOR_CL1.DataSource = this.ZFRM_FACTOR_CL1Values;

            this.comboBoxZFRM_FACTOR_CL1.SelectedIndex = 2;

            this.comboBoxZTOUCH_SCREEN.DataSource = this.ZTOUCH_SCREENValues;

            this.comboBoxZTOUCH_SCREEN.SelectedIndex = 0;
        }

        private string[] ZFRM_FACTOR_CL1Values = new string[] 
        {
            ModuleConfiguration.OHRValue_ZFRM_FACTOR_CL1_Desktop,
            ModuleConfiguration.OHRValue_ZFRM_FACTOR_CL1_Notebook,
            ModuleConfiguration.OHRValue_ZFRM_FACTOR_CL1_Tablet
        };

        private Dictionary<string, string[]> ZFRM_FACTOR_CL2Values = new Dictionary<string, string[]>() 
        {
            {ModuleConfiguration.OHRValue_ZFRM_FACTOR_CL1_Desktop, new string[]{ModuleConfiguration.OHRValue_ZFRM_FACTOR_CL2_Standard, ModuleConfiguration.OHRValue_ZFRM_FACTOR_CL2_AIO}},
            {ModuleConfiguration.OHRValue_ZFRM_FACTOR_CL1_Notebook, new string[]{ModuleConfiguration.OHRValue_ZFRM_FACTOR_CL2_Standard, ModuleConfiguration.OHRValue_ZFRM_FACTOR_CL2_Ultraslim, ModuleConfiguration.OHRValue_ZFRM_FACTOR_CL2_Convertible}},
            {ModuleConfiguration.OHRValue_ZFRM_FACTOR_CL1_Tablet, new string[]{ModuleConfiguration.OHRValue_ZFRM_FACTOR_CL2_Standard, ModuleConfiguration.OHRValue_ZFRM_FACTOR_CL2_Detachable}}
        };

        private string[] ZTOUCH_SCREENValues = new string[] 
        {
            ModuleConfiguration.OHRValue_ZTOUCH_SCREEN_Touch,
            ModuleConfiguration.OHRValue_ZTOUCH_SCREEN_Nontouch
        };

        public Dictionary<string, string> GetOHR() 
        {
            return new Dictionary<string, string>()
            {
                {ModuleConfiguration.OHRKey_ZPC_MODEL_SKU, this.textBoxZPC_MODEL_SKU.Text},
                {ModuleConfiguration.OHRKey_ZFRM_FACTOR_CL1, (this.comboBoxZFRM_FACTOR_CL1.SelectedItem != null) ? this.comboBoxZFRM_FACTOR_CL1.SelectedItem.ToString() : ""},
                {ModuleConfiguration.OHRKey_ZFRM_FACTOR_CL2, (this.comboBoxZFRM_FACTOR_CL2.SelectedItem != null) ? this.comboBoxZFRM_FACTOR_CL2.SelectedItem.ToString() : ""},
                {ModuleConfiguration.OHRKey_ZSCREEN_SIZE, this.textBoxZSCREEN_SIZE.Text},
                {ModuleConfiguration.OHRKey_ZTOUCH_SCREEN, (this.comboBoxZTOUCH_SCREEN.SelectedItem != null) ? this.comboBoxZTOUCH_SCREEN.SelectedItem.ToString() : ""}
            };
        }

        public void SetOHR(Dictionary<string, string> OHRData) 
        {
            if ((OHRData != null) && (OHRData.Count > 0))
            {
                foreach (string key in OHRData.Keys)
                {
                    if (key == ModuleConfiguration.OHRKey_ZPC_MODEL_SKU)
                    {
                        this.textBoxZPC_MODEL_SKU.Text = OHRData[key];
                    }
                    else if (key == ModuleConfiguration.OHRKey_ZSCREEN_SIZE)
                    {
                        this.textBoxZSCREEN_SIZE.Text = OHRData[key];
                    }
                    else if (key == ModuleConfiguration.OHRKey_ZFRM_FACTOR_CL1)
                    {
                        if (this.ZFRM_FACTOR_CL1Values.Contains(OHRData[key]))
                        {
                            this.comboBoxZFRM_FACTOR_CL1.SelectedItem = OHRData[key];
                        }
                    }
                    else if (key == ModuleConfiguration.OHRKey_ZFRM_FACTOR_CL2)
                    {
                        if (this.comboBoxZFRM_FACTOR_CL2.Items.Contains(key))
                        {
                            this.comboBoxZFRM_FACTOR_CL2.SelectedItem = OHRData[key];
                        }
                    }
                    else if (key == ModuleConfiguration.OHRKey_ZTOUCH_SCREEN)
                    {
                        if (this.ZTOUCH_SCREENValues.Contains(key))
                        {
                            this.comboBoxZTOUCH_SCREEN.SelectedItem = OHRData[key];
                        }
                    }
                }
            }
        }

        private void comboBoxZFRM_FACTOR_CL1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = (sender as ComboBox).SelectedItem.ToString();

            this.comboBoxZFRM_FACTOR_CL2.DataSource = this.ZFRM_FACTOR_CL2Values[selectedValue];
            this.comboBoxZFRM_FACTOR_CL2.Refresh();
        }
    }
}
