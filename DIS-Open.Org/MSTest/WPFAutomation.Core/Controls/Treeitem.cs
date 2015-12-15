using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WPFAutomation.Core.Controls
{
    public class Treeitem
    {
        private AutomationElement treeitem;
        private AutomationElementCollection treeitemcollection;

        public Treeitem(AutomationElement parentelement,string automationid)
        {
            treeitem = Helper.ExtractElementByAutomationID(parentelement,automationid);
            Helper.ValidateArgumentNotNull(treeitem,"treeview automationelement");
        }

        public Treeitem(AutomationElement parentelement,ControlType type)
        {
            treeitemcollection = Helper.ExtractElementByControlType(parentelement,ControlType.TreeItem);
            Helper.ValidateArgumentNotNull(treeitemcollection,"treeitem collection automationelement");



        }

        public void Click()
        {
            TogglePattern invokepattern = (TogglePattern)treeitem.GetCurrentPattern(TogglePattern.Pattern );
            invokepattern.Toggle();

        }
        public void Click(int index)
        {
            TogglePattern invokepattern = (TogglePattern)treeitemcollection[index].GetCurrentPattern(TogglePattern.Pattern );
            invokepattern.Toggle();

        }

        public string  getstatus()
        { 
        
       TogglePattern invokepattern = (TogglePattern)treeitem.GetCurrentPattern(TogglePattern.Pattern );
        return invokepattern.Current.ToggleState.ToString();
       
        
        }
    }
}
