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
    public partial class UCPGMELIG : UserControl
    {
        public UCPGMELIG()
        {
            InitializeComponent();
        }

        public string GetPromoCodes()
        {
            string returnValue = ModuleConfiguration.OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueSeparator;

            if (this.flowLayoutPanelPromoCodes.Controls.Count > 0)
            {
                foreach (PromoCodeTextBox control in this.flowLayoutPanelPromoCodes.Controls)
                {
                    returnValue += String.Format("{0}{1}", control.Text, ModuleConfiguration.OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueSeparator);
                }
            }
            else
            {
                returnValue = "";
            }

            return returnValue;
        }

        public void SetPromoCodes(string PromoCodeString)
        {
            if (PromoCodeString.StartsWith(ModuleConfiguration.OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueSeparator) && PromoCodeString.EndsWith(ModuleConfiguration.OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueSeparator))
            {
                string promoCodeStr = PromoCodeString.Substring(1, (PromoCodeString.Length - 2));

                string[] promoCodes = promoCodeStr.Split(new string[] { ModuleConfiguration.OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueSeparator }, StringSplitOptions.None);

                if (promoCodes != null && promoCodes.Length > 0)
                {
                    this.flowLayoutPanelPromoCodes.Controls.Clear();

                    int codeCount = promoCodes.Length <= ModuleConfiguration.OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueMaxCount ? promoCodes.Length : ModuleConfiguration.OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueMaxCount;

                    for (int i = 0; i < codeCount; i++)
                    {
                        this.flowLayoutPanelPromoCodes.Controls.Add(new PromoCodeTextBox()
                        {
                            MaxLength = ModuleConfiguration.OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueMaxLength,
                            Width = 248,
                            Text = promoCodes[i]
                        });
                    }

                    this.setControls();
                }    
            }
        }

        private void setControls()
        {
            this.buttonAdd.Enabled = this.flowLayoutPanelPromoCodes.Controls.Count < ModuleConfiguration.OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueMaxCount;
            this.buttonRemove.Enabled = this.flowLayoutPanelPromoCodes.Controls.Count > 0;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int value = this.flowLayoutPanelPromoCodes.Controls.Count + 1;

            this.flowLayoutPanelPromoCodes.Controls.Add(new PromoCodeTextBox()
            {
                MaxLength = ModuleConfiguration.OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueMaxLength,
                Width = 248,
                Text = (value < 10) ? String.Format("XXX{0}", value) : String.Format("XX{0}", value)
            });

            this.setControls();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanelPromoCodes.Controls.RemoveAt(this.flowLayoutPanelPromoCodes.Controls.Count - 1);

            this.setControls();
        }
    }
}
