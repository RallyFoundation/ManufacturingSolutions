using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WPFAutomation.Core.Controls
{
    public class ListBox
    {
        private AutomationElement _listBox;

        public AutomationElement MainElement
        {
            get { return _listBox; }
            set { _listBox = value; }
        }

        public ListBox(AutomationElement parent, string automationId)
        {
            _listBox = Helper.ExtractElementByAutomationID(parent, automationId);
            //Helper.ValidateArgumentNotNull(_listBox, "ListBox AutomationElement ");
        }

        public void Select(int index)
        {
            AutomationElementCollection items = Helper.ExtractElementByControlType(_listBox, ControlType.ListItem);
            Helper.ValidateArgumentNotNull(items, "Items in the ListBox ");
            SelectionItemPattern pattern = items[index].GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
            pattern.Select();
        }
    }
}
