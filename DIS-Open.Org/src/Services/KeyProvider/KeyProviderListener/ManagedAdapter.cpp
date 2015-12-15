//------------------------------------------------------------------------------
// Copyright (c) 2010 Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Module Name:
//              ManagedAdapter.cpp
//
// Abstract:
// 	            CLI interface to managed code for CPP.
//
// Author (adaptation):
//              Don Schuy (v-dschuy)   JUL 29, 2010
//
// Environment:
//		        WinPE, Windows XP or better
//
// Revision History:
//		        JUL 29, 2010, v-dschuy
//			    Adaptation of Scott Burkhalter's CLI code for Key Provider.
//------------------------------------------------------------------------------ 

#include "stdafx.h"

#include "kplistener.h"
#include "ManagedAdapter.h"
#pragma warning(disable : 4512)

#include <msclr/auto_gcroot.h>
using msclr::auto_gcroot;

#using <System.dll>
using namespace System;
using namespace System::Runtime::InteropServices;

using namespace  DIS::Business::Proxy;

namespace ManagedAdapter {

	///
	// Coupler needed to hide clr stuff from our main .h file so client app 
	// code can compile cleanly; Provides adapter to the CSharp component
	// that we wish to interact with. -SB
	///
	public class ManagedCoupler
	{
	private:
		auto_gcroot<KeyStoreProviderProxy^> adapter;

	public:
		ManagedCoupler() : adapter(gcnew KeyStoreProviderProxy()) {}

		int GetKey(LPCWSTR parameters, LPWSTR ProductKeyInfo)
		{
			HRESULT hr = S_OK;

			try
			{
				String^ sResult = gcnew String("");
				hr = adapter->GetKey(gcnew String(parameters), sResult);
			
				IntPtr p = Marshal::StringToHGlobalAuto(sResult); 
				LPWSTR pResult = static_cast<LPWSTR>(p.ToPointer());
				int resultLength = wcsnlen_s(pResult, DM_SIZE);
				wcsncpy_s(ProductKeyInfo, DM_SIZE, pResult, resultLength);
				Marshal::FreeHGlobal(p); 
			}
			catch(char * ex)
			{
				fprintf_s( stderr, "Exception: %s\n", ex );
			}

			return hr;
		}

		int UpdateKey(LPCWSTR parameters, LPCWSTR ProductKeyInfo)
		{
			HRESULT hr = S_OK;

			try
			{
				hr = adapter->UpdateKey(gcnew String(parameters), gcnew String(ProductKeyInfo));
			}
			catch(char * ex)
			{
				fprintf_s( stderr, "Exception: %s\n", ex );
			}

			return hr;
		}

		int Ping(LPWSTR ProductKeyInfo)
		{
			HRESULT hr = S_OK;

			try
			{
				String^ sResult = gcnew String("");
				hr = adapter->Ping(sResult);
			
				IntPtr p = Marshal::StringToHGlobalAuto(sResult); 
				LPWSTR pResult = static_cast<LPWSTR>(p.ToPointer());
				int resultLength = wcsnlen_s(pResult, DM_SIZE);
				wcsncpy_s(ProductKeyInfo, DM_SIZE, pResult, resultLength);
				Marshal::FreeHGlobal(p); 
			}
			catch(char * ex)
			{
				fprintf_s( stderr, "Exception: %s\n", ex );
			}

			return hr;
		}
	};
	
	///
	// Bridges Unmanaged to managed code - uses a coupler to thunk the gap
	///
	ManagedBridge::ManagedBridge() 
	{
		coupler = new ManagedCoupler();
	}
		
	ManagedBridge::~ManagedBridge() 
	{
		delete coupler;
	}

	int ManagedBridge::GetKey(LPCWSTR parameters, LPWSTR ProductKeyInfo)
	{
		return coupler->GetKey(parameters, ProductKeyInfo);
	}

	int ManagedBridge::UpdateKey(LPCWSTR parameters, LPCWSTR ProductKeyInfo)
	{
		return coupler->UpdateKey(parameters, ProductKeyInfo);
	}

	int ManagedBridge::Ping(LPWSTR ProductKeyInfo)
	{
		return coupler->Ping(ProductKeyInfo);
	}
}