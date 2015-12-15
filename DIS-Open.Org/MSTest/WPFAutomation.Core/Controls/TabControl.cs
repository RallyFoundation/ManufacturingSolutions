using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WPFAutomation.Core.Controls
{
    public class TabControl
    {
        private AutomationElement _tabControl;

        public TabControl(AutomationElement parent)
        {
            _tabControl = parent; 
          Helper.ValidateArgumentNotNull(_tabControl, "Tab AutomationElement ");  
        }

        public TabControl(AutomationElement parent, string className)
        {
            _tabControl = Helper.ExtractElementByClassName(parent, className);
            if (_tabControl == null)
            {
                Helper.ValidateArgumentNotNull(_tabControl, "Tab AutomationElement ");
            }
        }

        /// <summary>
        /// Select one item
        /// </summary>
        /// <param name="index"></param>
        public void SelectItem(int index)
        {
            AutomationElementCollection items = Helper.ExtractElementByControlType(_tabControl, ControlType.TabItem);
            Helper.ValidateArgumentNotNull(items, "Items in the Tab ");
            AutomationElement item = items[index];
            SelectionItemPattern pattern = item.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
            pattern.Select();
        }
           
    



    }
}
