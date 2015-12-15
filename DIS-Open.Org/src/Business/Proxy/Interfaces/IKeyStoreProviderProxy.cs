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

namespace DIS.Business.Proxy
{
    /// <summary>
    /// Interface of KeyStoreProvider
    /// </summary>
    public interface IKeyStoreProviderProxy
    {
        /// <summary>
        /// Retrieves a product key from the factory-floor key inventory for installation into the manufactured computer's BIOS
        /// </summary>
        int GetKey(string parameters, ref string productKeyInfo);

        /// <summary>
        /// Records confirmed product-key usage and stores a hardware identifier in the 
        /// factory-floor key inventory. 
        /// This information will be consolidated within the corporate key inventory 
        /// and uploaded to Microsoft. 
        /// </summary>
        int UpdateKey(string parameters, string productKeyInfo);

        /// <summary>
        /// Validates that the Key Provider server and key inventory database can be accessed. 
        /// </summary>
        /// <param name="productKeyInfo"></param>
        /// <returns></returns>
        int Ping(ref string productKeyInfo);
    }
}
