//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

#include "CoreComponent.h"

using namespace DllComponent;

#define MAX_SIZE 2048
/// <summary>
/// Test screen brightness.
/// </summary>
/// <returns>Return execution result.</returns>
HRESULT  CoreComponent::TestBrightness()
{
	//HRESULT  status = S_OK;
	DWORD status = ERROR_SUCCESS;
	LPGUID pScheme;
	static ULONG ulType = REG_DWORD;
	DWORD autoAcBrightness = UNKNOWN_STATE, autoDcBrightness = UNKNOWN_STATE;
	DWORD curAcBrightness = UNKNOWN_PERCENTAGE, curDcBrightness = UNKNOWN_PERCENTAGE;
	DWORD dwSize = sizeof(DWORD);

	status = PowerGetActiveScheme(NULL, &pScheme);
	if (status != ERROR_SUCCESS)
	{
		//wprintf(L"PowerGetActiveScheme() Error = %u\n", status);
		goto Exit;
	}

	// Get the current auto brightness state and brightness percentage
	status = PowerReadACValue(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_VIDEO_ADAPTIVE_DISPLAY_BRIGHTNESS, &ulType, (LPBYTE)(&autoAcBrightness), &dwSize);
	if (status != ERROR_SUCCESS)
	{
		//wprintf(L"PowerReadACValue() Error = %u\n", status);
		goto Exit;
	}
	status = PowerReadACValue(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_DEVICE_POWER_POLICY_VIDEO_BRIGHTNESS, &ulType, (LPBYTE)(&curAcBrightness), &dwSize);
	if (status != ERROR_SUCCESS)
	{
		//wprintf(L"PowerReadACValue() Error = %u\n", status);
		goto Exit;
	}

	status = PowerReadDCValue(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_VIDEO_ADAPTIVE_DISPLAY_BRIGHTNESS, &ulType, (LPBYTE)(&autoDcBrightness), &dwSize);
	if (status != ERROR_SUCCESS)
	{
		//wprintf(L"PowerReadDCValue() Error = %u\n", status);
		goto Exit;
	}

	status = PowerReadDCValue(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_DEVICE_POWER_POLICY_VIDEO_BRIGHTNESS, &ulType, (LPBYTE)(&curDcBrightness), &dwSize);
	if (status != ERROR_SUCCESS)
	{
		//wprintf(L"PowerReadDCValue() Error = %u\n", status);
		goto Exit;
	}

	/*wprintf(L"AutoAC = %u, AutoDC = %u, curACBrightness = %u, curDCBrightness = %u\n",
		autoAcBrightness, autoDcBrightness, curAcBrightness, curDcBrightness);*/

	// Disable adjust screen brightness automatically
	status = PowerWriteACValueIndex(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_VIDEO_ADAPTIVE_DISPLAY_BRIGHTNESS, 0);
	if (status != ERROR_SUCCESS)
	{
		//wprintf(L"PowerWriteACValueIndex() Error = %u\n", status);
		goto Exit;
	}
	status = PowerWriteDCValueIndex(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_VIDEO_ADAPTIVE_DISPLAY_BRIGHTNESS, 0);
	if (status != ERROR_SUCCESS)
	{
		//wprintf(L"PowerWriteDCValueIndex() Error = %u\n", status);
		goto Exit;
	}

	status = PowerSetActiveScheme(NULL, pScheme);
	if (status != ERROR_SUCCESS)
	{
		//wprintf(L"PowerWriteDCValueIndex() Error = %u\n", status);
		goto Exit;
	}

	for (DWORD i = 0; i <= 100; i += 20)
	{
		status = PowerWriteACValueIndex(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_DEVICE_POWER_POLICY_VIDEO_BRIGHTNESS, i);
		if (status != ERROR_SUCCESS)
		{
			//wprintf(L"PowerWriteACValueIndex() Error = %u\n", status);
			goto Exit;
		}
		status = PowerWriteDCValueIndex(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_DEVICE_POWER_POLICY_VIDEO_BRIGHTNESS, i);
		if (status != ERROR_SUCCESS)
		{
			//wprintf(L"PowerWriteDCValueIndex() Error = %u\n", status);
			goto Exit;
		}

		//wprintf(L"Brightness = %u\n", i);
		status = PowerSetActiveScheme(NULL, pScheme);
		if (status != ERROR_SUCCESS) goto Exit;

		Sleep(MAX_WAIT_TIME);
	}

Exit:

	// Restore back to the current auto brightness state and brightness percentage
	if (autoAcBrightness != UNKNOWN_STATE)
	{
		status = PowerWriteACValueIndex(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_VIDEO_ADAPTIVE_DISPLAY_BRIGHTNESS, autoAcBrightness);
		if (status != ERROR_SUCCESS)
		{
			//wprintf(L"PowerWriteACValueIndex() Error = %u\n", status);
			goto Exit;
		}
	}

	if (autoDcBrightness != UNKNOWN_STATE)
	{
		status = PowerWriteDCValueIndex(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_VIDEO_ADAPTIVE_DISPLAY_BRIGHTNESS, autoDcBrightness);
		if (status != ERROR_SUCCESS)
		{
			//wprintf(L"PowerWriteDCValueIndex() Error = %u\n", status);
			goto Exit;
		}
	}

	if (curAcBrightness != UNKNOWN_PERCENTAGE)
	{
		status = PowerWriteACValueIndex(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_DEVICE_POWER_POLICY_VIDEO_BRIGHTNESS, curAcBrightness);
		if (status != ERROR_SUCCESS)
		{
			//wprintf(L"PowerWriteACValueIndex() Error = %u\n", status);
			goto Exit;
		}
	}

	if (curDcBrightness != UNKNOWN_PERCENTAGE)
	{
		status = PowerWriteDCValueIndex(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_DEVICE_POWER_POLICY_VIDEO_BRIGHTNESS, curDcBrightness);
		if (status != ERROR_SUCCESS)
		{
			//wprintf(L"PowerWriteACValueIndex() Error = %u\n", status);
			goto Exit;
		}
	}

	status = PowerSetActiveScheme(NULL, pScheme);
	if (status != ERROR_SUCCESS)
	{
		//wprintf(L"PowerWriteDCValueIndex() Error = %u\n", status);
		goto Exit;
	}

	// Get the current auto brightness state and brightness percentage
	status = PowerReadACValue(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_VIDEO_ADAPTIVE_DISPLAY_BRIGHTNESS, &ulType, (LPBYTE)(&autoAcBrightness), &dwSize);
	if (status != ERROR_SUCCESS)
	{
		//wprintf(L"PowerReadACValue() Error = %u\n", status);
		goto Exit;
	}

	status = PowerReadDCValue(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_VIDEO_ADAPTIVE_DISPLAY_BRIGHTNESS, &ulType, (LPBYTE)(&autoDcBrightness), &dwSize);
	if (status != ERROR_SUCCESS)
	{
		//wprintf(L"PowerReadDCValue() Error = %u\n", status);
		goto Exit;
	}

	status = PowerReadACValue(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_DEVICE_POWER_POLICY_VIDEO_BRIGHTNESS, &ulType, (LPBYTE)(&curAcBrightness), &dwSize);
	if (status != ERROR_SUCCESS)
	{
		//wprintf(L"PowerReadACValue() Error = %u\n", status);
		goto Exit;
	}

	status = PowerReadDCValue(NULL, pScheme, &GUID_VIDEO_SUBGROUP, &GUID_DEVICE_POWER_POLICY_VIDEO_BRIGHTNESS, &ulType, (LPBYTE)(&curDcBrightness), &dwSize);
	if (status != ERROR_SUCCESS)
	{
		//wprintf(L"PowerReadDCValue() Error = %u\n", status);
		goto Exit;
	}

	/*wprintf(L"AutoAC = %u, AutoDC = %u, curACBrightness = %u, curDCBrightness = %u\n",
		autoAcBrightness, autoDcBrightness, curAcBrightness, curDcBrightness);*/

	return status;
}

