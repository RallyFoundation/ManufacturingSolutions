// exports.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

// This is an example of an exported function.
extern "C"
{
    __declspec(dllexport) long RegisterRpc(
        LPWSTR pszProtocolSequence
      , LPWSTR pszEndpoint
      , LPWSTR pszServiceClass
      , LPWSTR pszNetworkAddress
      , LPWSTR psErrorMessageBuffer
      , int errorMessageBufferLength
      )
    {
        LPWSTR pszSpn = NULL;
        long lStatus;

        // The RpcServerUseProtseqEp function tells the RPC run-time library to 
        // use the specified protocol sequence combined with the specified 
        // endpoint for receiving remote procedure calls.
        //
        lStatus = RpcServerUseProtseqEp( ( RPC_WSTR ) pszProtocolSequence,
                                            RPC_C_PROTSEQ_MAX_REQS_DEFAULT,
                                            ( RPC_WSTR ) pszEndpoint,
                                            NULL );

        if ( lStatus )
        {
            swprintf_s(psErrorMessageBuffer, errorMessageBufferLength, L"RpcServerUseProtseqEp returned 0x%x", lStatus );
            return KEYPROVIDER_FAILED;
        }
	
        // User did not specify spn, construct one.
        if ( NULL == pszSpn )
        {
            // Add 2 for the slash character and null terminator.
            INT nSpnLength = wcslen( pszServiceClass ) + wcslen( pszNetworkAddress ) + 2;
            pszSpn = ( LPWSTR ) malloc( nSpnLength * sizeof( wchar_t ) ); 

            if ( !pszSpn )
            {
                return KEYPROVIDER_NOT_ENOUGH_MEMORY;
            }

            wcscpy_s( pszSpn, nSpnLength, pszServiceClass );
            wcscat_s( pszSpn, nSpnLength, L"/" );
            wcscat_s( pszSpn, nSpnLength, pszNetworkAddress );
        }

        // The RpcServerRegisterIfEx function registers an interface with 
        // the RPC run-time library.
        //
        lStatus = RpcServerRegisterIfEx( KeyProviderListener_v1_0_s_ifspec,
                                         NULL,
                                         NULL, 
                                         RPC_IF_ALLOW_CALLBACKS_WITH_NO_AUTH, 
                                         RPC_C_LISTEN_MAX_CALLS_DEFAULT, 
                                         NULL );

        if ( lStatus )
        {
            swprintf_s(psErrorMessageBuffer, errorMessageBufferLength, L"RpcServerRegisterIfEx returned 0x%x", lStatus );
            return KEYPROVIDER_FAILED;
        }

        // The RpcServerRegisterAuthInfo function registers authentication 
        // information with the RPC run-time library.
        //
        // Using Negotiate as security provider.
        lStatus = RpcServerRegisterAuthInfo( ( RPC_WSTR ) pszSpn,
                                             RPC_C_AUTHN_GSS_NEGOTIATE,
                                             NULL,
                                             NULL );
	
        if ( lStatus )
        {
            swprintf_s(psErrorMessageBuffer, errorMessageBufferLength, L"RpcServerRegisterAuthInfo returned 0x%x", lStatus );
            return KEYPROVIDER_FAILED;
        }	

        // The RpcServerListen function signals the RPC run-time library to listen 
        // for remote procedure calls.
        //
        lStatus = RpcServerListen( 1,
                                   RPC_C_LISTEN_MAX_CALLS_DEFAULT,
                                   TRUE );
        if ( lStatus )
        {
            swprintf_s(psErrorMessageBuffer, errorMessageBufferLength, L"RpcServerListen returned: 0x%x", lStatus );
            return KEYPROVIDER_FAILED;
        }

        return 0;
    }

    __declspec(dllexport) long UnregisterRpc(
        LPWSTR errorMessageBuffer
      , int errorMessageBufferLength
      )
    {
        RPC_STATUS lStatus = -1;

        // The RpcMgmtStopServerListening function tells a server to stop listening for remote procedure calls.
        //
        lStatus = RpcMgmtStopServerListening( NULL );
        if (0 != lStatus )
        {
            return lStatus;
        }

        // The RpcServerUnregisterIf function removes an interface from the RPC run-time library registry.
        //
        lStatus = RpcServerUnregisterIf( NULL, NULL, TRUE );
        if (0 != lStatus )
        {
            return lStatus;
        }

        return 0;
    }
}