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
using System.Runtime.InteropServices;
using System.Collections;

namespace DIS.Common.ConfigurationProtectedTool
{
    // Constants used with PInvoke methods
    internal class Constants
    {
        // Standard input, output, and error
        internal const int STD_INPUT_HANDLE = -10;
        internal const int STD_OUTPUT_HANDLE = -11;
        internal const int STD_ERROR_HANDLE = -12;

        //  Input Mode flags.
        internal const int ENABLE_WINDOW_INPUT = 0x0008;
        internal const int ENABLE_MOUSE_INPUT = 0x0010;

        //  EventType flags.
        internal const int KEY_EVENT = 0x0001; // Event contains key event record
        internal const int MOUSE_EVENT = 0x0002; // Event contains mouse event record
        internal const int WINDOW_BUFFER_SIZE_EVENT = 0x0004; // Event contains window change event record
        internal const int MENU_EVENT = 0x0008; // Event contains menu event record
        internal const int FOCUS_EVENT = 0x0010; // event contains focus change

        // Returned by GetStdHandle when an error occurs
        internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
    }

    // Struct uChar is meant to support the Windows Console API's uChar union.
    // Unions do not exist in the pure .NET world. We have to use the regular 
    // C# struct and the StructLayout and FieldOffset Attributes to preserve
    // the memory layout of the unmanaged union.
    // 
    // We specify the "LayoutKind.Explicit" value for the StructLayout attribute 
    // to specify that every field of the struct uChar is marked with a byte offset.
    // 
    // This byte offset is specified by the FieldOffsetAttribute and it indicates
    // the number of bytes between the beginning of the struct in memory and the
    // beginning of the field.
    //
    // As you can see in the struct uChar (below), the fields "UnicodeChar"
    // and "AsciiChar" have been marked as being of offset 0. This is the only
    // way that an unmanaged C/C++ union can be represented in C#.
    //
    [StructLayout(LayoutKind.Explicit)]
    internal struct uCharUnion
    {
        [FieldOffset(0)]
        internal ushort UnicodeChar;
        [FieldOffset(0)]
        internal byte AsciiChar;
    }

    // The struct KEY_EVENT_RECORD is used to report keyboard input events 
    // in a console INPUT_RECORD structure.
    //
    // Internally, it uses the structure uChar which is treated as a union
    // in the unmanaged world.
    // 
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct KEY_EVENT_RECORD
    {
        internal int bKeyDown;
        internal ushort wRepeatCount;
        internal ushort wVirtualKeyCode;
        internal ushort wVirtualScanCode;
        internal uCharUnion uchar;
        internal uint dwControlKeyState;
    }

    // The other stuctures are not used within our application.
    internal struct COORD
    {
        internal short X;
        internal short Y;
    }

    internal struct MOUSE_EVENT_RECORD
    {
        internal COORD dwMousePosition;
        internal uint dwButtonState;
        internal uint dwControlKeyState;
        internal uint dwEventFlags;
    }

    internal struct WINDOW_BUFFER_SIZE_RECORD
    {
        internal COORD dwSize;
    }

    internal struct MENU_EVENT_RECORD
    {
        internal uint dwCommandId;
    }

    internal struct FOCUS_EVENT_RECORD
    {
        internal bool bSetFocus;
    }

    // The EventUnion struct is also treated as a union in the unmanaged world.
    // We therefore use the StructLayoutAttribute and the FieldOffsetAttribute.
    [StructLayout(LayoutKind.Explicit)]
    internal struct EventUnion
    {
        [FieldOffset(0)]
        internal KEY_EVENT_RECORD KeyEvent;
        [FieldOffset(0)]
        internal MOUSE_EVENT_RECORD MouseEvent;
        [FieldOffset(0)]
        internal WINDOW_BUFFER_SIZE_RECORD WindowBufferSizeEvent;
        [FieldOffset(0)]
        internal MENU_EVENT_RECORD MenuEvent;
        [FieldOffset(0)]
        internal FOCUS_EVENT_RECORD FocusEvent;
    }

    // The INPUT_RECORD structure is used within our application 
    // to capture console input data.
    internal struct INPUT_RECORD
    {
        internal ushort EventType;
        internal EventUnion Event;
    }

    /// <summary>
    /// Summary description for ConsolePasswordInput.
    /// </summary>
    class ConsolePasswordInput
    {
        // This class requires alot of imported functions from Kernel32.dll.

        // ReadConsoleInput() is used to read data from a console input buffer and then remove it from the buffer.
        // We will be relying heavily on this function.
        [DllImport("Kernel32.DLL", EntryPoint = "ReadConsoleInputW", CallingConvention = CallingConvention.StdCall)]
        static extern bool ReadConsoleInput(IntPtr hConsoleInput, [Out] INPUT_RECORD[]
            lpBuffer, uint nLength, out uint lpNumberOfEventsRead);

