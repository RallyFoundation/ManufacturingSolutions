using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OA3ToolConfGen
{
    public class PromoCodeTextBox : TextBox
    {
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            if (this.TextLength < ModuleConfiguration.OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueMaxLength)
            {
                string errorMessage = "Value should be {0} alphanumeric characters!";
                errorMessage = String.Format(errorMessage, ModuleConfiguration.OEMOptionalInfo_ZPGM_ELIG_VAL_SubValueMaxLength);

                MessageBox.Show(this.Parent, errorMessage, "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
