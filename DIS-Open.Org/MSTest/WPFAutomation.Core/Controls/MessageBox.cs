using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;


namespace WPFAutomation.Core.Controls
{
    public class MessageBox
    {
        public static int TimeOutMillSec = 20000;

        /// <summary>
        /// The title of messagebox
        /// </summary>
        private string _title;

        public string Title
        {
            get { return currentElement.Current.Name; }
            set { _title = value; }
        }

        /// <summary>
        /// the text is content of messagebox.
        /// </summary>
        private Label _text;
        public string Text
        {
            get
            {
                _text = new Label(currentElement, "65535");
                return _text.text;
            }
        }

        private AutomationElement currentElement;

        public AutomationElement MainElement
        {
            get { return currentElement; }
            set { currentElement = value; }
        }

        public MessageBox(AutomationElement parentElement, string className)
        {
            int temp = DateTime.Now.Second;
            DateTime timeOut = DateTime.Now.AddMilliseconds(TimeOutMillSec);
            do
            {
                currentElement = Helper.ExtractElementByClassName(parentElement, className);

            } while (currentElement == null && DateTime.Now < timeOut);
          
            //Helper.ValidateArgumentNotNull(currentElement, "MessageBox AutomationElement ");
        }

        /// <summary>
        /// Click OK button in the MessageBox
        /// </summary>
        public void ClickOK()
        {
            Button btnOK = new Button(currentElement, "2");
            btnOK.Click();
        }

        public void ClickOK(string automationID)
        {
            Button btnOK = new Button(currentElement, automationID);
            btnOK.Click();
        }

        public void ClickYes()
        {
            Button btnYes = new Button(currentElement, "6");
            btnYes.Click();
        }

        public void ClickNo()
        {
            Button btnNo = new Button(currentElement, "7");
            btnNo.Click();
        }

        public bool ContainOk
        {
            get
            {
                if (currentElement == null) return false;
                return Helper.ExtractElementByAutomationID(currentElement, "2") == null ? false : true;
            }
        }

        public bool ContainYes
        {
            get
            {
                if (currentElement == null) return false;
                return Helper.ExtractElementByAutomationID(currentElement, "6") == null ? false : true;
            }
        }

        public bool ContainNo
        {
            get
            {
                if (currentElement == null) return false;
                return Helper.ExtractElementByAutomationID(currentElement, "7") == null ? false : true;
            }
        }

    }
}
