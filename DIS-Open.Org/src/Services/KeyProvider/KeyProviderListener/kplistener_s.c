

/* this ALWAYS GENERATED file contains the RPC server stubs */


 /* File created by MIDL compiler version 8.00.0584 */
/* Compiler settings for kplistener.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 8.00.0584 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#if !defined(_M_IA64) && !defined(_M_AMD64) && !defined(_ARM_)


#pragma warning( disable: 4049 )  /* more than 64k source lines */
#if _MSC_VER >= 1200
#pragma warning(push)
#endif

#pragma warning( disable: 4211 )  /* redefine extern to static */
#pragma warning( disable: 4232 )  /* dllimport identity*/
#pragma warning( disable: 4024 )  /* array to pointer mapping*/
#pragma warning( disable: 4100 ) /* unreferenced arguments in x86 call */

#pragma optimize("", off ) 

#include <string.h>
#include "kplistener.h"

#define TYPE_FORMAT_STRING_SIZE   13                                
#define PROC_FORMAT_STRING_SIZE   167                               
#define EXPR_FORMAT_STRING_SIZE   1                                 
#define TRANSMIT_AS_TABLE_SIZE    0            
#define WIRE_MARSHAL_TABLE_SIZE   0            

typedef struct _kplistener_MIDL_TYPE_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ TYPE_FORMAT_STRING_SIZE ];
    } kplistener_MIDL_TYPE_FORMAT_STRING;

typedef struct _kplistener_MIDL_PROC_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ PROC_FORMAT_STRING_SIZE ];
    } kplistener_MIDL_PROC_FORMAT_STRING;

typedef struct _kplistener_MIDL_EXPR_FORMAT_STRING
    {
    long          Pad;
    unsigned char  Format[ EXPR_FORMAT_STRING_SIZE ];
    } kplistener_MIDL_EXPR_FORMAT_STRING;


static const RPC_SYNTAX_IDENTIFIER  _RpcTransferSyntax = 
{{0x8A885D04,0x1CEB,0x11C9,{0x9F,0xE8,0x08,0x00,0x2B,0x10,0x48,0x60}},{2,0}};

extern const kplistener_MIDL_TYPE_FORMAT_STRING kplistener__MIDL_TypeFormatString;
extern const kplistener_MIDL_PROC_FORMAT_STRING kplistener__MIDL_ProcFormatString;
extern const kplistener_MIDL_EXPR_FORMAT_STRING kplistener__MIDL_ExprFormatString;

/* Standard interface: KeyProviderListener, ver. 1.0,
   GUID={0x27E0516C,0x7F5A,0x478B,{0x82,0x03,0xBF,0x49,0x2E,0x5B,0x13,0xA3}} */


extern const MIDL_SERVER_INFO KeyProviderListener_ServerInfo;

extern const RPC_DISPATCH_TABLE KeyProviderListener_v1_0_DispatchTable;

static const RPC_SERVER_INTERFACE KeyProviderListener___RpcServerInterface =
    {
    sizeof(RPC_SERVER_INTERFACE),
    {{0x27E0516C,0x7F5A,0x478B,{0x82,0x03,0xBF,0x49,0x2E,0x5B,0x13,0xA3}},{1,0}},
    {{0x8A885D04,0x1CEB,0x11C9,{0x9F,0xE8,0x08,0x00,0x2B,0x10,0x48,0x60}},{2,0}},
    (RPC_DISPATCH_TABLE*)&KeyProviderListener_v1_0_DispatchTable,
    0,
    0,
    0,
    &KeyProviderListener_ServerInfo,
    0x04000000
    };
RPC_IF_HANDLE KeyProviderListener_v1_0_s_ifspec = (RPC_IF_HANDLE)& KeyProviderListener___RpcServerInterface;

extern const MIDL_STUB_DESC KeyProviderListener_StubDesc;


#if !defined(__RPC_WIN32__)
#error  Invalid build platform for this stub.
#endif
#if !(TARGET_IS_NT60_OR_LATER)
#error You need Windows Vista or later to run this stub because it uses these features:
#error   compiled for Windows Vista.
#error However, your C/C++ compilation flags indicate you intend to run this app on earlier systems.
#error This app will fail with the RPC_X_WRONG_STUB_VERSION error.
#endif


