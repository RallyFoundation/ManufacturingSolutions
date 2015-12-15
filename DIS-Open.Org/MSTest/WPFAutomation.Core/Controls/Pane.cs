using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WPFAutomation.Core.Controls
{
   public class Pane
    {
        private AutomationElement _paneEle;
        public AutomationElement PaneEle
        {
            get { return _paneEle; }
            set { _paneEle = value; }
        }

       public Pane(AutomationElement parent, string automationID)
       {
           _paneEle = Helper.ExtractElementByAutomationID(parent, automationID);
       }
    }
}
