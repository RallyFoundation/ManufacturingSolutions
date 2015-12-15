using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WPFAutomation.Core.Controls
{
   public class DatePicker
    {
       private AutomationElement _dataPicker;
       public DatePicker(AutomationElement parent, string automationID)
       {
           _dataPicker = Helper.ExtractElementByAutomationID(parent, automationID);
           Helper.ValidateArgumentNotNull(_dataPicker, "DataPicker AutomationElement ");
       }

       /// <summary>
       /// 
       /// </summary>
       private TextBox _textBox;
    
       /// <summary>
       /// Set value to Data Picker
       /// </summary>
       /// <param name="dataTime"></param>
       public void SetValue(string dataTime)
       {
           _textBox = new Controls.TextBox(_dataPicker, "PART_TextBox");
           _textBox.SetValue(dataTime);
       }
    }
}
