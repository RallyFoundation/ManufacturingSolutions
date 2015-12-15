using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WPFAutomation.Core.Controls
{
   public class TempButton
    {
         private AutomationElement _button;
         public TempButton(AutomationElement parent, string automationID)
        {
            _button = parent.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.AutomationIdProperty, automationID));
            Helper.ValidateArgumentNotNull(_button, "Button AutomationElement ");
        }

        /// <summary>
        /// Click button event
        /// </summary>
        public void Click()
        {
            InvokePattern invokePattern = (InvokePattern)_button.GetCurrentPattern(InvokePattern.Pattern);
            invokePattern.Invoke(); 
        }

        public bool isEnable
        {
            get { return _button.Current.IsEnabled; }
        }
    }
}

