using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WPFAutomation.Core.Controls
{
    public class CheckBox
    {
        private AutomationElement checkBox;
        private AutomationElementCollection checkBoxCollection;

        public CheckBox(AutomationElement parentElement, string automationId)
        {
            checkBox = Helper.ExtractElementByAutomationID(parentElement, automationId);
            Helper.ValidateArgumentNotNull(checkBox, "CheckBox AutomationElement ");
        }

        public CheckBox(AutomationElement parentElement, ControlType type)
        {
            checkBoxCollection = Helper.ExtractElementByControlType(parentElement, ControlType.CheckBox);
            Helper.ValidateArgumentNotNull(checkBoxCollection, "CheckBox Collection AutomationElement ");
        }

        public void Click()
        {
            TogglePattern invokePattern = checkBox.GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
            invokePattern.Toggle();
        }

        public void Click(int index)
        {
            TogglePattern invokePattern = checkBoxCollection[index].GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
            invokePattern.Toggle();
        }

        public string GetStatus()
        {
            TogglePattern invokePattern = checkBox.GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
            return invokePattern.Current.ToggleState.ToString();
        }


    }
}
