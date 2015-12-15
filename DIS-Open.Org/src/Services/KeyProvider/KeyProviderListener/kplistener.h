

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0584 */
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 500
#endif

/* verify that the <rpcsal.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCSAL_H_VERSION__
#define __REQUIRED_RPCSAL_H_VERSION__ 100
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__


#ifndef __kplistener_h__
#define __kplistener_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __KeyProviderListener_INTERFACE_DEFINED__
#define __KeyProviderListener_INTERFACE_DEFINED__

/* interface KeyProviderListener */
/* [version][uuid] */ 

#define	DM_SIZE	( 2500 )

typedef wchar_t DM_STRING[ 2500 ];

int GetKey( 
    /* [in] */ handle_t h1,
    /* [string][in] */ __RPC__in_string wchar_t *pszParameters,
    /* [out] */ __RPC__out_ecount_full(DM_SIZE) DM_STRING pszDM);

int UpdateKey( 
    /* [in] */ handle_t h1,
    /* [string][in] */ __RPC__in_string wchar_t *pszParameters,
    /* [string][in] */ __RPC__in_string wchar_t *pszDM);

int Ping( 
    /* [in] */ handle_t h1,
    /* [out] */ __RPC__out_ecount_full(DM_SIZE) DM_STRING pszMSG);

int Shutdown( 
    /* [in] */ handle_t h1);



extern RPC_IF_HANDLE KeyProviderListener_v1_0_c_ifspec;
extern RPC_IF_HANDLE KeyProviderListener_v1_0_s_ifspec;
#endif /* __KeyProviderListener_INTERFACE_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