static const kplistener_MIDL_PROC_FORMAT_STRING kplistener__MIDL_ProcFormatString =
    {
        0,
        {

	/* Procedure GetKey */

			0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/*  2 */	NdrFcLong( 0x0 ),	/* 0 */
/*  6 */	NdrFcShort( 0x0 ),	/* 0 */
/*  8 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 10 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 12 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 14 */	NdrFcShort( 0x0 ),	/* 0 */
/* 16 */	NdrFcShort( 0x13a0 ),	/* 5024 */
/* 18 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x3,		/* 3 */
/* 20 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 22 */	NdrFcShort( 0x0 ),	/* 0 */
/* 24 */	NdrFcShort( 0x0 ),	/* 0 */
/* 26 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter h1 */

/* 28 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 30 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 32 */	NdrFcShort( 0x4 ),	/* Type Offset=4 */

	/* Parameter pszParameters */

/* 34 */	NdrFcShort( 0x12 ),	/* Flags:  must free, out, */
/* 36 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 38 */	NdrFcShort( 0x6 ),	/* Type Offset=6 */

	/* Parameter pszDM */

/* 40 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 42 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 44 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure UpdateKey */


	/* Return value */

/* 46 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 48 */	NdrFcLong( 0x0 ),	/* 0 */
/* 52 */	NdrFcShort( 0x1 ),	/* 1 */
/* 54 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 56 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 58 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 60 */	NdrFcShort( 0x0 ),	/* 0 */
/* 62 */	NdrFcShort( 0x8 ),	/* 8 */
/* 64 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x3,		/* 3 */
/* 66 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 68 */	NdrFcShort( 0x0 ),	/* 0 */
/* 70 */	NdrFcShort( 0x0 ),	/* 0 */
/* 72 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter h1 */

/* 74 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 76 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 78 */	NdrFcShort( 0x4 ),	/* Type Offset=4 */

	/* Parameter pszParameters */

/* 80 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 82 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 84 */	NdrFcShort( 0x4 ),	/* Type Offset=4 */

	/* Parameter pszDM */

/* 86 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 88 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 90 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure Ping */


	/* Return value */

/* 92 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 94 */	NdrFcLong( 0x0 ),	/* 0 */
/* 98 */	NdrFcShort( 0x2 ),	/* 2 */
/* 100 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 102 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 104 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 106 */	NdrFcShort( 0x0 ),	/* 0 */
/* 108 */	NdrFcShort( 0x13a0 ),	/* 5024 */
/* 110 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x2,		/* 2 */
/* 112 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 114 */	NdrFcShort( 0x0 ),	/* 0 */
/* 116 */	NdrFcShort( 0x0 ),	/* 0 */
/* 118 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter h1 */

/* 120 */	NdrFcShort( 0x12 ),	/* Flags:  must free, out, */
/* 122 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 124 */	NdrFcShort( 0x6 ),	/* Type Offset=6 */

	/* Parameter pszMSG */

/* 126 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 128 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 130 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure Shutdown */


	/* Return value */

/* 132 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 134 */	NdrFcLong( 0x0 ),	/* 0 */
/* 138 */	NdrFcShort( 0x3 ),	/* 3 */
/* 140 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 142 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 144 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 146 */	NdrFcShort( 0x0 ),	/* 0 */
/* 148 */	NdrFcShort( 0x8 ),	/* 8 */
/* 150 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x1,		/* 1 */
/* 152 */	0x8,		/* 8 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 154 */	NdrFcShort( 0x0 ),	/* 0 */
/* 156 */	NdrFcShort( 0x0 ),	/* 0 */
/* 158 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter h1 */

/* 160 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 162 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 164 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

			0x0
        }
    };

static const kplistener_MIDL_TYPE_FORMAT_STRING kplistener__MIDL_TypeFormatString =
    {
        0,
        {
			NdrFcShort( 0x0 ),	/* 0 */
/*  2 */	
			0x11, 0x8,	/* FC_RP [simple_pointer] */
/*  4 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/*  6 */	
			0x1d,		/* FC_SMFARRAY */
			0x1,		/* 1 */
/*  8 */	NdrFcShort( 0x1388 ),	/* 5000 */
/* 10 */	0x5,		/* FC_WCHAR */
			0x5b,		/* FC_END */

			0x0
        }
    };

static const unsigned short KeyProviderListener_FormatStringOffsetTable[] =
    {
    0,
    46,
    92,
    132
    };


static const MIDL_STUB_DESC KeyProviderListener_StubDesc = 
    {
    (void *)& KeyProviderListener___RpcServerInterface,
    MIDL_user_allocate,
    MIDL_user_free,
    0,
    0,
    0,
    0,
    0,
    kplistener__MIDL_TypeFormatString.Format,
    1, /* -error bounds_check flag */
    0x60001, /* Ndr library version */
    0,
    0x8000248, /* MIDL Version 8.0.584 */
    0,
    0,
    0,  /* notify & notify_flag routine table */
    0x1, /* MIDL flag */
    0, /* cs routines */
    0,   /* proxy/server info */
    0
    };

static const RPC_DISPATCH_FUNCTION KeyProviderListener_table[] =
    {
    NdrServerCall2,
    NdrServerCall2,
    NdrServerCall2,
    NdrServerCall2,
    0
    };
static const RPC_DISPATCH_TABLE KeyProviderListener_v1_0_DispatchTable = 
    {
    4,
    (RPC_DISPATCH_FUNCTION*)KeyProviderListener_table
    };

static const SERVER_ROUTINE KeyProviderListener_ServerRoutineTable[] = 
    {
    (SERVER_ROUTINE)GetKey,
    (SERVER_ROUTINE)UpdateKey,
    (SERVER_ROUTINE)Ping,
    (SERVER_ROUTINE)Shutdown
    };

static const MIDL_SERVER_INFO KeyProviderListener_ServerInfo = 
    {
    &KeyProviderListener_StubDesc,
    KeyProviderListener_ServerRoutineTable,
    kplistener__MIDL_ProcFormatString.Format,
    KeyProviderListener_FormatStringOffsetTable,
    0,
    0,
    0,
    0};
#pragma optimize("", on )
#if _MSC_VER >= 1200
#pragma warning(pop)
#endif


#endif /* !defined(_M_IA64) && !defined(_M_AMD64) && !defined(_ARM_) */

