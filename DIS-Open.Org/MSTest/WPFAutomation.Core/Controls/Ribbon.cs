using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WPFAutomation.Core.Controls
{
    public class Ribbon
    {
        public Ribbon(AutomationElement Ele, string className)
        {
            _ribbon = Helper.ExtractElementByClassName(Ele, "Ribbon");
            Helper.ValidateArgumentNotNull(_ribbon, "Ribbon AutomationElement ");

        }
        private AutomationElement _ribbon;

        /// <summary>
        /// 
        /// </summary>
        private Menu _menu;
        public Menu Menu
        {
            get
            {
                _menu = new Menu(_ribbon, "RibbonApplicationMenu");
                return _menu;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private TabControl _tab;
        public TabControl Tab
        {
            get
            {
                _tab = new TabControl(_ribbon);
                return _tab;
            }

        }



    }
}
