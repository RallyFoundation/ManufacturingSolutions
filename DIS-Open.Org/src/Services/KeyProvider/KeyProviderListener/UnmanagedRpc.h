// UnmanagedRpc.h : unmanaged code to register or unregister the RPC calls.
//

#pragma once

class UnmanagedRpc
{
public:
	UnmanagedRpc(void);
	~UnmanagedRpc(void);

    long RegisterRpc(
          LPWSTR pszProtocolSequence
        , LPWSTR pszEndpoint
        , LPWSTR pszServiceClass
        , LPWSTR pszNetworkAddress
        , LPWSTR psErrorMessageBuffer
        , int errorMessageBufferLength
        );

    long UnregisterRpc(
        LPWSTR errorMessageBuffer
        , int errorMessageBufferLength
        );
};
