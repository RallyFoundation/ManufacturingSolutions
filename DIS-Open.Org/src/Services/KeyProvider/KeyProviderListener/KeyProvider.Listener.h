// KeyProvider.Listener.h

#pragma once

#include "Stdafx.h"

using namespace System;

namespace KeyProvider {

	public ref class Listener
	{
	public:
		Listener();
		~Listener() {delete pInner;}
	
        long RegisterRpc(
              String^ protocolSequence
            , String^ endpoint
            , String^ serviceClass
            , String^ networkAddress
            , String^ errorMessage
            );

        long UnregisterRpc(
              String^ errorMessage
            );

	protected:
		// explicit finalizer
		!Listener() {delete pInner;}

		UnmanagedRpc *pInner;
	};
}