/// <summary>
/// Test WiFi for searching nearby AP.
/// </summary>
/// <returns>Return searched AP list with String.</returns>
String^ CoreComponent::TestWiFi(String^ TargetSSID, int iIfConnection)
{
	int dwRetVal = 0;
	WIFIINFO strWIFIINFO[32];
	int iOut = 0;

	wchar_t str[MAX_SIZE] = L"";
	wchar_t strTemp[MAX_SIZE] = L"";
	

	const wchar_t* wchars = (const wchar_t*)(Marshal::StringToHGlobalUni(TargetSSID)).ToPointer();
	dwRetVal = WiFiQuery(strWIFIINFO, 32, &iOut, (WCHAR*)wchars, iIfConnection);
	for (int i = 0; i < iOut; i++)
	{
		if (wcslen(strWIFIINFO[i].wsSSID) == 0)
		{
			swprintf_s(strTemp, MAX_STRING_LENGTH, L"NULL ,%d\n", strWIFIINFO[i].iSignaldegree);  // strWIFIINFO[i].iIfSecurity for security
		}
		else
		{
			swprintf_s(strTemp, MAX_STRING_LENGTH, L"%s ,%d\n", strWIFIINFO[i].wsSSID, strWIFIINFO[i].iSignaldegree); // strWIFIINFO[i].iIfSecurity for security
		}
		wcscat(str, strTemp);
	}

	if (dwRetVal == WiFi_Conn_FAIL)
		wcscat(str, L"WiFi_Connect_Fail ,0\n");

		

	return gcnew String(str);
}
/// <summary>
/// Test WiFi for searching nearby AP.
/// </summary>
/// <param name="pWIFIINFO">the struct of SSID,Signaldegree and Security status.< / param>
/// <param name="infoSize">the maximum size of pWIFIINFO.< / param>
/// <param name="outInfoLen">the number of pWIFIINFO.< / param>
/// <returns>Return execution result.</returns>
int CoreComponent::WiFiQuery(WIFIINFO *pWIFIINFO, int infoSize, int *outInfoLen, WCHAR *TargetSSID, int iIfConnection)
{
	// variables used for WlanEnumInterfaces
	PWLAN_INTERFACE_INFO_LIST pIfList = NULL;
	PWLAN_INTERFACE_INFO pIfInfo = NULL;
	PWLAN_AVAILABLE_NETWORK_LIST pBssList = NULL;
	PWLAN_AVAILABLE_NETWORK pBssEntry = NULL;

	HRESULT hr = S_OK;
	HANDLE hClient = NULL;
	DWORD dwMaxClient = WLAN_API_VERSION_2_0;
	DWORD dwCurVersion = 0;
	DWORD dwResult = 0;
	DWORD dwRetVal = 0;
	int iRet = 0;
	unsigned int i, j, k;
	int m = 0;

	int selectedIndex = -1;
	PWLAN_AVAILABLE_NETWORK selectedBssEntry = NULL;
	PWLAN_AVAILABLE_NETWORK curConnectedBssEntry = NULL;
	WCHAR selectedSSID[64] = { 0 };
	WCHAR curConnectedSSID[64] = { 0 };
	LPWSTR pProfileXml = NULL;

	WCHAR ssidName[WLAN_MAX_NAME_LENGTH + 1];
	WCHAR str[MAX_STRING_LENGTH] = { 0 };



	dwResult = WlanOpenHandle(dwMaxClient, NULL, &dwCurVersion, &hClient);
	if (dwResult != ERROR_SUCCESS) {
		hr = HRESULT_FROM_WIN32(dwResult);
		dwRetVal = 1;
		goto Exit;
	}

	dwResult = WlanEnumInterfaces(hClient, NULL, &pIfList);
	if (dwResult != ERROR_SUCCESS) {
		hr = HRESULT_FROM_WIN32(dwResult);
		dwRetVal = 1;
		goto Exit;
	}

	for (i = 0; i < (int)pIfList->dwNumberOfItems; i++)
	{
		pIfInfo = (WLAN_INTERFACE_INFO *)&pIfList->InterfaceInfo[i];

		dwResult = WlanGetAvailableNetworkList(hClient,
			&pIfInfo->InterfaceGuid,
			WLAN_AVAILABLE_NETWORK_INCLUDE_ALL_MANUAL_HIDDEN_PROFILES,
			NULL,
			&pBssList);
		if (dwResult != ERROR_SUCCESS) {
			hr = HRESULT_FROM_WIN32(dwResult);
			dwRetVal = 1;
			goto Exit;
		}

		for (j = 0; j < pBssList->dwNumberOfItems; j++)
		{
			pBssEntry = (WLAN_AVAILABLE_NETWORK *)&pBssList->Network[j];

			// Remove duplicated network name (the SSID with profile)
			if (pBssEntry->dwFlags == 0x2) continue;

			// Skip null string or hidden SSID
			if (pBssEntry->dot11Ssid.uSSIDLength != 0)
			{
				ssidName[0] = L'\0';
				for (k = 0; k < pBssEntry->dot11Ssid.uSSIDLength; k++)
				{
					swprintf_s(ssidName, WLAN_MAX_NAME_LENGTH, L"%s%c", ssidName, pBssEntry->dot11Ssid.ucSSID[k]);
				}
				ssidName[WLAN_MAX_NAME_LENGTH] = L'\0';

				swprintf_s(pWIFIINFO[m].wsSSID, 64, L"%s", ssidName);
				pWIFIINFO[m].iSignaldegree = (int)pBssEntry->wlanSignalQuality;
				pWIFIINFO[m].iIfSecurity = (int)pBssEntry->bSecurityEnabled;
				if (m >= infoSize) goto Exit;
				m++;

				if (pBssEntry->dwFlags) {
					if (pBssEntry->dwFlags & WLAN_AVAILABLE_NETWORK_CONNECTED)
					{
						//Currently connected
						curConnectedBssEntry = pBssEntry;
						swprintf(&curConnectedSSID[0], 64, L"%s", &ssidName[0]);
					}
				}
				if (TargetSSID != NULL)
				{
					if (selectedIndex == -1 && wcscmp(TargetSSID, ssidName) == 0)
					{
						selectedIndex = j;
						selectedBssEntry = pBssEntry;
						swprintf(&selectedSSID[0], 64, L"%s", &ssidName[0]);
					}
				}

			}

			/*swprintf_s(str, MAX_STRING_LENGTH, L"%s%s \t\t%u \t\t%d \n",
				str, ssidName, pBssEntry->bSecurityEnabled, pBssEntry->wlanSignalQuality);*/

		}// end of WlanGetAvailableNetworkList
		if (iIfConnection == 1)
		{
			if (TargetSSID != NULL)
			{
				if (selectedIndex < 0) // SSID not found!
				{
					dwRetVal = WiFi_Conn_FAIL;
					goto Exit;
				}
				else
				{
					// disconnect current connection first
					if (curConnectedBssEntry != NULL)
					{
						dwResult = WlanDisconnect(hClient,
							&pIfInfo->InterfaceGuid,
							0);

						if (dwResult != ERROR_SUCCESS) { //WlanDisconnect failed with error
							dwRetVal = WiFi_Conn_FAIL;
							goto Exit;
							// You can use FormatMessage to find out why the function failed
						}
					}
					// go connect
					wprintf(L"Go connect (%s)...\n", selectedSSID);

					WLAN_CONNECTION_PARAMETERS connectionData;
					connectionData.wlanConnectionMode = wlan_connection_mode_discovery_unsecure;
					connectionData.strProfile = NULL;
					connectionData.pDot11Ssid = &(selectedBssEntry->dot11Ssid);
					connectionData.pDesiredBssidList = NULL;
					connectionData.dot11BssType = selectedBssEntry->dot11BssType;
					connectionData.dwFlags = 0;

					if (selectedBssEntry->dwFlags & WLAN_AVAILABLE_NETWORK_HAS_PROFILE)
					{
						DWORD dwFlags = 0;
						DWORD dwGrantedAccess = 0;

						dwResult = WlanGetProfile(hClient,
							&pIfInfo->InterfaceGuid,
							selectedBssEntry->strProfileName,
							NULL,
							&pProfileXml,
							&dwFlags,
							&dwGrantedAccess);

						if (dwResult != ERROR_SUCCESS) {
							wprintf(L"WlanGetProfile failed with error: %u\n", dwResult);
							dwRetVal = WiFi_Conn_FAIL;
							goto Exit;
							// You can use FormatMessage to find out why the function failed
						}
						else {

							connectionData.strProfile = pProfileXml;
						}
					}
					dwResult = WlanConnect(hClient,
						&pIfInfo->InterfaceGuid,
						&connectionData,
						0);

					if (dwResult != ERROR_SUCCESS) {
						wprintf(L"WlanConnect (%s) failed with error: 0x%08x\n", selectedSSID, dwResult);
						dwRetVal = WiFi_Conn_FAIL;
						// You can use FormatMessage to find out why the function failed
					}

				}

			}
		}
	}

Exit:
	if (hClient != INVALID_HANDLE_VALUE)
	{
		WlanCloseHandle(hClient, 0);
		hClient = INVALID_HANDLE_VALUE;
	}
	if (pBssList != NULL) {
		WlanFreeMemory(pBssList);
		pBssList = NULL;
	}

	if (pIfList != NULL) {
		WlanFreeMemory(pIfList);
		pIfList = NULL;
	}
	if (pProfileXml != NULL) {
		WlanFreeMemory(pProfileXml);
		pProfileXml = NULL;
	}
	*outInfoLen = m;

	return dwRetVal;
}
/// <summary>
/// Test BT for searching nearby BT devices.
/// </summary>
/// <param name="iSec">the BT scan delay time and it can be configure on the SFTConfig.xml.< / param>
String^ CoreComponent::TestBT(int iSec)
{
	UINT8 nRetry;
	HRESULT  hr = S_OK;
	static int iTimeoutMultiplier = (int)ceil(iSec / 1.28);
	if (iTimeoutMultiplier > 48)
		iTimeoutMultiplier = 5;
	
	BLUETOOTH_DEVICE_SEARCH_PARAMS parameters = { sizeof(BLUETOOTH_DEVICE_SEARCH_PARAMS),
		1, 0, 1, 1, 1, iTimeoutMultiplier, NULL };
	BLUETOOTH_DEVICE_INFO deviceInfo = { 0 };
	HBLUETOOTH_DEVICE_FIND hFind = NULL;
	HRESULT hrInit = E_FAIL;
	WCHAR str[MAX_STRING_LENGTH];
	bool bBTDiscover = false;
	str[0] = L'\0';

	// Grab a handle to the first device
	deviceInfo.dwSize = sizeof(BLUETOOTH_DEVICE_INFO);
	for (nRetry = 0; nRetry < 2 && hFind == NULL; nRetry++)
	{
		hFind = BluetoothFindFirstDevice(&parameters, &deviceInfo);
	}

	if (NULL == hFind)
	{
		//hr = HRESULT_FROM_WIN32(GetLastError());
		goto Exit;
	}

	int i = 1;
	if (deviceInfo.szName[0] == L'\0')
	{
		swprintf_s(str, MAX_STRING_LENGTH, L"%2d. %s\n", i++, L"(Unknown)");
		wprintf(L"\n(Unknown)");
	}

	else
	{
		swprintf_s(str, MAX_STRING_LENGTH, L"%2d. %s\n", i++, deviceInfo.szName);
		wprintf(L"\n");
		wprintf(deviceInfo.szName);
		bBTDiscover = true;

	}


	while (BluetoothFindNextDevice(hFind, &deviceInfo))
	{
		if (deviceInfo.szName[0] == L'\0')
		{
			swprintf_s(str, MAX_STRING_LENGTH, L"%s%2d. %s\n", str, i++, L"(Unknown)");
			wprintf(L"\n(Unknown)");
		}
		else
		{
			swprintf_s(str, MAX_STRING_LENGTH, L"%s%2d. %s\n", str, i++, deviceInfo.szName);
			wprintf(L"\n");
			wprintf(deviceInfo.szName);
			bBTDiscover = true;

		}

	}

	DWORD dwError = GetLastError();
	if (dwError != ERROR_NO_MORE_ITEMS && dwError != ERROR_SUCCESS)
	{
		hr = HRESULT_FROM_WIN32(dwError);
		goto Exit;
	}

	//swprintf_s(lpBluetoothList, MAX_STRING_LENGTH, L"%s\0", str);

Exit:

	// Close deviceHandle so nothing is leaked
	if (NULL != hFind)  BluetoothFindDeviceClose(hFind);

	if (hrInit == S_OK)  CoUninitialize();

	return gcnew String(str);

}

