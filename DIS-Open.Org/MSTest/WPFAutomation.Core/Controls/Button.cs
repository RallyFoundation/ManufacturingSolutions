using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Threading;

namespace WPFAutomation.Core.Controls
{
    public class Button
    {
        private AutomationElement _button;
       
        public AutomationElement MainElement
        {
            get { return _button; }
            set { _button = value; }
        }

        public Button(AutomationElement parent,string automationID)
        {
            _button = Helper.ExtractElementByAutomationID(parent, automationID);
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
