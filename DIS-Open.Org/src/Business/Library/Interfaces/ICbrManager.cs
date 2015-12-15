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
    public interface ICbrManager
    {
        List<Cbr> GetCbrsNotBeenSent(bool includeKeyInfo = false);

        Cbr GetFirstSentCbr();

        //Cbr already been sent but still not retrive ack
        List<Cbr> GetReportedCbrs();

        List<Cbr> GetReadyCbrs();

        List<Cbr> GetFailedCbrs();

        Cbr GenerateCbr(List<KeyInfo> keys, bool isExport = false, KeyStoreContext context = null);

        void UpdateCbrIfSendingFailed(Cbr cbr);

        void UpdateCbrIfSearchResultEmpty(Cbr cbr);

        void UpdateCbrAfterReported(Cbr cbr, KeyStoreContext context);

        void UpdateCbrsAfterAckReady(List<Cbr> cbrs);

        void UpdateCbrAfterAckRetrieved(Cbr cbr, bool isDuplicated = false, KeyStoreContext context = null);

        void UpdateCbrWhenAckFailed(Cbr cbr);

        string GenerateCbrToFile(List<KeyInfo> keys, string outputPath);

        #region Duplicated Cbr

        List<Cbr> GetCbrsDuplicated();

        void UpdateCbrsAfterExported(Cbr cbr);

        void UpdateCbrsAfterImported(Cbr cbr);

        Cbr RetrieveCbrAck(string path);

        #endregion
    }
}
