//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
#pragma once

#include <Winsock2.h>   // Winsock2.h includes Windows.h
#include <Ws2bth.h>     // Include Ws2bth.h before BluetoothAPIs.h
#include <initguid.h>
#include <windows.h>
#include <stdio.h>
#include <winioctl.h>
#include <devguid.h>
#include <poclass.h>
#include <ntddvdeo.h>
#include <wlanapi.h>
#include <objbase.h>
#include <wtypes.h>
#include <stdlib.h>
#include <string>

#include <propsys.h>
#include <propkey.h>

#include <BluetoothAPIs.h>
#include <iostream>
#include <bthdef.h>
#include <BluetoothAPIs.h>
#include <tchar.h>

#include <setupapi.h>

#include <mfapi.h>
#include <mfcaptureengine.h>
#include <shlwapi.h>
#include <cfgmgr32.h>

#pragma comment(lib, "irprops.lib")


#include <powersetting.h>
#define UNKNOWN_STATE       2
#define UNKNOWN_PERCENTAGE  101
#define MAX_WAIT_TIME       1000 // 1 sec
#define WiFi_Conn_FAIL      555


#define MAX_STRING_LENGTH 1024
using namespace std;
using namespace System;
using namespace System::Collections;
using namespace System::Diagnostics;

// Settings for FsTestBrightness()
#define MAX_BRIGHT_PERIOD 500       // 0.5 sec
#define MAX_CHANGE_LEVEL 5
// for Code analysis
using namespace System;
using namespace System::Reflection;
using namespace System::Runtime::InteropServices;
[assembly:CLSCompliant(true)];
[assembly:System::Runtime::InteropServices::ComVisible(false)];
[assembly:AssemblyVersionAttribute("4.3.2.1")];
namespace DesignLibrary {}

namespace DllComponent {
	// for c++
	struct WIFIINFO
	{
		WCHAR wsSSID[64];
		int iSignaldegree;
		int iIfSecurity;
	}WIFIINFO_default = { 0, 0, 0 };
    /// <summary>
    /// Summary for CoreComponent
    /// </summary>
	public ref class CoreComponent sealed
    {
    public:
        CoreComponent() {}

        HRESULT TestBrightness();
		String^ TestWiFi(String^ TargetSSID, int iIfConnection);
		String^ TestBT(int iSec);
		String^ GetCameraDevice();
		int DisableTouch(IntPtr hWnd);
	private:
		int WiFiQuery(WIFIINFO *pWIFIINFO, int infoSize, int *outInfoLen, WCHAR *TargetSSID, int iIfConnection);// for c++

    protected:
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        ~CoreComponent() {}

    private:



    };
}
