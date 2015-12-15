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

using System.Runtime.Serialization;

namespace DIS.Data.ServiceContract
{
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class OEMHardwareReport
    {
        [DataMember(Order = 1)]
        public System.Nullable<FormFactorL1Enum> FRM_FACTOR_CL1 { get; set; }

        [DataMember(Order = 2)]
        public System.Nullable<FormFactorL2Enum> FRM_FACTOR_CL2 { get; set; }

        [DataMember(Order = 3)]
        public System.Nullable<TouchEnum> TOUCH_SCREEN { get; set; }

        [DataMember(Order = 4)]
        public System.Nullable<decimal> SCREEN_SIZE { get; set; }

        [DataMember(Order = 5)]
        public string PC_MODEL_SKU { get; set; }
    }
}
