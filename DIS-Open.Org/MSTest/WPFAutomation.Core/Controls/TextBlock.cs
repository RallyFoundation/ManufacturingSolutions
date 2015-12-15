using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WPFAutomation.Core.Controls
{
    public class TextBlock
    {
        private AutomationElement textBlockElement;
        public string Text
        {
            get {
                return textBlockElement.Current.Name;
            }
            
        }

        public TextBlock(AutomationElement parent, string className)
        {
            textBlockElement = Helper.ExtractElementByClassName(parent, className);
            if (textBlockElement == null)
            {
                textBlockElement = Helper.ExtractElementByAutomationID(parent, className);
            }
            Helper.ValidateArgumentNotNull(textBlockElement, "Lable AutomationElement ");
        }

       

    }
}
