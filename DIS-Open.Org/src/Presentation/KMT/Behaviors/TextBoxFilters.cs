//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace DIS.Presentation.KMT.Behaviors
{
    /// <summary>
    /// This class is used to apply filters to TextBox such as preventing special characters.
    /// </summary>
    public static class TextBoxFilters
    {
        #region Private Fields

        // List of allowed keys. Put them here if you want to allow that key to pressed
        private static readonly List<Key> controlKeys = new List<Key>
                                                            {
                                                                Key.Back,
                                                                Key.CapsLock,
                                                                Key.Down,
                                                                Key.End,
                                                                Key.Enter,
                                                                Key.Escape,
                                                                Key.Home,
                                                                Key.Insert,
                                                                Key.Left,
                                                                Key.PageDown,
                                                                Key.PageUp,
                                                                Key.Right,
                                                                Key.LeftShift,
                                                                Key.RightShift,
                                                                Key.Tab,
                                                                Key.Up,
                                                            };

        // List of allowed keys for Rma Textbox. Put them here if you want to allow that key to pressed
        private static readonly List<Key> RmacontrolKeys = new List<Key>
                                                              {
                                                                Key.D9,
                                                                Key.D0,
                                                                Key.OemMinus,
                                                                Key.Oem2,
                                                                Key.Oem5,
                                                                Key.OemPeriod,
                                                                Key.OemComma,
                                                              };
        #endregion

        #region Private Methods

        /// <summary>
        /// Verifies pasting text with Special Alphanumeric regular expression and allows if it is alphanumeric
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">DataObjectPastingEventArgs</param>
        private static void RmaCancelCommand(object sender, DataObjectPastingEventArgs e)
        {
            bool isAlphaNumeric = false;
            string AlphaNumberRegEx = "^[a-zA-Z0-9_,()/\\\\-]+$"; // Regular expression to check AlphaNumeric values
            string value = string.Empty;
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                // Get the copied value to the clipboard
                value = e.DataObject.GetData(typeof(string)).ToString();

                // Remove spaces in the text so that we allow text with spaces too
                // But the actual data is pasted with spaces
                value = value.Replace(" ", "").Trim();

                // Verify with Alphanumeric Regular expression
                isAlphaNumeric = System.Text.RegularExpressions.Regex.IsMatch(value, AlphaNumberRegEx);
            }

            if (!isAlphaNumeric)
            {
                // Dont allow to paste
                e.CancelCommand();
            }
        }

        /// <summary>
        /// Verifies pasting text with Alphanumeric regular expression and allows if it is alphanumeric
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">DataObjectPastingEventArgs</param>
        private static void CancelCommand(object sender, DataObjectPastingEventArgs e)
        {
            bool isAlphaNumeric = false;
            string AlphaNumberRegEx = "^[a-zA-Z0-9]+$"; // Regular expression to check AlphaNumeric values
            string value = string.Empty;
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                // Get the copied value to the clipboard
                value = e.DataObject.GetData(typeof(string)).ToString();

                // Remove spaces in the text so that we allow text with spaces too
                // But the actual data is pasted with spaces
                value = value.Replace(" ", "").Trim();

                // Verify with Alphanumeric Regular expression
                isAlphaNumeric = System.Text.RegularExpressions.Regex.IsMatch(value, AlphaNumberRegEx);
            }

            if (!isAlphaNumeric)
            {
                // Dont allow to paste
                e.CancelCommand();
            }
        }

        /// <summary>
        /// Verifies pasting text with Alphanumeric regular expression and allows if it is alphanumeric
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">DataObjectPastingEventArgs</param>
        private static void NumericCancelCommand(object sender, DataObjectPastingEventArgs e)
        {
            bool isAlphaNumeric = false;
            string AlphaNumberRegEx = "^[0-9]+$"; // Regular expression to check AlphaNumeric values
            string value = string.Empty;
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                // Get the copied value to the clipboard
                value = e.DataObject.GetData(typeof(string)).ToString();

                // Verify with Alphanumeric Regular expression
                isAlphaNumeric = System.Text.RegularExpressions.Regex.IsMatch(value, AlphaNumberRegEx);
            }

            if (!isAlphaNumeric)
            {
                // Dont allow to paste
                e.CancelCommand();
            }
        }

        public static void RmaInputCancelCommand(object sender, DataObjectSettingDataEventArgs e)
        {
            bool isAlphaNumeric = false;
            string AlphaNumberRegEx = "^[0-9]+$"; // Regular expression to check AlphaNumeric values
            string value = string.Empty;
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                // Get the copied value to the clipboard
                value = e.DataObject.GetData(typeof(string)).ToString();

                // Verify with Alphanumeric Regular expression
                isAlphaNumeric = System.Text.RegularExpressions.Regex.IsMatch(value, AlphaNumberRegEx);
            }

            if (!isAlphaNumeric)
            {
                // Dont allow to paste
                e.CancelCommand();
            }
        }

        private static void DecimalCancelCommand(object sender, DataObjectPastingEventArgs e)
        {
            bool isAlphaNumeric = false;
            string AlphaNumberRegEx = @"^[0-9]+(\.[0-9]+)?$";// @"\d+\.?\d{0,4}"; // Regular expression to check AlphaNumeric values
            string value = string.Empty;
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                // Get the copied value to the clipboard
                value = e.DataObject.GetData(typeof(string)).ToString();

                // Verify with Alphanumeric Regular expression
                isAlphaNumeric = System.Text.RegularExpressions.Regex.IsMatch(value, AlphaNumberRegEx);
            }

            if (!isAlphaNumeric)
            {
                // Dont allow to paste
                e.CancelCommand();
            }
        }

        /// <summary>
        /// Verifies pressed key is a Digit or not
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True if it is Digit</returns>
        private static bool IsDigit(Key key)
        {
            bool shiftKey = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;
            bool retVal;
            if (key >= Key.D0 && key <= Key.D9 && !shiftKey)
            {
                retVal = true;
            }
            else
            {
                retVal = key >= Key.NumPad0 && key <= Key.NumPad9;
            }
            return retVal;
        }

        private static bool IsSpecialChar(Key key)
        {
            return false;
        }

        /// <summary>
        /// Verifies pressed key is a valid letter or not
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True if it is valid letter</returns>
        private static bool IsLetter(Key key)
        {
            bool retVal = false;
            if (key >= Key.A && key <= Key.Z)
            {
                retVal = true;
            }
            return retVal;
        }

        private static void RmaTextBoxKeyDown(object sender, KeyEventArgs e)
        {

            bool handled = true;

            // Check if it is valid key, digit, letter
            if (RmacontrolKeys.Contains(e.Key) || controlKeys.Contains(e.Key) || IsDigit(e.Key) || IsLetter(e.Key))
            {
                // Allow to press
                handled = false;
            }

            // Dont allow to press
            e.Handled = handled;
        }

        private static void TextBoxKeyDown(object sender, KeyEventArgs e)
        {

            bool handled = true;

            // Check if it is valid key, digit, letter
            if (controlKeys.Contains(e.Key) || IsDigit(e.Key) || IsLetter(e.Key))
            {
                // Allow to press
                handled = false;
            }

            // Dont allow to press
            e.Handled = handled;
        }

        private static void NumericTextBoxKeyDown(object sender, KeyEventArgs e)
        {

            bool handled = true;

            // Check if it is valid key, digit, letter
            if (controlKeys.Contains(e.Key) || IsDigit(e.Key))
            {
                // Allow to press
                handled = false;
            }

            // Dont allow to press
            e.Handled = handled;
        }

        private static void DecimalTextBoxKeyDown(object sender, KeyEventArgs e)
        {

            bool handled = true;

            var textBox = (TextBox)sender;
            if (textBox == null)
                return;
            string txt = textBox.Text;
            // Check if it is valid key, digit, letter
            if ((controlKeys.Contains(e.Key) || IsDigit(e.Key)) ||
                (txt.IndexOf('.') == -1) && (e.Key == Key.OemPeriod || e.Key == Key.Decimal))
            {
                // Allow to press
                handled = false;
            }

            // Dont allow to press
            e.Handled = handled;
        }

        private static void FilterSpaceKeyDown(object sender, KeyEventArgs e)
        {
            bool handled = false;
            if (e.Key == Key.Space)
            {
                handled = true;
            }
            e.Handled = handled;
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Gets IsSpecialAlphaNumericFilterProperty
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool GetIsRmaAlphaNumericFilter(DependencyObject src)
        {
            return (bool)src.GetValue(IsRmaAlphaNumericFilterProperty);
        }



        /// <summary>
        /// Sets IsSpecialAlphaNumericFilterProperty
        /// </summary>
        /// <param name="src"></param>
        /// <param name="value"></param>
        public static void SetIsRmaAlphaNumericFilter(DependencyObject src, bool value)
        {
            src.SetValue(IsRmaAlphaNumericFilterProperty, value);
        }

        /// <summary>
        /// Gets IsAlphaNumericFilterProperty
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool GetIsDecimalFilter(DependencyObject src)
        {
            return (bool)src.GetValue(IsDecimalFilterProperty);
        }



        /// <summary>
        /// Sets IsAlphaNumericFilterProperty
        /// </summary>
        /// <param name="src"></param>
        /// <param name="value"></param>
        public static void SetIsDecimalFilter(DependencyObject src, bool value)
        {
            src.SetValue(IsDecimalFilterProperty, value);
        }

        /// <summary>
        /// Gets IsAlphaNumericFilterProperty
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool GetIsAlphaNumericFilter(DependencyObject src)
        {
            return (bool)src.GetValue(IsAlphaNumericFilterProperty);
        }



        /// <summary>
        /// Sets IsAlphaNumericFilterProperty
        /// </summary>
        /// <param name="src"></param>
        /// <param name="value"></param>
        public static void SetIsAlphaNumericFilter(DependencyObject src, bool value)
        {
            src.SetValue(IsAlphaNumericFilterProperty, value);
        }

        /// <summary>
        /// Gets IsAlphaNumericFilterProperty
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool GetIsNumericFilter(DependencyObject src)
        {
            return (bool)src.GetValue(IsAlphaNumericFilterProperty);
        }

        /// <summary>
        /// Sets IsAlphaNumericFilterProperty
        /// </summary>
        /// <param name="src"></param>
        /// <param name="value"></param>
        public static void SetIsNumericFilter(DependencyObject src, bool value)
        {
            src.SetValue(IsAlphaNumericFilterProperty, value);
        }

        /// <summary>
        /// The event occurs on IsAlphaNumericFilterProperty changed
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        public static void IsRmaAlphaNumericFilterChanged(DependencyObject src, DependencyPropertyChangedEventArgs args)
        {
            if (src != null && src is TextBox)
            {
                TextBox textBox = src as TextBox;

                InputMethod.SetIsInputMethodEnabled(src, false);

                if ((bool)args.NewValue)
                {
                    //DataObject.AddSettingDataHandler(textBox, RmaInputCancelCommand);
                    textBox.KeyDown += new KeyEventHandler(RmaTextBoxKeyDown);
                    DataObject.AddPastingHandler(textBox, RmaCancelCommand);
                }
            }
        }
        /// <summary>
        /// The event occurs on IsAlphaNumericFilterProperty changed
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        public static void IsAlphaNumericFilterChanged(DependencyObject src, DependencyPropertyChangedEventArgs args)
        {
            if (src != null && src is TextBox)
            {
                TextBox textBox = src as TextBox;

                InputMethod.SetIsInputMethodEnabled(src, false);

                if ((bool)args.NewValue)
                {
                    textBox.KeyDown += TextBoxKeyDown;
                    DataObject.AddPastingHandler(textBox, CancelCommand);
                }
            }
        }

        public static void IsDecimalFilterChanged(DependencyObject src, DependencyPropertyChangedEventArgs args)
        {
            if (src != null && src is TextBox)
            {
                TextBox textBox = src as TextBox;

                InputMethod.SetIsInputMethodEnabled(src, false);

                if ((bool)args.NewValue)
                {
                    textBox.PreviewKeyDown += FilterSpaceKeyDown;
                    textBox.KeyDown += DecimalTextBoxKeyDown;
                    DataObject.AddPastingHandler(textBox, DecimalCancelCommand);
                }
            }
        }


        /// <summary>
        /// The event occurs on IsNumericFilterProperty changed
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        public static void IsNumericFilterChanged(DependencyObject src, DependencyPropertyChangedEventArgs args)
        {
            if (src != null && src is TextBox)
            {
                TextBox textBox = src as TextBox;

                InputMethod.SetIsInputMethodEnabled(src, false);

                if ((bool)args.NewValue)
                {
                    textBox.PreviewKeyDown += FilterSpaceKeyDown;
                    textBox.KeyDown += NumericTextBoxKeyDown;
                    DataObject.AddPastingHandler(textBox, NumericCancelCommand);
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The Property indicates if the text is special alphanumeric
        /// </summary>
        public static DependencyProperty IsRmaAlphaNumericFilterProperty =
            DependencyProperty.RegisterAttached(
            "IsRmaAlphaNumericFilter", typeof(bool), typeof(TextBoxFilters),
            new PropertyMetadata(false, IsRmaAlphaNumericFilterChanged));

        /// <summary>
        /// The Property indicates if the text is alphanumeric
        /// </summary>
        public static DependencyProperty IsAlphaNumericFilterProperty =
            DependencyProperty.RegisterAttached(
            "IsAlphaNumericFilter", typeof(bool), typeof(TextBoxFilters),
            new PropertyMetadata(false, IsAlphaNumericFilterChanged));

        public static DependencyProperty IsDecimalFilterProperty =
         DependencyProperty.RegisterAttached(
         "IsDecimalFilter", typeof(bool), typeof(TextBoxFilters),
         new PropertyMetadata(false, IsDecimalFilterChanged));

        /// <summary>
        /// The Property indicates if the text is numeric
        /// </summary>
        public static DependencyProperty IsNumericFilterProperty =
            DependencyProperty.RegisterAttached(
            "IsNumericFilter", typeof(bool), typeof(TextBoxFilters),
            new PropertyMetadata(false, IsNumericFilterChanged));
        #endregion

    }
}
