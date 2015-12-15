// KeyProvider.Listener.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

#include <stdlib.h>
#include <vcclr.h>
#include <string>

#include "UnmanagedRpc.h"
#include "KeyProvider.Listener.h"

#define ERROR_BUFFER_SIZE 500

using namespace System;
using namespace System::Runtime::InteropServices;

namespace KeyProvider {

    Listener::Listener()
	{
		pInner = new UnmanagedRpc();
	}

    long Listener::RegisterRpc(
          String^ protocolSequence
        , String^ endpoint
        , String^ serviceClass
        , String^ networkAddress
        , String^ errorMessage
        )
    {
		wchar_t *pszProtocolSequence = static_cast<wchar_t*>(Marshal::StringToHGlobalUni(protocolSequence).ToPointer());
		wchar_t *pszEndpoint = static_cast<wchar_t*>(Marshal::StringToHGlobalUni(endpoint).ToPointer());
		wchar_t *pszServiceClass = static_cast<wchar_t*>(Marshal::StringToHGlobalUni(serviceClass).ToPointer());
		wchar_t *pszNetworkAddress = static_cast<wchar_t*>(Marshal::StringToHGlobalUni(networkAddress).ToPointer());

        wchar_t pszErrorMessage[ERROR_BUFFER_SIZE];

        // call the unmanaged code...
        long result = pInner->RegisterRpc(
                          pszProtocolSequence
                        , pszEndpoint
                        , pszServiceClass
                        , pszNetworkAddress
                        , pszErrorMessage
                        , ERROR_BUFFER_SIZE
                        );

		// convert the returned error message into a managed string
		errorMessage = gcnew String(pszErrorMessage);

		// release resources!!
		Marshal::FreeHGlobal(static_cast<System::IntPtr>(pszProtocolSequence));
		Marshal::FreeHGlobal(static_cast<System::IntPtr>(pszEndpoint));
		Marshal::FreeHGlobal(static_cast<System::IntPtr>(pszServiceClass));
		Marshal::FreeHGlobal(static_cast<System::IntPtr>(pszNetworkAddress));

		return result;
    }

    long Listener::UnregisterRpc(
            String^ errorMessage
        )
    {
        wchar_t pszErrorMessage[ERROR_BUFFER_SIZE];

        long result = pInner->UnregisterRpc(
                          pszErrorMessage
                        , ERROR_BUFFER_SIZE
                        );

        errorMessage = gcnew String(pszErrorMessage);

        return result;
    }
}