//------------------------------------------------------------------------------
// Copyright (c) 2010 Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Module Name:
//              resources.h
//
// Abstract:
// 	            Error constants for DMTool client and the Key Provider server.
//
// Author:
//              Don Schuy (v-dschuy)    JUN 08, 2010
//
// Environment:
//		        WinPE, Windows XP or better
//
// Revision History:
//		        JUN 08, 2010, v-dschuy
//			    Created.
//------------------------------------------------------------------------------ 

#ifndef RESOURCES_H_
#define RESOURCES_H_

#define DM_FILE "dm.xml"

// DMTool error constants are in the range of 100-199
#define DMTOOL_SUCCESS 100
#define DMTOOL_FAILED 101
#define DMTOOL_NOT_ENOUGH_MEMORY 102
#define DMTOOL_CONFIGURATION_OPTION_NOT_FOUND 103
#define DMTOOL_NETWORK_ADDRESS_NOT_SPECIFIED 104
#define DMTOOL_ENDPOINT_NOT_SPECIFIED 105
#define DMTOOL_WRITE_XML_FAILED 106
#define DMTOOL_READ_XML_FAILED 107
#define DMTOOL_TAG_NOT_FOUND 108
#define DMTOOL_UPDATE_XML_FAILED 109
#define DMTOOL_BADLY_FORMED_XML 110
#define DMTOOL_RPC_INITIALIZATION_FAILED 111
#define DMTOOL_RPC_INVOCATION_FAILED 112
#define DMTOOL_RPC_UNBIND_FAILED 113

// Key Provider error constants are in the range of 300-399
#define KEYPROVIDER_SUCCESS 300
#define KEYPROVIDER_FAILED 301
#define KEYPROVIDER_FAILED_DB_CONNECTION 302
#define KEYPROVIDER_BADLY_FORMED_XML 303
#define KEYPROVIDER_MISSING_KEY_TAG 304
#define KEYPROVIDER_MISSING_PRODUCTKEYSTATE_TAG 305
#define KEYPROVIDER_NOT_ENOUGH_MEMORY 306

#endif