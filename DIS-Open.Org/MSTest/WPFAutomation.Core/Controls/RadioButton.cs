using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WPFAutomation.Core.Controls
{
   public class RadioButton 
    {
        private AutomationElement _radioButton;
        private AutomationElementCollection _radioButtonCollection;

        public RadioButton(AutomationElement parent, string automationID)
        {
            _radioButton = Helper.ExtractElementByAutomationID(parent, automationID);
            Helper.ValidateArgumentNotNull(_radioButton, "Radio Button AutomationElement ");
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="parent"></param>
       /// <param name="property"></param>
        public RadioButton(AutomationElement parent)
        {
            _radioButtonCollection = Helper.ExtractElementByControlType(parent, ControlType.RadioButton);
            Helper.ValidateArgumentNotNull(_radioButtonCollection, "Radio Collection Button AutomationElement ");
        }
      
       public void Click()
        {
            SelectionItemPattern pattern = _radioButton.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
            pattern.Select();
        }

       public void Click(int index)
       {
           SelectionItemPattern pattern = _radioButtonCollection[index].GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
           pattern.Select();
       }
    }
}
