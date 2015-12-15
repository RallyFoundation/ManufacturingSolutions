using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WPFAutomation.Core.Controls;

namespace WPFAutomation.Core
{
    /// <summary>
    /// Base window 
    /// </summary>
    public abstract class BaseWindow
    {
        private AutomationElement mainElement = null;
        private string title = string.Empty;

        /// <summary>
        /// The title of the window
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// The Main window
        /// </summary>
        public AutomationElement MainElement
        {
            get { return mainElement; }
            set { mainElement = value; }
        }

        public BaseWindow() { }

        public BaseWindow(AutomationElement autoElement, string title)
        {
            Helper.ValidateArgumentNotNull(autoElement, "autoElement");
            Helper.ValidateArgumentNotNull(title, "title");
            this.title = title;
            this.mainElement = Helper.ExtractElement(autoElement, title);
        }
    }
}
