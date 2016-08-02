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
    /// <summary>
    /// The system diagnostic result
    /// </summary>
    public class DiagnosticResult
    {
        private DiagnosticResultType diagnosticResultType = DiagnosticResultType.Ok;

        public DiagnosticResultType DiagnosticResultType
        {
            get { return diagnosticResultType; }
            set { diagnosticResultType = value; }
        }

        public Exception Exception { get; set; }
        
    }

    public enum DiagnosticResultType
    {
        Ok = 1,
        Error = 2,

    }
}
