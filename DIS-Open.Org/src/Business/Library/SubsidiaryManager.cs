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
using DIS.Data.DataAccess.Repository;
using DIS.Data.DataContract;
using DIS.Common.Utility;

namespace DIS.Business.Library
{
    /// <summary>
    /// SubsidiaryManager class
    /// </summary>
    public class SubsidiaryManager : ISubsidiaryManager
    {
        #region Private Members

        private ISubsidiaryRepository subsRepository;
       
        #endregion

        #region Constrcutor

        public SubsidiaryManager()
            : this(new SubsidiaryRepository())
        {
        }

        public SubsidiaryManager(string dbConnectionString) 
        {
            this.subsRepository = new SubsidiaryRepository(dbConnectionString);
        }

        public SubsidiaryManager(ISubsidiaryRepository subsRepository)
        {
            if (subsRepository == null)
                this.subsRepository = new SubsidiaryRepository();
            else
                this.subsRepository = subsRepository;
        }

        #endregion

        #region Public Method

        public Subsidiary GetSubsidiary(int ssId)
        {
            return subsRepository.GetSubsidiary(ssId);
        }

        public Subsidiary GetSubsidiary(string userName)
        {
            return subsRepository.GetSubsidiary(userName);
        }

        /// <summary>
        /// get system Subsidiaries
        /// </summary>
        /// <returns></returns>
        public List<Subsidiary> GetSubsidiaries()
        {
            return subsRepository.GetSubsidiaries();
        }

        /// <summary>
        /// add new Subsidiary
        /// </summary>
        /// <param name="subs"></param>
        public void InsertSubsidiary(Subsidiary subs)
        {
            if (subs == null)
                throw new ApplicationException(string.Format("SubsidiaryManager:InsertSubsidiary - params error."));

            subsRepository.InsertSubsidiary(subs);
        }

        /// <summary>
        /// update Subsidiary
        /// </summary>
        /// <param name="subs"></param>
        public void UpdateSubsidiary(Subsidiary subs)
        {
            if (subs == null)
                throw new ApplicationException(string.Format("SubsidiaryManager:UpdateSubsidiary - params error."));

            subsRepository.UpdateSubsidiary(subs);
        }

        /// <summary>
        /// delete Subsidiary by SSID
        /// </summary>
        /// <param name="ssId"></param>
        public void DeleteSubsidiary(int ssId)
        {
            if (ssId == 0)
                throw new ApplicationException(string.Format("SubsidiaryManager:DeleteSubsidiary - params error."));
            subsRepository.DeleteSubsidiary(ssId);
        }

        public bool ValidateSubsidiary(string userName, string userKey)
        {
            List<Subsidiary> ss = subsRepository.GetSubsidiaries(userName, userKey);
            if (ss.Count > 0)
                return true;
            else
                return false;
        }
        #endregion

    }
}
