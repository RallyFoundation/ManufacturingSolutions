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
using DIS.Business.Client;
using DIS.Business.Library;
using DIS.Data.DataContract;
using System.Configuration;
using System.Linq;

namespace DIS.Business.Proxy
{
    public partial class KeyProxy : KeyManager, IKeyProxy
    {
        #region Private Members & Constructor

        private IServiceClient msClient
        {
            get 
            { 
                //return new ServiceClient(CurrentHeadQuarterId, (CallDirection.Internal | CallDirection.Microsoft), user); 
                return new ServiceClient(CurrentHeadQuarterId, (CallDirection.Internal | CallDirection.Microsoft), user, this.cloudConfigId, this.dbConnectionStr, this.cloudCustId); 
            }
        }
        private IServiceClient ulsClient
        {
            get 
            { 
                //return new ServiceClient(CurrentHeadQuarterId, (CallDirection.Internal | CallDirection.UpLevelSystem), user); 
                return new ServiceClient(CurrentHeadQuarterId, (CallDirection.Internal | CallDirection.UpLevelSystem), user, this.cloudConfigId, this.dbConnectionStr, this.cloudCustId); 
            }
        }
        private IServiceClient GetDlsClient(int ssId)
        {
            //return new ServiceClient(ssId, (CallDirection.Internal | CallDirection.DownLevelSystem), user);
            return new ServiceClient(ssId, (CallDirection.Internal | CallDirection.DownLevelSystem), user, this.cloudConfigId, this.dbConnectionStr, this.cloudCustId);
        }

        private IConfigManager configManager;
        private ICbrManager cbrManager;
        private IOhrManager ohrManager;
        private IFulfillmentManager fulfillManager;
        private ISubsidiaryManager subsidiaryManager;
        private IHeadQuarterManager headQuarterManager;
        private User user;

        private string cloudCustId;
        private string cloudConfigId;
        private string dbConnectionStr;

        public KeyProxy(User user, int? hqId)
            : this(user, hqId, null, null, null, null, null, null)
        {
        }

        public KeyProxy(User user, int? hqId, string dbConnectionString, string configurationId, string cloudCustomerId) : base(hqId, dbConnectionString)
        {
            this.user = user;

            this.cloudCustId = cloudCustomerId;

            this.cloudConfigId = configurationId;

            this.dbConnectionStr = dbConnectionString;

            this.configManager = new ConfigManager(dbConnectionString);

            this.cbrManager = new CbrManager(dbConnectionString);

            this.fulfillManager = new FulfillmentManager(dbConnectionString);

            this.subsidiaryManager = new SubsidiaryManager(dbConnectionString);

            this.headQuarterManager = new HeadQuarterManager(dbConnectionString);

            this.ohrManager = new OhrManager(dbConnectionString);
        }

        public KeyProxy(User user, int? hqId, IConfigManager configManager,
            ICbrManager cbrManager, 
            IFulfillmentManager fulfillManager, 
            ISubsidiaryManager subsidiaryManager, 
            IHeadQuarterManager headQuarterManager,
            IOhrManager ohrManager)
            : base(hqId)
        {
            this.user = user;

            if (configManager == null)
                this.configManager = new ConfigManager();
            else
                this.configManager = configManager;

            if (cbrManager == null)
                this.cbrManager = new CbrManager();
            else
                this.cbrManager = cbrManager;

            if (fulfillManager == null)
                this.fulfillManager = new FulfillmentManager();
            else
                this.fulfillManager = fulfillManager;

            if (subsidiaryManager == null)
                this.subsidiaryManager = new SubsidiaryManager();
            else
                this.subsidiaryManager = subsidiaryManager;

            if (headQuarterManager == null)
                this.headQuarterManager = new HeadQuarterManager();
            else
                this.headQuarterManager = headQuarterManager;

            if (ohrManager == null)
                this.ohrManager = new OhrManager();
            else
                this.ohrManager = ohrManager;
        }

        #endregion

        #region Diagnostic

        public void TestInternalConnection()
        {
            ulsClient.TestInternal();
        }

        public void TestUpLevelSystemConnection()
        {
            ulsClient.TestExternal();
        }

        #endregion

        #region Fulfillment

        public List<FulfillmentInfo> GetFulfillments(List<string> fulfillmentNumbers)
        {
            return fulfillManager.GetFulfillments(fulfillmentNumbers);
        }

        public List<FulfillmentInfo> GetFailedFulfillments(bool shouldIncludeExpired)
        {
            return fulfillManager.GetFailedFulfillments(shouldIncludeExpired);
        }

        public List<Ohr> GetConfirmedOhrs()
        {
            return ohrManager.GetConfirmedOhrs();
        }

        public void UpdateOhrAfterNotification(List<Ohr> ohrs)
        {
            ohrManager.UpdateOhrAfterNotification(ohrs);
        }

        #endregion

        #region Private Methods

        private bool GetIsCarbonCopy() {
            if (Constants.InstallType == InstallType.Tpi) {
                HeadQuarter headQuarter = headQuarterManager.GetHeadQuarter(CurrentHeadQuarterId.Value);
                if (headQuarter == null)
                    throw new ApplicationException(string.Format("HeadQuarterId {0} does not exist.", CurrentHeadQuarterId.Value));
                return headQuarter.IsCarbonCopy;
            }
            else
                return false;
        }

        private bool GetIsCentralizeMode() {
            if (Constants.InstallType == InstallType.Tpi) {
                HeadQuarter headQuarter = headQuarterManager.GetHeadQuarter(CurrentHeadQuarterId.Value);
                if (headQuarter == null)
                    throw new ApplicationException(string.Format("HeadQuarterId {0} does not exist.", CurrentHeadQuarterId.Value));
                return headQuarter.IsCentralizedMode;
            }
            else
                return true;
        }

        #endregion

    }
}

