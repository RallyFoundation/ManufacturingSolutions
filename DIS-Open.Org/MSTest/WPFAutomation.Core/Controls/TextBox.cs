using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WPFAutomation.Core.Controls
{
    public class TextBox
    {
        //public TextBox();

        private AutomationElement _textBox;
        public AutomationElement MainElement
        {
            get { return _textBox; }
            set { _textBox = value; }
        }
        private AutomationElementCollection _textBoxCollection;

        public TextBox(AutomationElement parentElement, string automationId)
        {
            _textBox = Helper.ExtractElementByAutomationID(parentElement, automationId);
            //Helper.ValidateArgumentNotNull(_textBox, "TestBox AutomationElement ");
        }

        public TextBox(AutomationElement parentElement, ControlType type)
        {
            _textBoxCollection = Helper.ExtractElementByControlType(parentElement, ControlType.Edit);
            Helper.ValidateArgumentNotNull(_textBoxCollection, "TestBox Collection AutomationElement ");
        }

        /// <summary>
        /// Get value from TextBox
        /// </summary>
        public string Text
        {
            get
            {
                ValuePattern setValue = (ValuePattern)_textBox.GetCurrentPattern(ValuePattern.Pattern);
                return setValue.Current.Value;
            }
        }

        /// <summary>
        /// Input some test string in textbox
        /// </summary>
        /// <param name="input"></param>
        public void SetValue(string input)
        {
            if (_textBox != null)
            {
                ValuePattern setValue = (ValuePattern)_textBox.GetCurrentPattern(ValuePattern.Pattern);
                setValue.SetValue(input);
            }
        }

        /// <summary>
        /// Set test string in textbox collection
        /// </summary>
        /// <param name="input"></param>
        public void SetValue(int index, string value)
        {
            if (_textBoxCollection.Count > 0)
            {
                ValuePattern setValue = (ValuePattern)_textBoxCollection[index].GetCurrentPattern(ValuePattern.Pattern);
                setValue.SetValue(value);
            }
        }

        /// <summary>
        /// whether the text box is able to input something
        /// Wilson
        /// </summary>
        public bool IsEnabled { get { return _textBox.Current.IsEnabled; } }

        /// <summary>
        /// type some charactors simulating keyboard
        /// </summary>
        public void Type(string input)
        {
            if (_textBox != null)
            {
                ValuePattern setValue = (ValuePattern)_textBox.GetCurrentPattern(ValuePattern.Pattern);
                setValue.SetValue(" ");
                setValue.SetValue("");
                _textBox.SetFocus();
                Keyboard.Type(input);
            }
        }

    }
}
