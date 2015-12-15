//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

namespace DIS.Business.KeyProviderLibrary
{
    /// <summary>
    /// Return value and key state enumerators
    /// </summary>
    public enum ReturnValue
    {
        /// <summary>
        /// 
        /// </summary>
        MSG_KEYPROVIDER_SUCCESS = 300,
        /// <summary>
        /// 
        /// </summary>
        MSG_KEYPROVIDER_FAILED = -1073741523,
        MSG_KEYPROVIDER_FAILED_DB_CONNECTION = -1073741522,
        MSG_KEYPROVIDER_XML_SCHEMA_FORMAT_VIOLATION = -1073741521,
        MSG_KEYPROVIDER_XML_INVALID_PARAMETER = -1073741520,
        MSG_KEYPROVIDER_XML_SCHEMA_MISSING_PRODUCT_KEY_TAG = -1073741519,
        MSG_KEYPROVIDER_XML_SCHEMA_MISSING_PRODUCT_KEY_STATE_TAG = -1073741518,
        MSG_KEYPROVIDER_NO_KEYS_AVAILABLE_FOR_SPECIFIED_PARAMETERS = -1073741517,
        MSG_KEYPROVIDER_INVALID_PRODUCT_KEY_STATE_TRANSITION = -1073741516
    };
}