/// <summary>
/// Returns friendly name of available camera devices.
/// </summary>
/// <returns>Return execution result.</returns>
String^  CoreComponent::GetCameraDevice()
{
	WCHAR str[MAX_STRING_LENGTH] = { 0 };

	IMFActivate **ppDevices = NULL;
	UINT32      count = 0;

	IMFAttributes *pAttributes = NULL;

	HRESULT hr = MFCreateAttributes(&pAttributes, 1);
	if (FAILED(hr))
	{
		wprintf(L" MFCreateAttributes() Error = 0x%X \n", hr);
		return "";
	}
	
	// Ask for source type = video capture devices
	hr = pAttributes->SetGUID(MF_DEVSOURCE_ATTRIBUTE_SOURCE_TYPE,
		MF_DEVSOURCE_ATTRIBUTE_SOURCE_TYPE_VIDCAP_GUID);
	if (FAILED(hr))
	{
		wprintf(L" SetGUID() Error = 0x%X \n", hr);
		return "";
	}

	// Enumerate devices.
	hr = MFEnumDeviceSources(pAttributes, &ppDevices, &count);
	if (FAILED(hr))
	{
		wprintf(L" MFEnumDeviceSources() Error = 0x%X \n", hr);
		return "";
	}

	String ^outStr = gcnew String("");

	for (DWORD k = 0; k < count; k++)
	{
		wchar_t *szFriendlyName = NULL;
		UINT32 cchName;
		HRESULT hr = S_OK;

		hr = ppDevices[k]->GetAllocatedString(MF_DEVSOURCE_ATTRIBUTE_FRIENDLY_NAME,
			&szFriendlyName, &cchName);
		OutputDebugStringW(szFriendlyName);
		int size = sizeof(szFriendlyName[0]);

		if (FAILED(hr))
		{
			break;
		}

		String ^tmpStr = gcnew String(szFriendlyName);
		if (outStr->Length > 0) {
			outStr = outStr + "," + tmpStr;
		}
		else {
			outStr = outStr + tmpStr;
		}
		
		CoTaskMemFree(szFriendlyName);
	}
	return outStr;
}



HRESULT SetTouchDisableProperty(HWND hwnd, BOOL fDisableTouch)
{
	IPropertyStore* pPropStore;
	HRESULT hrReturnValue = SHGetPropertyStoreForWindow(hwnd, IID_PPV_ARGS(&pPropStore));
	if (SUCCEEDED(hrReturnValue))
	{
		PROPVARIANT var;
		var.vt = VT_BOOL;
		var.boolVal = fDisableTouch ? VARIANT_TRUE : VARIANT_FALSE;
		hrReturnValue = pPropStore->SetValue(PKEY_EdgeGesture_DisableTouchWhenFullscreen, var);
		pPropStore->Release();
	}
	return hrReturnValue;
}

/// <summary>
/// Disable edge UI when in full screen mode
/// </summary>
/// <param name="hWnd">the pointer of a window handle< / param>
int CoreComponent::DisableTouch(IntPtr hWnd) 
{
	HWND nativeHWND = (HWND)hWnd.ToPointer();
	SetTouchDisableProperty(nativeHWND, true);
	return 0;
}

