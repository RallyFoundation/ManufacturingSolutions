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

using System.Collections.Generic;
using System.Linq;
using DIS.Data.DataContract;
using DIS.Data.DataAccess;

namespace DIS.Business.Proxy
{
    public partial class KeyProxy
    {
        #region export keys to file

        //export keys:fulfilled,report,toolkey,CBR
        public List<KeyOperationResult> ExportKeys(ExportParameters exportParameters)
        {
            List<KeyOperationResult> result = null;
            switch (exportParameters.ExportType)
            {
                case Constants.ExportType.CBR:
                    return base.ExportCbr(exportParameters,
                        k => cbrManager.GenerateCbrToFile(k, exportParameters.OutputPath));
                case Constants.ExportType.FulfilledKeys:
                    {
                        var exportKeys = base.ExportFulfilledKeys(exportParameters);
                        if (GetIsCarbonCopy())
                            base.UpdateKeysToCarbonCopy(
                                exportKeys.Where(r => !r.Failed && r.KeyInDb.KeyInfoEx.KeyType == KeyType.MBR).Select(r => r.KeyInDb).ToList(), true);
                        return exportKeys;
                    }
                case Constants.ExportType.ReportKeys:
                    return base.ExportBoundKeys(exportParameters);
                case Constants.ExportType.ToolKeys:
                    return base.ExportToolKeys(exportParameters);
            }
            return result;
        }

        //export duplicated CBR
        public new List<KeyOperationResult> ExportDuplicatedCbr(Cbr cbr, string outputPath, string @operator)
        {
            var result = base.ExportDuplicatedCbr(cbr, outputPath, @operator);
            this.cbrManager.UpdateCbrsAfterExported(cbr);
            return result;
        }

        public List<Cbr> GetCbrsDuplicated()
        {
            return cbrManager.GetCbrsDuplicated();
        }

        public List<Cbr> GetFailedCbrs()
        {
            return cbrManager.GetFailedCbrs();
        }

        #endregion

        #region Import keys from file

        //import fulfilled keys from ULS
        public new List<KeyOperationResult> ImportULSFulfilledKeys(string filePath, HeadQuarter headquarter, bool IsCheckFileSignature)
        {
            return base.ImportULSFulfilledKeys(filePath, headquarter, IsCheckFileSignature);
        }

        //import bound keys from DLS
        public List<KeyOperationResult> ImportDLSBoundKeys(string filePath, bool IsCheckFileSignature)
        {
            List<Subsidiary> subsidiaries = subsidiaryManager.GetSubsidiaries();
            return base.ImportDLSBoundKeys(filePath, subsidiaries, IsCheckFileSignature);
        }

        //import CBR/DuplicatedCbr from MS
        public List<KeyOperationResult> ImportCbr(string path, bool isDuplicated = false)
        {
            using (KeyStoreContext context = KeyStoreContext.GetContext(this.dbConnectionStr))//using (KeyStoreContext context = KeyStoreContext.GetContext())
            {
                var cbr = cbrManager.RetrieveCbrAck(path);
                cbrManager.UpdateCbrAfterAckRetrieved(cbr, isDuplicated, context);
                var result = base.UpdateKeysAfterRetrieveCbrAck(cbr, isDuplicated, context);
                if (GetIsCarbonCopy())
                    base.UpdateKeysToCarbonCopy(result.Where(r => !r.Failed).Select(r => r.KeyInDb).ToList(), true, context);
                context.SaveChanges();
                return result;
            }
        }

        //import Return keys.Ack from MS
        public List<KeyOperationResult> ImportReturnAckKeys(string filePath)
        {
            using (KeyStoreContext context = KeyStoreContext.GetContext(this.dbConnectionStr))//using (KeyStoreContext context = KeyStoreContext.GetContext())
            {
                List<KeyOperationResult> result = base.ImportReturnAckKeys(filePath, context);
                if (GetIsCarbonCopy())
                    UpdateKeysToCarbonCopy(result.Where(r => !r.Failed).Select(r => r.KeyInDb).ToList(), true, context);

                context.SaveChanges();
                return result;
            }
        }

        #endregion
    }
}
