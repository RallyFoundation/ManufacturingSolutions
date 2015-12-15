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
using DIS.Business.Library.Properties;
using DIS.Data.DataAccess.Repository;
using DIS.Data.DataContract;
using DIS.Common.Utility;
using Microsoft.ServiceModel.Web;
using DIS.Data.DataAccess;

namespace DIS.Business.Library {
    public class FulfillmentManager : IFulfillmentManager {
        private IFulfillmentRepository fulfillRepository;

        public FulfillmentManager()
            : this(new FulfillmentRepository()) {
        }

        public FulfillmentManager(string dbConnectionString) 
        {
            this.fulfillRepository = new FulfillmentRepository(dbConnectionString);
        }

        public FulfillmentManager(IFulfillmentRepository fulfillRepository) {
            if (fulfillRepository == null)
                this.fulfillRepository = new FulfillmentRepository();
            else
                this.fulfillRepository = fulfillRepository;
        }

        public FulfillmentInfo GetFirstFulfilledFulfillment() {
            return fulfillRepository.GetFirstFulfillment(FulfillmentStatus.Fulfilled);
        }

        public List<FulfillmentInfo> GetFulfillments(List<string> fulfillmentNumbers) {
            if (fulfillmentNumbers == null)
                throw new ApplicationException("Fulfillment numbers cannot be null.");

            return fulfillRepository.GetFulfillments(fulfillmentNumbers);
        }

        public List<FulfillmentInfo> GetFailedFulfillments(bool shouldIncludeExpired) {
            return fulfillRepository.GetFulfillments(FulfillmentStatus.Failed,
                shouldIncludeExpired ? null : (DateTime?)DateTime.UtcNow.AddHours(-Constants.RefulfillmentTimeLimit));
        }

        public void SaveAvailableFulfillments(List<FulfillmentInfo> infoes) {
            if (infoes == null || infoes.Count == 0)
                throw new ApplicationException("No fulfillments to save.");

            IEqualityComparer<FulfillmentInfo> comparer = new FulfillmentComparer();
            List<FulfillmentInfo> existFulfillments = fulfillRepository.GetFulfillments(infoes.Select(i => i.FulfillmentNumber).ToList());
            fulfillRepository.InsertFulfillments(infoes.Except(existFulfillments, comparer).ToList());
            foreach (FulfillmentInfo f in existFulfillments.Where(f => f.FulfillmentStatus == FulfillmentStatus.Failed).Intersect(infoes, comparer)) {
                f.FulfillmentStatus = FulfillmentStatus.Ready;
                fulfillRepository.UpdateFulfillment(f);
            }
        }

        public void RetrieveFulfilment(List<FulfillmentInfo> infoes,
            Func<FulfillmentInfo, List<KeyInfo>> fulfillKeys) {
            if (infoes.Count > 0)
                SaveAvailableFulfillments(infoes);

            foreach (FulfillmentInfo info in fulfillRepository.GetFulfillments(FulfillmentStatus.Ready)) {
                try {
                    UpdateFullfillmentToInProgess(info);

                    List<KeyInfo> keys = fulfillKeys(info);
                    info.Keys = keys;
                    info.FulfillmentStatus = FulfillmentStatus.Fulfilled;
                    fulfillRepository.UpdateFulfillment(info);
                }
                catch (Exception ex) {
                    if (ex is WebProtocolException)
                    {
                        info.FulfillmentStatus = FulfillmentStatus.Failed;
                        fulfillRepository.UpdateFulfillment(info);
                        MessageLogger.LogSystemRunning("Fulfillment Failed", string.Format(
                            "Fulfillment {0} has failed at {1}. Please contact Microsoft and request a re-fulfillment.",
                            info.FulfillmentNumber, DateTime.Now.ToString()), this.fulfillRepository.GetDBConnectionString());
                    }
                    else
                    {
                        info.FulfillmentStatus = FulfillmentStatus.Ready;
                        fulfillRepository.UpdateFulfillment(info);
                    }
                    ExceptionHandler.HandleException(ex, this.fulfillRepository.GetDBConnectionString());
                }
            }
        }

        public void UpdateFulfillmentToCompleted(FulfillmentInfo info, KeyStoreContext context) {
            info.FulfillmentStatus = FulfillmentStatus.Completed;
            fulfillRepository.UpdateFulfillment(info, context);
        }

        public void UpdateFulfillmentFailedWhenDiskIsFull()
        {
            List<FulfillmentInfo> readyInfoes = fulfillRepository.GetFulfillments(FulfillmentStatus.InProgress);
            if (readyInfoes.Count > 0)
            {
                foreach (var info in readyInfoes)
                {
                    info.FulfillmentStatus = FulfillmentStatus.Failed;
                    fulfillRepository.UpdateFulfillment(info);
                }
            }
        }

        private void UpdateFullfillmentToInProgess(FulfillmentInfo info)
        {
            info.FulfillmentStatus = FulfillmentStatus.InProgress;
            fulfillRepository.UpdateFulfillment(info);
        }
    }
}
