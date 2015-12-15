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

using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;

namespace DIS.Presentation.KMT.Behaviors
{
    /// <summary>
    /// Event Focus class to set focus after button click
    /// </summary>
    public class EventFocusAttachment
    {
        #region Public methods
        
        /// <summary>
        /// Gets ElementToFocusProperty
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static Control GetElementToFocus(Button button)
        {
            return (Control)button.GetValue(ElementToFocusProperty);
        }

        /// <summary>
        /// Sets ElementToFocusProperty
        /// </summary>
        /// <param name="button"></param>
        /// <param name="value"></param>
        public static void SetElementToFocus(Button button, Control value)
        {
            button.SetValue(ElementToFocusProperty, value);
        }

        /// <summary>
        /// The Property indicates the element to focus
        /// </summary>
        public static readonly DependencyProperty ElementToFocusProperty =
            DependencyProperty.RegisterAttached("ElementToFocus", typeof(Control),
            typeof(EventFocusAttachment), new UIPropertyMetadata(null, ElementToFocusPropertyChanged));

        /// <summary>
        /// The event occurs on element to focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ElementToFocusPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                button.Click += (s, args) =>
                {
                    Control control = GetElementToFocus(button);
                    if (control != null)
                    {
                        control.Focus();
                    }
                };
            }
        }

        #endregion
    }
}
