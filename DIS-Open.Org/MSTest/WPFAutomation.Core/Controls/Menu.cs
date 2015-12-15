using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WPFAutomation.Core.Controls
{
    public class Menu
    {
        private AutomationElement _Menu;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="className"></param>
        public Menu(AutomationElement parent, string className)
        {
            _Menu = Helper.ExtractElementByClassName(parent, className);

            Helper.ValidateArgumentNotNull(_Menu, "Menu AutomationElement ");
        }

        /// <summary>
        /// 
        /// </summary>
        public void Expand()
        {
            ExpandCollapsePattern ExPattern = (ExpandCollapsePattern)_Menu.GetCurrentPattern(ExpandCollapsePattern.Pattern);
            ExPattern.Expand();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Collapsed()
        {
            ExpandCollapsePattern ExPattern = (ExpandCollapsePattern)_Menu.GetCurrentPattern(ExpandCollapsePattern.Pattern);
            ExPattern.Collapse();
        }

        /// <summary>
        /// Click one item
        /// </summary>
        /// <param name="index"></param>
        public void SelectItem(int index)
        {
            try
            {
                AutomationElementCollection items = Helper.ExtractElementByControlType(_Menu, ControlType.MenuItem);
                Helper.ValidateArgumentNotNull(items, "Items in the Menu ");
                AutomationElement item = items[index];
                InvokePattern patt = item.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                patt.Invoke();
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// get the menu content array
        /// </summary>
        public string[] Items
        {
            get
            {
                this.Expand();
                AutomationElementCollection menuItems = Helper.ExtractElementByControlType(_Menu, ControlType.MenuItem);
                string[] items = new string[menuItems.Count];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = menuItems[i].Current.Name;
                }
                this.Collapsed();
                return items;
            }
        }

    }
}
