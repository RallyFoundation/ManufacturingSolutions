using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Windows.Automation.Text;

namespace WPFAutomation.Core.Controls
{
   public class Label
    {
       private AutomationElement lableElement;
       public Label(AutomationElement parent, string automationID)
       {
           lableElement = Helper.ExtractElementByAutomationID(parent, automationID);

           Helper.ValidateArgumentNotNull(lableElement, "Lable AutomationElement ");
       }

     /// <summary>
     /// get value of lable
     /// </summary>
       public string text
       {
           get
           {
               return lableElement.Current.Name;
           }
       }
    }
}
