using DllComponent;
using DllLog;
//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace TouchTest
{

    public partial class MainForm : Form
    {

        #region Fields
        private static ResourceManager LocRM;

        private static bool PassPhaseOne = false; //flag: false -> run phase; true -> do not run phase
        private static bool PassPhaseTwo = false; //flag: false -> run phase; true -> do not run phase
        private static bool PassPhaseThree = false; //flag: false -> run phase; true -> do not run phase
        private static int PhaseTwo_point = 0;
        private static int PhaseThree_point = 0;
        private IList<string> threePoints = new List<string>();

        private int touchInputSize;        // size of TOUCHINPUT structure
        private const int WM_TOUCH = 0x0240;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainForm form class.
        /// </summary>
        public MainForm()
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);

            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
           
            Load += new System.EventHandler(this.OnLoadHandler);

            // GetTouchInputInfo needs to be
            // passed the size of the structure it will be filling.
            // We get the size upfront so it can be used later.
            touchInputSize = Marshal.SizeOf(new NativeMethods.TOUCHINPUT());

            SetString();
            CheckArguments();
        }

        #endregion // Constructor

        /// <summary>
        /// Gets Windows message. Catch for Touch messages. 
        /// </summary>
        /// <param name="m">The Windows Message to process.</param>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            // Decode and handle WM_TOUCH message.
            switch (m.Msg)
            {
                case WM_TOUCH:
                    DecodeTouch(ref m);
                    break;
                case 0x201:
                    DecodeMouseClick(ref m); //left button up, ie. a click
                    break;
                default:
                    break;
            }

            // Call parent WndProc for default message processing.
            base.WndProc(ref m);

        }

        /// <summary>
        /// Decode Touch messages. Clear control based on position of Touch action.  
        /// </summary>
        /// <param name="m">The Windows Message to decode.</param>
        private void DecodeTouch(ref Message m)
        {
            int inputCount = LoWord(m.WParam.ToInt32()); // Number of touch inputs, actual per-contact messages
            NativeMethods.TOUCHINPUT[] inputs; // Array of TOUCHINPUT structures
            inputs = new NativeMethods.TOUCHINPUT[inputCount]; // Allocate the storage for the parameters of the per-contact messages

            if (!NativeMethods.GetTouchInputInfo(m.LParam, inputCount, inputs, touchInputSize))
            {
                // Get touch info failed.
                return;
            }

            if (!PassPhaseOne) //Phase one: clear grid
            {
                PhaseOne(inputs);
            }
            else if (!PassPhaseTwo) //Phase two: multitouch
            {
                PhaseTwo(inputCount);
            }
            else if (!PassPhaseThree)  //Phase three: 3-point swipe
            {
                PhaseThree(inputs);

            }
            else //all 3 phases passed
            {
                Program.ExitApplication(0);
            }
            
        }

        /// <summary>
        /// 1st phase of test: Clear all grids by touch input  
        /// </summary>
        private void PhaseOne(NativeMethods.TOUCHINPUT[] touchInputs)
        {
            for (int i = 0; i < touchInputs.Length; i++)
            {
                Point screenPoint = new Point(touchInputs[i].x / 100, touchInputs[i].y / 100);
                Point winPoint = TouchPanel.PointToClient(screenPoint);
                Control c = TouchPanel.GetChildAtPoint(winPoint);
                if (c != null && c.BackColor == Color.YellowGreen)
                {
                    c.Dispose();
                    if (TouchPanel.Controls.Count == 4)
                    {
                        InstructionLbl.Text = "Pass";
                        PassPhaseOne = true;
                    }
                }
            }
        }

        /// <summary>
        /// 2nd phase of test: check multitouch. By default 5 point touch to pass test. 
        /// </summary>
        private void PhaseTwo(int touchInputsCount)
        {
            if (PhaseTwo_point == 0)
            {
                PassPhaseTwo = true;
            } 
            else if (PhaseTwo_point >= 1)
            {
                int touchCount = PhaseTwo_point;
                InstructionLbl.Text = touchCount + LocRM.GetString("TouchTest2") + touchInputsCount;
                if (touchCount == touchInputsCount)
                {
                    InstructionLbl.Text = "Pass";
                    PassPhaseTwo = true;
                }
            }
        }

        /// <summary>
        /// 3rd phase of test: check 3 finger swipe. 
        /// </summary>
        private void PhaseThree(NativeMethods.TOUCHINPUT[] touchInputs)
        {
            if (PhaseThree_point == 0)
            {
                PassPhaseThree = true;
            } 
             else if (PhaseThree_point >= 1)
            {
                InstructionLbl.Text = string.Format(LocRM.GetString("TouchTest3"), PhaseThree_point);
                LeftPanel.Visible = true;
                RightPanel.Visible = true;
                TouchPanel.SetRowSpan(LeftPanel, 5);
                TouchPanel.SetRowSpan(RightPanel, 5);

                int rightPointsCount = 0;

                //if less than 3 touch point, restart over
                if (touchInputs.Length < PhaseThree_point)
                {
                    LeftPanel.BackColor = Color.Crimson;
                    threePoints.Clear();
                    rightPointsCount = 0;
                }
                else
                {
                    for (int i = 0; i < touchInputs.Length; i++)
                    {
                        Point screenPoint = new Point(touchInputs[i].x / 100, touchInputs[i].y / 100);
                        Point winPoint = TouchPanel.PointToClient(screenPoint);
                        Control c = TouchPanel.GetChildAtPoint(winPoint);

                        if (c == LeftPanel && touchInputs.Length >= PhaseThree_point && LeftPanel.BackColor == Color.Crimson)
                        {
                            if (!threePoints.Contains(touchInputs[i].dwID.ToString()))
                            {
                                threePoints.Add(touchInputs[i].dwID.ToString());
                            }
                            if (threePoints.Count >= PhaseThree_point)
                            {
                                LeftPanel.BackColor = Color.YellowGreen;
                            }
                        }

                        if (c == RightPanel && touchInputs.Length >= PhaseThree_point && LeftPanel.BackColor == Color.YellowGreen)
                        {
                            if (threePoints.Contains(touchInputs[i].dwID.ToString()))
                            {
                                rightPointsCount++;
                                if (rightPointsCount >= PhaseThree_point)
                                {
                                    RightPanel.BackColor = Color.YellowGreen;
                                    InstructionLbl.Text = "Pass";
                                    PassPhaseThree = true;
                                }
                            }
                        }

                    }//end of for loop on touchpoints
                }
            }
        }


        /// <summary>
        /// Decode Mouse Click messages. If Exit clicked, exit application with result = Fail
        /// </summary>
        /// <param name="m">The Windows Message to decode.</param>
        private void DecodeMouseClick(ref Message m)
        {
            Point point = new Point(m.LParam.ToInt32());
            Control c = TouchPanel.GetChildAtPoint(point);
            if (c != null && c == ExitLbl)
            {
                Program.ExitApplication(255);
            }
        }


        /// <summary>
        /// Form load event. On load register touch window and DisableTouch
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void OnLoadHandler(Object sender, EventArgs e)
        {
            try
            {
                // Registering the window for multi-touch, using the default settings.
                // p/invoking into user32.dll
                if (!NativeMethods.RegisterTouchWindow(this.Handle, 0))
                {
                    Log.LogError("Could not register window for multi-touch");
                }
            }
            catch (Exception exception)
            {
                Log.LogError("RegisterTouchWindow API not available");
                Log.LogError(exception.ToString());
                MessageBox.Show("RegisterTouchWindow API not available", "MTScratchpadWMTouch ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
                Log.LogError("RegisterTouchWindow API not available");

            }

            CoreComponent component = null;
            try
            {

                component = new CoreComponent();
                component.DisableTouch(NativeMethods.GetActiveWindow());
            }
            catch (Exception ex)
            {
                Log.LogError("Cannot disable edge UI " + ex.ToString());
            }
            finally
            {
                if (component != null)
                {
                    component.Dispose();
                    component = null;
                }
            }
        }

        /// <summary>
        /// Retrieves the low-order word from the specified value.
        /// </summary>
        private static int LoWord(int number)
        {
            return (number & 0xffff);
        }

        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        private void SetString()
        {
            ExitLbl.Text = LocRM.GetString("Exit");
            InstructionLbl.Text = LocRM.GetString("TouchTest1");

        }

        private static void CheckArguments()
        {
            //sample arguments: [0:"zh-Hans", 1:"5", 2:"3"]
            if (Program.ProgramArgs == null)
            {
                return;
            }
            if (Program.ProgramArgs.Count == 3)
            {
                try
                {
                    int point_2 = Convert.ToInt32(Program.ProgramArgs[1]);
                    if ( point_2 > 0)
                    {
                        PhaseTwo_point = point_2;
                    }
                }
                catch (Exception)
                {
                    PassPhaseTwo = true;
                    Log.LogComment(Log.LogLevel.Warning, "No multitouch input");
                    //log warning
                }
                try
                {
                    int point_3 = Convert.ToInt32(Program.ProgramArgs[2]);
                    if (point_3  > 0)
                    {
                        PhaseThree_point = point_3;
                    }
                }
                catch (Exception)
                {
                    PassPhaseThree = true;
                    Log.LogComment(Log.LogLevel.Warning, "No multitouch swipe input");
                    //log warning
                }      
            }            
        }

    }

    /// <summary>
    /// Using [DllImport("user32")] API
    /// </summary>
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern IntPtr GetActiveWindow();

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool RegisterTouchWindow(IntPtr hWnd, uint ulFlags);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetTouchInputInfo(System.IntPtr hTouchInput, int cInputs, [In, Out] TOUCHINPUT[] pInputs, int cbSize);

        // Touch API defined structures [winuser.h]
        [StructLayout(LayoutKind.Sequential)]
        internal struct TOUCHINPUT
        {
            public int x;
            public int y;
            private System.IntPtr hSource;
            public int dwID;
            private int dwFlags;
            private int dwMask;
            private int dwTime;
            private System.IntPtr dwExtraInfo;
            private int cxContact;
            private int cyContact;
        }


    }

}
