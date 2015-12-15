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
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DIS.Presentation.KMT.Controls;

namespace DIS.Presentation.KMT.Behaviors
{
    /// <summary>
    /// Busy or Progress Indicator class
    /// </summary>
    public static class BusyIndicator
    {
        #region Attached Properties

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty AddMarginsProperty = DependencyProperty.RegisterAttached("AddMargins", typeof(bool),
                                                                                                           typeof(BusyIndicator),
                                                                                                           new UIPropertyMetadata(false));


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty BusyStateProperty = DependencyProperty.RegisterAttached("BusyState", typeof(bool),
                                                                                                          typeof(BusyIndicator),
                                                                                                          new UIPropertyMetadata(false, OnBusyStateChanged));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DimBackgroundProperty = DependencyProperty.RegisterAttached("DimBackground", typeof(bool),
                                                                                                              typeof(BusyIndicator),
                                                                                                              new UIPropertyMetadata(true,
                                                                                                                                     OnDimBackgroundChanged));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DimmerBrushProperty = DependencyProperty.RegisterAttached("DimmerBrush", typeof(Brush),
                                                                                                            typeof(BusyIndicator),
                                                                                                            new UIPropertyMetadata(Brushes.Black));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DimmerOpacityProperty = DependencyProperty.RegisterAttached("DimmerOpacity", typeof(double),
                                                                                                              typeof(BusyIndicator),
                                                                                                              new UIPropertyMetadata(0.5));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DimTransitionDurationProperty = DependencyProperty.RegisterAttached("DimTransitionDuration",
                                                                                                                      typeof(Duration),
                                                                                                                      typeof(BusyIndicator),
                                                                                                                      new UIPropertyMetadata(
                                                                                                                        new Duration(TimeSpan.FromSeconds(0.5))));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TargetVisualProperty = DependencyProperty.RegisterAttached("TargetVisual", typeof(UIElement),
                                                                                                             typeof(BusyIndicator),
                                                                                                             new UIPropertyMetadata(null));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ProcessingTextProperty = DependencyProperty.RegisterAttached("ProcessingText", typeof(string),
                                                                                                             typeof(BusyIndicator),
                                                                                                             new UIPropertyMetadata("Processing, please wait...", OnProcessingTextChanged));

        /// <summary>
        /// GetProcessingText
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetProcessingText(DependencyObject obj)
        {
            return obj.GetValue(ProcessingTextProperty).ToString();
        }

        /// <summary>
        /// SetProcessingText
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetProcessingText(DependencyObject obj, string value)
        {
            obj.SetValue(ProcessingTextProperty, value);
        }

        /// <summary>
        /// GetDimmerOpacity
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double GetDimmerOpacity(DependencyObject obj)
        {
            return (double)obj.GetValue(DimmerOpacityProperty);
        }

        /// <summary>
        /// SetDimmerOpacity
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetDimmerOpacity(DependencyObject obj, double value)
        {
            obj.SetValue(DimmerOpacityProperty, value);
        }

        /// <summary>
        /// GetAddMargins
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetAddMargins(DependencyObject obj)
        {
            return (bool)obj.GetValue(AddMarginsProperty);
        }

        /// <summary>
        /// SetAddMargins
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetAddMargins(DependencyObject obj, bool value)
        {
            obj.SetValue(AddMarginsProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Duration GetDimTransitionDuration(DependencyObject obj)
        {
            return (Duration)obj.GetValue(DimTransitionDurationProperty);
        }

        /// <summary>
        /// SetDimTransitionDuration
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetDimTransitionDuration(DependencyObject obj, Duration value)
        {
            obj.SetValue(DimTransitionDurationProperty, value);
        }

        /// <summary>
        /// GetDimmerBrush
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Brush GetDimmerBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(DimmerBrushProperty);
        }

        /// <summary>
        /// SetDimmerBrush
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetDimmerBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(DimmerBrushProperty, value);
        }

        /// <summary>
        /// GetDimBackground
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetDimBackground(DependencyObject obj)
        {
            return (bool)obj.GetValue(DimBackgroundProperty);
        }