        // The GetStdHandle() function retrieves a handle for the standard input, standard output, or standard 
        // error device, depending on its input parameter.
        // Handles returned by GetStdHandle() can be used by applications that need to read from or write 
        // to the console. We will be using the handle returned by GetStdHandle() to call the various
        // Console APIs.
        // Note that although handles are integers by default, we will be using the managed type IntPtr
        // to represent the unmanaged world's HANDLE types. This is the recommended practice as expounded
        // in the documentation.
        [DllImport("Kernel32.DLL", EntryPoint = "GetStdHandle", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        // The GetConsoleMode() function retrieves the current input mode of a console's input buffer 
        // or the current output mode of a console screen buffer. 
        // A console consists of an input buffer and one or more screen buffers. The mode of a console 
        // buffer determines how the console behaves during input or output (I/O) operations. 
        // One set of flag constants is used with input handles, and another set is used with screen buffer 
        // (output) handles. 
        // Setting the output modes of one screen buffer does not affect the output modes of other 
        // screen buffers. 
        // We shall be retrieving the mode of our console during password input in order to temporarily 
        // modify the console mode. Later, after retrieving the required password, we will need to restore 
        // the original console mode.
        [DllImport("Kernel32.DLL", EntryPoint = "GetConsoleMode", CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetConsoleMode(IntPtr hConsoleHandle, ref int Mode);

        // The SetConsoleMode() function sets the input mode of a console's input buffer or the output mode 
        // of a console screen buffer.
        // We will be calling this API before the end of our password processing function to restore the
        // previous console mode.
        [DllImport("Kernel32.DLL", EntryPoint = "SetConsoleMode", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int Mode);

        // GetLastError() is a useful Win32 API to determine the cause of a problem when something went wrong.
        [DllImport("Kernel32.DLL", EntryPoint = "GetLastError", CallingConvention = CallingConvention.StdCall)]
        public static extern uint GetLastError();

        // The WriteConsole() function writes a character string to a console screen buffer beginning 
        // at the current cursor location.
        // We will be using this API to write '*'s to the screen in place of a password character.
        [DllImport("Kernel32.DLL", EntryPoint = "WriteConsoleW", CallingConvention = CallingConvention.StdCall)]
        public static extern bool WriteConsole
        (
            IntPtr hConsoleOutput,           // handle to screen buffer
            string lpBuffer,            // write buffer
            uint nNumberOfCharsToWrite,     // number of characters to write
            ref uint lpNumberOfCharsWritten,  // number of characters written
            IntPtr lpReserved                // reserved
        );

        // Not used in this application but declared here for possible future use.
        [DllImport("Kernel32.DLL", EntryPoint = "FlushConsoleInputBuffer", CallingConvention = CallingConvention.StdCall)]
        public static extern bool FlushConsoleInputBuffer(IntPtr hConsoleInput);

        // Not used in this application but declared here for possible future use.
        [DllImport("Kernel32.DLL", EntryPoint = "WriteConsoleOutputCharacterW", CallingConvention = CallingConvention.StdCall)]
        public static extern bool WriteConsoleOutputCharacter
        (
            IntPtr hConsoleOutput,          // handle to screen buffer
            string lpCharacter,            // characters 
            uint nLength,                  // number of characters to write
            COORD dwWriteCoord,             // first cell coordinates
            ref uint lpNumberOfCharsWritten  // number of cells written
        );

        // Declare a delegate to encapsulate a console event handler function.
        // All event handler functions must return a boolean value indicating whether
        // the password processing function should continue to read in another console
        // input record (via ReadConsoleInput() API). 
        // Returning a true indicates continue.
        // Returning a false indicates don't continue.
        internal delegate bool ConsoleInputEvent(INPUT_RECORD input_record, ref string strBuildup);
        // Std console input and output handles.
        protected IntPtr hStdin = (IntPtr)0;
        protected IntPtr hStdout = (IntPtr)0;
        // Used to set and reset console modes.
        protected int dwSaveOldMode = 0;
        protected int dwMode = 0;
        // Counter used to detect how many characters have been typed in.
        protected int iCounter = 0;
        // Hashtable to store console input event handler functions.
        protected Hashtable htCodeLookup;
        // Used to indicate the maximum number of characters for a password. 20 is the default.
        protected int iMaxNumberOfCharacters;

        // Event handler to handle a keyboard event. 
        // We use this function to accumulate characters typed into the console and build
        // up the password this way.
        // All event handler functions must return a boolean value indicating whether
        // the password processing function should continue to read in another console
        // input record (via ReadConsoleInput() API). 
        // Returning a true indicates continue.
        // Returning a false indicates don't continue.
        private bool KeyEventProc(INPUT_RECORD input_record, ref string strBuildup)
        {
            // From the INPUT_RECORD, extract the KEY_EVENT_RECORD structure.
            KEY_EVENT_RECORD ker = input_record.Event.KeyEvent;

            // We process only during the keydown event.
            if (ker.bKeyDown != 0)
            {
                IntPtr intptr = new IntPtr(0);  // This is to simulate a NULL handle value.
                char ch = (char)(ker.uchar.UnicodeChar);  // Get the current character pressed.
                uint dwNumberOfCharsWritten = 0;
                string strOutput = "*";  // The character string that will be displayed on the console screen.

                // If we have received a Carriage Return character, we exit.
                if (ch == (char)'\r')
                {
                    return false;
                }
                else
                {
                    if (ch > 0)  // The typed in key must represent a character and must not be a control ley (e.g. SHIFT, ALT, CTRL, etc)
                    {
                        // A regular (non Carriage-Return character) is typed in...

                        // We first display a '*' on the screen...
                        WriteConsole
                            (
                            hStdout,           // handle to screen buffer
                            strOutput,            // write buffer
                            1,     // number of characters to write
                            ref dwNumberOfCharsWritten,  // number of characters written
                            intptr                // reserved
                            );

                        // We build up our password string...
                        string strConcat = new string(ch, 1);

                        // by appending each typed in character at the end of strBuildup.
                        strBuildup += strConcat;

                        if (++iCounter < MaxNumberOfCharacters)
                        {
                            // Adding 1 to iCounter still makes iCounter less than MaxNumberOfCharacters.
                            // This means that the total number of characters collected so far (this is 
                            // equal to iCounter, by the way) is less than MaxNumberOfCharacters.
                            // We can carry on.
                            return true;
                        }
                        else
                        {
                            // If, by adding 1 to iCounter makes iCounter greater than MaxNumberOfCharacters,
                            // it means that we have already collected MaxNumberOfCharacters number of characters
                            // inside strBuildup. We must exit now.
                            return false;
                        }
                    }
                }
            }

            // The keydown state is false, we allow further characters to be typed in...
            return true;
        }

        // All event handler functions must return a boolean value indicating whether
        // the password processing function should continue to read in another console
        // input record (via ReadConsoleInput() API). 
        // Returning a true indicates continue.
        // Returning a false indicates don't continue.
        private bool MouseEventProc(INPUT_RECORD input_record, ref string strBuildup)
        {
            // Since our Mouse Event Handler does not intend to do anything, 
            // we simply return a true to indicate to the password processing
            // function to readin another console input record.
            return true;
        }

        // All event handler functions must return a boolean value indicating whether
        // the password processing function should continue to read in another console
        // input record (via ReadConsoleInput() API). 
        // Returning a true indicates continue.
        // Returning a false indicates don't continue.
        private bool WindowBufferSizeEventProc(INPUT_RECORD input_record, ref string strBuildup)
        {
            // Since our Window Buffer Size Event Handler does not intend to do anything, 
            // we simply return a true to indicate to the password processing
            // function to readin another console input record.
            return true;
        }

        // All event handler functions must return a boolean value indicating whether
        // the password processing function should continue to read in another console
        // input record (via ReadConsoleInput() API). 
        // Returning a true indicates continue.
        // Returning a false indicates don't continue.
        private bool MenuEventProc(INPUT_RECORD input_record, ref string strBuildup)
        {
            // Since our Menu Event Handler does not intend to do anything, 
            // we simply return a true to indicate to the password processing
            // function to readin another console input record.
            return true;
        }

        // All event handler functions must return a boolean value indicating whether
        // the password processing function should continue to read in another console
        // input record (via ReadConsoleInput() API). 
        // Returning a true indicates continue.
        // Returning a false indicates don't continue.
        private bool FocusEventProc(INPUT_RECORD input_record, ref string strBuildup)
        {
            // Since our Focus Event Handler does not intend to do anything, 
            // we simply return a true to indicate to the password processing
            // function to readin another console input record.
            return true;
        }

        // Public constructor.
        // Here, we prepare our hashtable of console input event handler functions.
        public ConsolePasswordInput()
        {
            htCodeLookup = new Hashtable();
            // Note well that we must cast Constant.* event numbers to ushort's.
            // This is because Constants.*_EVENT have been declared as of type int.
            // We could have, of course, declare Constants.*_EVENT to be of type ushort
            // but I deliberately declared them as ints to show the importance of 
            // types in C#.
            htCodeLookup.Add((object)((ushort)(Constants.KEY_EVENT)), new ConsoleInputEvent(KeyEventProc));
            htCodeLookup.Add((object)((ushort)(Constants.MOUSE_EVENT)), new ConsoleInputEvent(MouseEventProc));
            htCodeLookup.Add((object)((ushort)(Constants.WINDOW_BUFFER_SIZE_EVENT)), new ConsoleInputEvent(WindowBufferSizeEventProc));
            htCodeLookup.Add((object)((ushort)(Constants.MENU_EVENT)), new ConsoleInputEvent(MenuEventProc));
            htCodeLookup.Add((object)((ushort)(Constants.FOCUS_EVENT)), new ConsoleInputEvent(FocusEventProc));
        }

        // Public property.
        public int MaxNumberOfCharacters
        {
            get
            {
                return iMaxNumberOfCharacters;
            }
            set
            {
                iMaxNumberOfCharacters = value;
            }
        }

        // The main function of this class.
        public void PasswordInput(ref string refPasswordToBuild, int iMaxNumberOfCharactersSet)
        {
            INPUT_RECORD[] irInBuf = new INPUT_RECORD[128]; // Define an array of 128 INPUT_RECORD structs.
            uint cNumRead = 0;
            bool bContinueLoop = true;  // Used to indicate whether to continue our ReadConsoleInput() loop.

            // Reset character counter.
            iCounter = 0;

            // Initialize hStdin.
            if (hStdin == (IntPtr)0)
            {
                hStdin = GetStdHandle(Constants.STD_INPUT_HANDLE);
                if (hStdin == Constants.INVALID_HANDLE_VALUE)
                {
                    return;
                }
            }

            // Initialize hStdout.
            if (hStdout == (IntPtr)0)
            {
                hStdout = GetStdHandle(Constants.STD_OUTPUT_HANDLE);
                if (hStdout == Constants.INVALID_HANDLE_VALUE)
                {
                    return;
                }
            }

            // Retrieve the current console mode.
            if (GetConsoleMode(hStdin, ref dwSaveOldMode) == false)
            {
                return;
            }

            // Set the current console mode to enable window input and mouse input.
            // This is not necessary for our password processing application. 
            // This is set only for demonstration purposes.
            //
            // By setting ENABLE_WINDOW_INPUT into the console mode, user interactions 
            // that change the size of the console screen buffer are reported in the 
            // console's input buffer. Information about this event can be read from 
            // the input buffer by our application using the ReadConsoleInput function.
            //
            // By setting ENABLE_MOUSE_INPUT into the console mode, if the mouse pointer 
            // is within the borders of the console window and the window has the 
            // keyboard focus, mouse events generated by mouse movement and button presses 
            // are placed in the input buffer. Information about this event can be read from 
            // the input buffer by our application using the ReadConsoleInput function.
            dwMode = Constants.ENABLE_WINDOW_INPUT | Constants.ENABLE_MOUSE_INPUT;
            if (SetConsoleMode(hStdin, dwMode) == false)
            {
                return;
            }

            // To safeguard against invalid values, we stipulate that only if iMaxNumberOfCharactersSet
            // is greater than zero do we set MaxNumberOfCharacters equal to it.
            // Otherwise, MaxNumberOfCharacters is set to 20 by default.
            // An alternative to setting MaxNumberOfCharacters to a default value is to throw an exception.
            if (iMaxNumberOfCharactersSet > 0)
            {
                MaxNumberOfCharacters = iMaxNumberOfCharactersSet;
            }
            else
            {
                // We could throw an exception here if we want to.
                MaxNumberOfCharacters = 20;
            }

            // Main loop to collect characters typed into the console.
            while (bContinueLoop == true)
            {
                if
                (
                    ReadConsoleInput
                    (
                        hStdin,      // input buffer handle 
                        irInBuf,     // buffer to read into 
                        128,         // size of read buffer 
                        out cNumRead // number of records read 
                    ) == true
                )
                {
                    // Dispatch the events to the appropriate handler. 
                    for (uint i = 0; i < cNumRead; i++)
                    {
                        // Lookup the hashtable for the appropriate handler function... courtesy of Derek Kiong !
                        ConsoleInputEvent cie_handler = (ConsoleInputEvent)htCodeLookup[(object)(irInBuf[i].EventType)];

                        // Note well that htCodeLookup may not have the handler for the current event, 
                        // so check first for a null value in cie_handler.
                        if (cie_handler != null)
                        {
                            // Invoke the handler.
                            bContinueLoop = cie_handler(irInBuf[i], ref refPasswordToBuild);
                        }
                    }
                }
            }

            // Restore the previous mode before we exit.
            SetConsoleMode(hStdin, dwSaveOldMode);

            return;
        }
    }
}
