using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WPFAutomation.Core.Controls
{
    public class ProgressBar
    {

        public AutomationElement progressBar { get; set; }
        private AutomationElement Mparent;
        private string MclassName;
        public ProgressBar(AutomationElement parent, string className)
        {
            Mparent = parent;
            MclassName = className;
            progressBar = Helper.ExtractElementByClassName(parent, className);
        }

        public void isFinish(int timeOut)
        {
            DateTime maxTime = DateTime.Now.AddSeconds(timeOut);
            do
            {
                progressBar = Mparent.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, MclassName));

            } while (progressBar != null && DateTime.Now < maxTime);
        }
    }
}
