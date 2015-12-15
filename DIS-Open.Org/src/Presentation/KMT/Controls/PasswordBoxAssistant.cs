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

using System.Windows;
using System.Windows.Controls;

namespace DIS.Presentation.KMT.Controls
{
    /// <summary>
    /// PasswordBox control binding assistant
    /// </summary>
    public static class PasswordBoxAssistant
    {
        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty BoundPassword =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxAssistant), new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty BindPassword = DependencyProperty.RegisterAttached(
            "BindPassword", typeof(bool), typeof(PasswordBoxAssistant), new PropertyMetadata(false, OnBindPasswordChanged));

        #endregion

        #region Priviate & Protected member variables

        private static readonly DependencyProperty UpdatingPassword =
            DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordBoxAssistant));

        #endregion

        #region Private & Protected methods

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox box = d as PasswordBox;
            if (d == null || !GetBindPassword(d))
            {
                return;
            }
            box.PasswordChanged -= HandlePasswordChanged;
            string newPassword = (string)e.NewValue;
            if (!GetUpdatingPassword(box))
            {
                box.Password = newPassword;
            }
            box.PasswordChanged += HandlePasswordChanged;
        }

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox box = dp as PasswordBox;
            if (box == null)
            {
                return;
            }
            bool wasBound = (bool)(e.OldValue);
            bool needToBind = (bool)(e.NewValue);
            if (wasBound)
            {
                box.PasswordChanged -= HandlePasswordChanged;
            }
            if (needToBind)
            {
                box.PasswordChanged += HandlePasswordChanged;
            }
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;

            SetUpdatingPassword(box, true);
            SetBoundPassword(box, box.Password);
            SetUpdatingPassword(box, false);
        }

        private static bool GetUpdatingPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(UpdatingPassword);
        }

        private static void SetUpdatingPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdatingPassword, value);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="value"></param>
        public static void SetBindPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(BindPassword, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static bool GetBindPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(BindPassword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static string GetBoundPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(BoundPassword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="value"></param>
        public static void SetBoundPassword(DependencyObject dp, string value)
        {
            dp.SetValue(BoundPassword, value);
        }

        #endregion
    }
}