        /// <summary>
        /// SetDimBackground
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetDimBackground(DependencyObject obj, bool value)
        {
            obj.SetValue(DimBackgroundProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static UIElement GetTargetVisual(DependencyObject obj)
        {
            return (UIElement)obj.GetValue(TargetVisualProperty);
        }

        /// <summary>
        /// SetTargetVisual
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetTargetVisual(DependencyObject obj, UIElement value)
        {
            obj.SetValue(TargetVisualProperty, value);
        }

        /// <summary>
        /// GetBusyState
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetBusyState(DependencyObject obj)
        {
            return (bool)obj.GetValue(BusyStateProperty);
        }

        /// <summary>
        /// SetBusyState
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetBusyState(DependencyObject obj, bool value)
        {
            obj.SetValue(BusyStateProperty, value);
        }

        /// <summary>
        /// ProgressIndicator
        /// </summary>
        public static ProgressIndicator ProgressIndicator { get; set; }

        #endregion

        #region Implementation

        /// <summary>
        /// OnDimBackgroundChanged
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnDimBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool shouldDimBackground = (bool)e.NewValue;
            bool wasDimmingBackground = (bool)e.OldValue;

            if (shouldDimBackground == wasDimmingBackground)
            {
                return;
            }

            if (!GetBusyState(d))
            {
                return;
            }

            var hostGridObject = (GetTargetVisual(d) ?? d);
            Debug.Assert(hostGridObject != null);

            var hostGrid = hostGridObject as Grid;
            if (hostGrid != null)
            {
                var grid = (Grid)LogicalTreeHelper.FindLogicalNode(hostGrid, "BusyIndicator");

                if (grid != null)
                {
                    var dimmer = (Rectangle)LogicalTreeHelper.FindLogicalNode(grid, "Dimmer");

                    if (dimmer != null)
                    {
                        dimmer.Visibility = (shouldDimBackground ? Visibility.Visible : Visibility.Collapsed);
                    }

                    if (shouldDimBackground)
                    {
                        grid.Cursor = Cursors.Wait;
                        grid.ForceCursor = true;

                        InputManager.Current.PreProcessInput += OnPreProcessInput;
                    }
                    else
                    {
                        grid.Cursor = Cursors.Arrow;
                        grid.ForceCursor = false;

                        InputManager.Current.PreProcessInput -= OnPreProcessInput;
                    }
                }
            }
        }

        /// <summary>
        /// OnBusyStateChanged
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnBusyStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool isBusy = (bool)e.NewValue;
            bool wasBusy = (bool)e.OldValue;

            if (isBusy == wasBusy)
            {
                return;
            }

            var hostGridObject = (GetTargetVisual(d) ?? d);
            Debug.Assert(hostGridObject != null);

            var hostGrid = hostGridObject as Grid;
            if (hostGrid == null)
            {
                throw new InvalidCastException(
                    string.Format(
                        "The object being attached to must be of type {0}. Try embedding your visual inside a {0} control, and attaching the behavior to the {0} instead.",
                        typeof(Grid).Name));
            }

            if (isBusy)
            {
                Debug.Assert(LogicalTreeHelper.FindLogicalNode(hostGrid, "BusyIndicator") == null);

                bool dimBackground = GetDimBackground(d);
                var grid = new Grid
                {
                    Name = "BusyIndicator",
                    Opacity = 0.0
                };
                if (dimBackground)
                {
                    grid.Cursor = Cursors.Wait;
                    grid.ForceCursor = true;

                    InputManager.Current.PreProcessInput += OnPreProcessInput;
                }
                grid.SetBinding(FrameworkElement.WidthProperty, new Binding("ActualWidth")
                {
                    Source = hostGrid
                });
                grid.SetBinding(FrameworkElement.HeightProperty, new Binding("ActualHeight")
                {
                    Source = hostGrid
                });
                for (int i = 1; i <= 3; ++i)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    });
                    grid.RowDefinitions.Add(new RowDefinition
                    {
                        Height = new GridLength(1, GridUnitType.Star)
                    });
                }
                ProgressIndicator = new ProgressIndicator() { Width = 150, Height = 120 };

                var viewbox = new Viewbox
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Stretch = Stretch.Uniform,
                    StretchDirection = StretchDirection.Both,
                    Child = ProgressIndicator
                };
                grid.SetValue(Panel.ZIndexProperty, 1000);
                grid.SetValue(Grid.RowSpanProperty, Math.Max(1, hostGrid.RowDefinitions.Count));
                grid.SetValue(Grid.ColumnSpanProperty, Math.Max(1, hostGrid.ColumnDefinitions.Count));
                if (GetAddMargins(d))
                {
                    viewbox.SetValue(Grid.RowProperty, 1);
                    viewbox.SetValue(Grid.ColumnProperty, 1);
                }
                else
                {
                    viewbox.SetValue(Grid.RowSpanProperty, 3);
                    viewbox.SetValue(Grid.ColumnSpanProperty, 3);
                }
                viewbox.SetValue(Panel.ZIndexProperty, 1);

                var dimmer = new Rectangle
                {
                    Name = "Dimmer",
                    Opacity = GetDimmerOpacity(d),
                    Fill = GetDimmerBrush(d),
                    Visibility = (dimBackground ? Visibility.Visible : Visibility.Collapsed)
                };
                dimmer.SetValue(Grid.RowSpanProperty, 3);
                dimmer.SetValue(Grid.ColumnSpanProperty, 3);
                dimmer.SetValue(Panel.ZIndexProperty, 0);
                grid.Children.Add(dimmer);

                grid.Children.Add(viewbox);

                grid.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(1.0, GetDimTransitionDuration(d)));

                hostGrid.Children.Add(grid);
            }
            else
            {
                var grid = (Grid)LogicalTreeHelper.FindLogicalNode(hostGrid, "BusyIndicator");

                Debug.Assert(grid != null);

                if (grid != null)
                {
                    grid.Name = string.Empty;

                    var fadeOutAnimation = new DoubleAnimation(0.0, GetDimTransitionDuration(d));
                    fadeOutAnimation.Completed += (sender, args) => OnFadeOutAnimationCompleted(d, hostGrid, grid);
                    grid.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
                }
            }
        }

        /// <summary>
        /// OnProcessingTextChanged
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnProcessingTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (null != ProgressIndicator && null != e.NewValue)
            {
                ProgressIndicator.Text = e.NewValue.ToString();
            }
        }

        /// <summary>
        /// OnPreProcessInput
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnPreProcessInput(object sender, PreProcessInputEventArgs e)
        {
            if (e.StagingItem.Input.Device != null)
            {
                e.Cancel();
            }
        }

        /// <summary>
        /// OnFadeOutAnimationCompleted
        /// </summary>
        /// <param name="d"></param>
        /// <param name="hostGrid"></param>
        /// <param name="busyIndicator"></param>
        private static void OnFadeOutAnimationCompleted(DependencyObject d, Panel hostGrid, UIElement busyIndicator)
        {
            bool dimBackground = GetDimBackground(d);

            hostGrid.Children.Remove(busyIndicator);

            if (dimBackground)
            {
                InputManager.Current.PreProcessInput -= OnPreProcessInput;
            }
        }

        #endregion
    }
}
