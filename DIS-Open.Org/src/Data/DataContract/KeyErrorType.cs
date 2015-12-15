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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIS.Data.DataContract
{
    public enum KeyErrorType
    {
        None,
        Invalid,
        StateInvalid,
        SsIdInvalid,
        NotFound,
        Duplicate,
        DuplicateImport,
        AlreadyAssigned,
        ToBeConfirmed,
        NetworkFailure,
        KeyTypeInvalid,
        FileStateInvalid,
        InvalidOriginalFile,//- Rally, Nov 24, 2014
        OriginalFileNotFound//- Rally, Nov 24, 2014
    }
}
