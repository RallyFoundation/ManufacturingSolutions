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
using DIS.Data.DataAccess.Repository;

namespace DIS.Business.Library
{
    public class HeadQuarterManager : IHeadQuarterManager
    {
        private IHeadQuarterRepository hqRepository;

        public HeadQuarterManager()
            : this(new HeadQuarterRepository())
        {
        }

        public HeadQuarterManager(string dbConnectionString)
        {
            this.hqRepository = new HeadQuarterRepository(dbConnectionString);
        }

        public HeadQuarterManager(IHeadQuarterRepository hqRepository)
        {
            if (hqRepository == null)
                this.hqRepository = new HeadQuarterRepository();
            else
                this.hqRepository = hqRepository;
        }

        public List<HeadQuarter> GetHeadQuarters()
        {
            return hqRepository.GetHeadQuarters();
        }

        public List<HeadQuarter> GetHeadQuarters(User user)
        {
            return hqRepository.GetHeadQuarters(user);
        }

        public HeadQuarter GetHeadQuarter(int hqId)
        {
            return hqRepository.GetHeadQuarter(hqId);
        }

        public bool ValidateHeadQuarter(string userName, string userKey)
        {
            List<HeadQuarter> ss = hqRepository.GetHeadQuarters(userName, userKey);
            if (ss.Count > 0)
                return true;
            else
                return false;
        }

        public void InsertHeadQuarter(HeadQuarter headQuarter)
        {
            hqRepository.InsertHeadQuarter(headQuarter);
        }

        public void UpdateHeadQuarter(HeadQuarter headQuarter)
        {
            hqRepository.UpdateHeadQuarter(headQuarter);
        }

        public void DeleteHeadQuarter(int headQuarterId)
        {
            hqRepository.DeleteHeadQuarter(headQuarterId);
        }

        public void SetAsDefault(User user, HeadQuarter headQuarter)
        {
            if (user == null || headQuarter == null)
                throw new ArgumentException(string.Format("User {0} or HeadQuater {1} not found!", user.LoginId, headQuarter.Description));

            hqRepository.UpdateUserHeadQuarter(user, headQuarter);
        }

        public HeadQuarter GetHeadQuarter(string userName)
        {
            return hqRepository.GetHeadQuarter(userName);
        }
    }
}
