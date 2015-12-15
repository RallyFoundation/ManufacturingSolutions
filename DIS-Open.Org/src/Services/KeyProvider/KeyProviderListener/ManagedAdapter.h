//------------------------------------------------------------------------------
// Copyright (c) 2010 Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Module Name:
//              ManagedAdapter.h
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

#pragma once

namespace ManagedAdapter {
	
	class ManagedCoupler;

	class ManagedBridge
	{
	private:
		ManagedCoupler* coupler;

	public:
		ManagedBridge();		
		~ManagedBridge();

        int ManagedBridge::GetKey(LPCWSTR parameters, LPWSTR productKeyInfo);
		int ManagedBridge::UpdateKey(LPCWSTR parameters, LPCWSTR productKeyInfo);
		int ManagedBridge::Ping(LPWSTR productKeyInfo);
	};
};