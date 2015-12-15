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

using System.Linq;
using DIS.Data.DataContract;

namespace DIS.Business.KeyProviderLibrary.Parameters
{
    /// <summary>
    /// LicensablePartNumberParameter class is used to populate the Parameter relevant information
    /// </summary>
    class LicensablePartNumberParameter : IParameter
    {
        public void Attach(KeySearchCriteria searchCriteria, object value)
        {
            searchCriteria.MSPartNumber = value.ToString();
        }
    }
}
