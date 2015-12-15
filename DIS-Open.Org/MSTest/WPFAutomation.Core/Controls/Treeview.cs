using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Windows.Automation.Text;

namespace WPFAutomation.Core.Controls
{
   public class Treeview
    {
        private AutomationElement treeview;
        private AutomationElementCollection treeviewcollection;

        public Treeview(AutomationElement parentelement,string automationid)
        {
            treeview = Helper.ExtractElementByAutomationID(parentelement ,automationid);
            Helper.ValidateArgumentNotNull(treeview,"treeview automationelement");

        }

        public Treeview(AutomationElement parentelement,ControlType type)
        {
            treeviewcollection = Helper.ExtractElementByControlType(parentelement,ControlType.Tree);
            Helper.ValidateArgumentNotNull(treeviewcollection,"treeview collection automationelement");
        }

        public void Click()
        {
            TogglePattern invokepattern = treeview.GetCurrentPattern(TogglePattern.Pattern ) as TogglePattern;
            invokepattern.Toggle();
        }

        public void Click(int index)
        {
            TogglePattern invokepattern = treeviewcollection[index].GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
            invokepattern.Toggle();
        }

        public string Getstatus()
        {
            TogglePattern invokepattern = treeview.GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
            return invokepattern.Current.ToggleState.ToString();
        }

        public void selectindex(int index)
        {
            AutomationElementCollection itemele = Helper.ExtractElementByControlType(treeview,ControlType.TreeItem);
            Helper.ValidateArgumentNotNull(itemele,"Treeitem in treeview");
            TogglePattern pattern = itemele[index].GetCurrentPattern(TogglePattern .Pattern ) as TogglePattern ;

            pattern.Toggle();
            

          
        
        }




    }
}
