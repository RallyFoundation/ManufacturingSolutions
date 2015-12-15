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

// Copyright © Microsoft Corporation 2009
// Distrubted under the Microsoft Permissive License 
//
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DIS.Presentation.KMT.Behaviors
{
    /// <summary>
    ///  Bizzy spinner direction enumeration
    /// </summary>
    public enum BizzySpinnerDirection
    {
        /// <summary>
        /// 
        /// </summary>
        Clockwise,
        
        /// <summary>
        /// 
        /// </summary>
        CounterClockwise
    };

    /// <summary>
    /// Bizzy spinner class
    /// </summary>
    public class BizzySpinner : Control
    {
        #region General Private Members

        private DoubleAnimation spinAnimation = null;

        #endregion

        #region Construction

        static BizzySpinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BizzySpinner), new FrameworkPropertyMetadata(typeof(BizzySpinner)));
        }

        /// <summary>
        /// 
        /// </summary>
        public BizzySpinner()
            : base()
        {
            IsEnabledChanged += IsEnabledChangedHandler;
        }

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        public event DependencyPropertyChangedEventHandler IsSpinChanged;

        /// <summary>
        /// 
        /// </summary>
        public event DependencyPropertyChangedEventHandler IsSpinningChanged;

        #endregion

        #region Enbable State Change Handler

        private Brush BackgroundBrushSave;
        private Brush LeaderBrushSave;
        private Brush TailBrushSave;

        private void IsEnabledChangedHandler(Object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.OldValue) && !((bool)e.NewValue))
            {
                //
                // Going disabled
                //
                BackgroundBrushSave = Background;
                LeaderBrushSave = LeaderBrush;
                TailBrushSave = TailBrush;

                Background = DisabledBackgroundBrush;
                LeaderBrush = DisabledLeaderBrush;
                TailBrush = DisabledTailBrush;

                ControlSpinning(false);
            }

            if (!((bool)e.OldValue) && ((bool)e.NewValue))
            {
                //
                // Going enabedl
                //
                Background = BackgroundBrushSave;
                LeaderBrush = LeaderBrushSave;
                TailBrush = TailBrushSave;

                // The control is enabled, turn on spinning if the Spin property is ture
                ControlSpinning(Spin);
            }
        }

        #endregion

        #region Dependancy Properties
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region Angle Dependancy Property

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            "Angle",                                                   // The name of the dependency property to register
            typeof(double),                                            // The type of the property
            typeof(BizzySpinner),                                      // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(                             // Property metadata for the dependency property;;
                0.0,                                                   // default value
                new PropertyChangedCallback(OnAnglePropertyChanged),
                new CoerceValueCallback(AngleCoerceCallback)
                )
            );

        /// <summary>
        /// 
        /// </summary>
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        private static void OnAnglePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BizzySpinner me = (BizzySpinner)d;

            if (!me.Spin && me.IsEnabled)
            {
                me.SpinAngle = (double)e.NewValue;
            }
        }

        private static Object AngleCoerceCallback(DependencyObject d, Object baseValue)
        {
            if (!(baseValue is double))
            {
                return DependencyProperty.UnsetValue;
            }

            double b = (double)baseValue;

            double v = b % 360.0;

            return v;
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region Spin Dependancy Property

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SpinProperty = DependencyProperty.Register(
            "Spin",                             // The name of the dependency property to register
            typeof(bool),                       // The type of the property
            typeof(BizzySpinner),               // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(      // Property metadata for the dependency property
                false,
                new PropertyChangedCallback(OnSpinPropertyChanged)
                )
            );

        /// <summary>
        /// 
        /// </summary>
        public bool Spin
        {
            get { return (bool)GetValue(SpinProperty); }
            set { SetValue(SpinProperty, value); }
        }

        private static void OnSpinPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BizzySpinner me = (BizzySpinner)d;

            if ((bool)e.NewValue)
            {
                // Spin is true: turn spin on if this control is enabled
                me.ControlSpinning(me.IsEnabled);
            }

            if (!(bool)e.NewValue)
            {
                me.ControlSpinning(false);
            }

            if (me.IsSpinChanged != null)
            {
                me.IsSpinChanged(me, e);
            }
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region Spinning Dependancy Property

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyPropertyKey SpinningPropertyKey = DependencyProperty.RegisterReadOnly(
            "Spinning",                         // The name of the dependency property to register
            typeof(bool),                       // The type of the property
            typeof(BizzySpinner),               // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(      // Property metadata for the dependency property
                false,
                new PropertyChangedCallback(OnSpinningPropertyChanged)
                )
            );

        /// <summary>
        /// 
        /// </summary>
        public bool Spinning
        {
            get { return (bool)GetValue(SpinningPropertyKey.DependencyProperty); }
        }

        private static void OnSpinningPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BizzySpinner me = (BizzySpinner)d;

            if (me.IsSpinningChanged != null)
            {
                me.IsSpinningChanged(me, e);
            }
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region SpinRate Dependancy Property

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SpinRateProperty = DependencyProperty.Register(
            "SpinRate",                         // The name of the dependency property to register
            typeof(double),                     // The type of the property
            typeof(BizzySpinner),               // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(      // Property metadata for the dependency property
                1.0,                                                     // default 1 second
                FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnSpinRatePropertyChanged),
                new CoerceValueCallback(SpinRateCoerceCallback)
                )
            );

        /// <summary>
        ///  How long it takes for one revoluation.  In seconds.
        /// </summary>
        public double SpinRate
        {
            get { return (double)GetValue(SpinRateProperty); }
            set { SetValue(SpinRateProperty, value); }
        }

        private static void OnSpinRatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BizzySpinner me = (BizzySpinner)d;

            if (me.Spinning)
            {
                if (me.spinAnimation != null)
                {

                    Duration duration = TimeSpan.FromSeconds((double)e.NewValue);

                    if (me.spinAnimation.Duration != duration)
                    {

                        me.spinAnimation.Duration = duration;

                        me.ReBeginSpinningAnimation();
                    }
                }
            }
        }

        private static Object SpinRateCoerceCallback(DependencyObject d, Object baseValue)
        {
            if (!(baseValue is double))
            {
                return DependencyProperty.UnsetValue;
            }

            double v = (double)baseValue;

            if (v <= 0.0)
            {
                return DependencyProperty.UnsetValue;
            }

            return v;
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region SpinAngle Dependancy Property

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SpinAngleProperty = DependencyProperty.Register(
            "SpinAngle",                               // The name of the dependency property to register
            typeof(double),                            // The type of the property
            typeof(BizzySpinner),                      // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(             // Property metadata for the dependency property;;
                0.0
                )
            );

        /// <summary>
        /// 
        /// </summary>
        public double SpinAngle
        {
            get { return (double)GetValue(SpinAngleProperty); }
            private set { SetValue(SpinAngleProperty, value); }
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region SpinDirection Dependancy Property

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SpinDirectionProperty = DependencyProperty.Register(
            "SpinDirection",                           // The name of the dependency property to register
            typeof(BizzySpinnerDirection),             // The type of the property
            typeof(BizzySpinner),                      // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(             // Property metadata for the dependency property;;
                BizzySpinnerDirection.Clockwise
                )
            );

        /// <summary>
        /// 
        /// </summary>
        public BizzySpinnerDirection SpinDirection
        {
            get { return (BizzySpinnerDirection)GetValue(SpinDirectionProperty); }
            set { SetValue(SpinDirectionProperty, value); }
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region LeaderBrush Dependancy Property

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty LeaderBrushProperty = DependencyProperty.Register(
            "LeaderBrush",                      // The name of the dependency property to register
            typeof(Brush),                      // The type of the property
            typeof(BizzySpinner),               // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(      // Property metadata for the dependency property;;
                new SolidColorBrush(Colors.Red)
                )
            );

        /// <summary>
        /// 
        /// </summary>
        public Brush LeaderBrush
        {
            get { return (Brush)GetValue(LeaderBrushProperty); }
            set { SetValue(LeaderBrushProperty, value); }
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region TailBrush Dependancy Property

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TailBrushProperty = DependencyProperty.Register(
            "TailBrush",                      // The name of the dependency property to register
            typeof(Brush),                      // The type of the property
            typeof(BizzySpinner),               // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(      // Property metadata for the dependency property;;
                new SolidColorBrush(Colors.Green)
                )
            );

        /// <summary>
        /// 
        /// </summary>
        public Brush TailBrush
        {
            get { return (Brush)GetValue(TailBrushProperty); }
            set { SetValue(TailBrushProperty, value); }
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region DisabledLeaderBrush Dependancy Property

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DisabledLeaderBrushProperty = DependencyProperty.Register(
            "DisabledLeaderBrush",                       // The name of the dependency property to register
            typeof(Brush),                               // The type of the property
            typeof(BizzySpinner),                        // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(               // Property metadata for the dependency property;;
                new SolidColorBrush(Colors.DarkGray)   // Default value
                )
            );

        /// <summary>
        /// 
        /// </summary>
        public Brush DisabledLeaderBrush
        {
            get { return (Brush)GetValue(DisabledLeaderBrushProperty); }
            set { SetValue(DisabledLeaderBrushProperty, value); }
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region DisabledTailBrush Dependancy Property

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DisabledTailBrushProperty = DependencyProperty.Register(
            "DisabledTailBrush",                        // The name of the dependency property to register
            typeof(Brush),                              // The type of the property
            typeof(BizzySpinner),                       // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(              // Property metadata for the dependency property;;
                new SolidColorBrush(Colors.LightGray)   // Default value
                )
            );

        /// <summary>
        /// 
        /// </summary>
        public Brush DisabledTailBrush
        {
            get { return (Brush)GetValue(DisabledTailBrushProperty); }
            set { SetValue(DisabledTailBrushProperty, value); }
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        #region DisabledBackgroundBrush Dependancy Property

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DisabledBackgroundBrushProperty = DependencyProperty.Register(
            "DisabledBackgroundBrush",                  // The name of the dependency property to register
            typeof(Brush),                              // The type of the property
            typeof(BizzySpinner),                       // The owner type that is registering the dependency property.
            new FrameworkPropertyMetadata(              // Property metadata for the dependency property;;
                null                                    // Default value
                )
            );

        /// <summary>
        /// 
        /// </summary>
        public Brush DisabledBackgroundBrush
        {
            get { return (Brush)GetValue(DisabledBackgroundBrushProperty); }
            set { SetValue(DisabledBackgroundBrushProperty, value); }
        }

        #endregion
        #endregion

        #region Animation Control

        private double OneRotation
        {
            get
            {
                double direction = (SpinDirection == BizzySpinnerDirection.Clockwise) ? 1 : -1;
                return 360.0 * direction;
            }
        }

        void ReBeginSpinningAnimation()
        {
            spinAnimation.Duration = new Duration(TimeSpan.FromSeconds(SpinRate));
            spinAnimation.From = SpinAngle;
            spinAnimation.To = SpinAngle + OneRotation;

            //
            // From the MSDN documenation on Storyboard.Pause(): Calling the Begin method again 
            // replaces the paused storyboard with a new one, which has the appearance of resuming it. 
            //
            // From the MSND documention on Storybaord.begin(): If the targeted properties are already animated, 
            // they are replaced using the SnapshotAndReplace handoff behavior. 
            //
            this.BeginAnimation(SpinAngleProperty, spinAnimation);
        }

        /// <summary>Turns Spinning on and off</summary>
        /// <param name="shouldSpin">True to enable spinning</param>
        void ControlSpinning(bool shouldSpin)
        {
            if (shouldSpin)
            {
                if (!Spinning)
                {
                    if (spinAnimation != null)
                    {

                        this.BeginAnimation(SpinAngleProperty, spinAnimation);

                    }
                    else
                    {
                        // Create a new animation and storyboard

                        SpinAngle = Angle;

                        spinAnimation = new DoubleAnimation();
                        spinAnimation.RepeatBehavior = RepeatBehavior.Forever;

                        ReBeginSpinningAnimation();
                    }

                    SetValue(SpinningPropertyKey, true); // Spinning = true;
                }
            }
            else
            {
                if (spinAnimation != null)
                {
                    if (Spinning)
                    {
                        //
                        // We have to remember the current spin angle.  Why?  Becuase the animation will
                        // reset the SpinAngleProperty back to its base value when it is removed
                        // from the SpinAngleProperty.  The SpinAngleProperty base value is the 
                        // animations from value. 
                        //
                        double tmp = SpinAngle % OneRotation;

                        this.BeginAnimation(SpinAngleProperty, null);

                        Angle = tmp;
                        SpinAngle = tmp;

                        SetValue(SpinningPropertyKey, false); // Spinning = false;

                        //
                        // We have to 'forget' the spin animation here because once set, it never
                        // forget's the SpinAngleProperty base value so we will get jerks when
                        // spin is renabled.  Said another way, simply removing the animation
                        // from the perorty doesn't cause it to forget its state.  So, we must
                        // throw away the current animation and create a new one next time
                        // spinning is re-enabled.
                        //
                        spinAnimation = null;
                    }
                }
            }
        }
        #endregion
    }
}
