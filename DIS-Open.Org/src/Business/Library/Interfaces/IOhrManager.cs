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
using DIS.Data.DataContract;
using DIS.Data.DataAccess;

namespace DIS.Business.Library
{
    public interface IOhrManager
    {
        void UpdateOhrAfterNotification(List<Ohr> ohrs);

        List<Ohr> GetConfirmedOhrs();

        List<Ohr> GetOhrsNotBeenSent();

        List<Ohr> GetReportedOhrs();

        List<Ohr> GetReadyOhrs();

        List<Ohr> GetFailedOhrs();

        Ohr GenerateOhr(List<KeyInfo> keys, KeyStoreContext context = null);

        Ohr UpdateOhrAfterAckRetrieved(Ohr ohr, KeyStoreContext context = null);

        void UpdateOhrsAfterAckReady(List<Ohr> ohrs);

        string GenerateOhrToFile(List<KeyInfo> keys, string outputPath);

        Ohr RetrieveOhrAck(string path);

        void UpdateOhrAfterReported(Ohr ohr, KeyStoreContext context);

        void UpdateOhrWhenAckFailed(Ohr ohr);

    }
}
