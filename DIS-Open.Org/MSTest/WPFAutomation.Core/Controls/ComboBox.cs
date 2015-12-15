using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Threading;

namespace WPFAutomation.Core.Controls
{
    public class ComboBox
    {
        private AutomationElement ele;
        public ComboBox(AutomationElement parent, string automationID)
        {
            ele = Helper.ExtractElementByAutomationID(parent, automationID);

            Helper.ValidateArgumentNotNull(ele, "ComboBox AutomationElement ");
        }

        /// <summary>
        /// Select one item
        /// </summary>
        /// <param name="index"></param>
        public string SelectItem(int index)
        {
            ExpandCollapse(isexpand.Expand);
            Thread.Sleep(300);
            AutomationElementCollection items = Helper.ExtractElementByControlType(ele, ControlType.ListItem);
            Helper.ValidateArgumentNotNull(items, "Items in the ComboBox ");
            AutomationElement item = items[index];
            SelectionItemPattern pattern = item.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
            pattern.Select();
            ExpandCollapse(isexpand.Collapse);
            return item.Current.Name;

        }

        /// <summary>
        /// Expand or Collapse Combobox
        /// </summary>
        /// <param name="i"></param>
        public void ExpandCollapse(isexpand i)
        {
            ExpandCollapsePattern comboboxPattern = ele.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            if (i == isexpand.Expand)
            {
                comboboxPattern.Expand();
            }
            else
            {
                comboboxPattern.Collapse();
            }

        }

        /// <summary>
        /// total item number
        /// </summary>
        public int RowCount
        {
            get
            {
                AutomationElementCollection items = Helper.ExtractElementByControlType(ele, ControlType.ListItem);
                return items.Count;
            }
        }

        /// <summary>
        /// the current selected row number
        /// </summary>
        public int CurrentRow
        {
            get
            {
                ExpandCollapse(isexpand.Expand);
                ExpandCollapse(isexpand.Collapse);
                AutomationElementCollection items = Helper.ExtractElementByControlType(ele, ControlType.ListItem);
                SelectionItemPattern sip;
                for (int i = 0; i < items.Count; i++)
                {
                    sip = items[i].GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
                    if (sip.Current.IsSelected)
                        return i;
                }
                return -1;
            }
        }


        public enum isexpand
        {
            Expand, Collapse
        };
    }
}
