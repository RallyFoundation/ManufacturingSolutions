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
using System.Data.SqlTypes;

namespace DIS.Business.Library
{
    public partial class LocalKeyManager : ILocalKeyManager
    {
        public PagedList<KeyInfo> SearchKeys(KeySearchCriteria searchCriteria)
        {
            var headSearchCriteria = ConvertSearchCriteria(searchCriteria);
            headSearchCriteria.HqId = CurrentHeadQuarterId;
            if (Constants.InstallType == InstallType.Oem)
            {
                headSearchCriteria.ShouldCarbonCopy = true;
            }
            return keyRepository.SearchKeys(headSearchCriteria);
        }

        public List<KeyInfo> SearchBoundKeysToReport(KeySearchCriteria searchCriteria)
        {
            return keyRepository.SearchKeys(
                GetBoundKeyToReportSearchCriteria(searchCriteria));
        }

        public List<KeyInfo> SearchBoundKeysToReport(List<KeyGroup> keyGroups)
        {
            return keyRepository.SearchKeys(keyGroups);
        }

        public List<KeyGroup> SearchBoundKeyGroupsToReport(KeySearchCriteria searchCriteria)
        {
            return keyRepository.SearchKeyGroups(
                GetBoundKeyToReportSearchCriteria(searchCriteria));
        }

        public List<KeyInfo> SearchBoundKeysToMs(KeySearchCriteria searchCriteria)
        {
            return keyRepository.SearchKeys(
                GetBoundKeyToMsSearchCriteria(searchCriteria));
        }

        public List<KeyInfo> SearchOhrKeysToMs(KeySearchCriteria searchCriteria)
        {
            return keyRepository.SearchKeys(
                GetOhrKeyToMsSearchCriteria(searchCriteria));
        }

        public List<KeyInfo> SearchBoundKeysToMs(List<KeyGroup> keyGroups)
        {
            return keyRepository.SearchKeys(keyGroups);
        }

        public List<KeyGroup> SearchBoundKeyGroupsToMs(KeySearchCriteria searchCriteria)
        {
            return keyRepository.SearchKeyGroups(
                GetBoundKeyToMsSearchCriteria(searchCriteria));
        }

        private KeySearchCriteria[] GetBoundKeyToReportSearchCriteria(KeySearchCriteria searchCriteria)
        {
            var myCriteria = ConvertSearchCriteria(searchCriteria);
            var headQuarterCriteria = ConvertSearchCriteria(searchCriteria);

            myCriteria.HasHardwareHash = true;
            myCriteria.KeyType = KeyType.Standard;
            myCriteria.KeyState = KeyState.Bound;
            myCriteria.HqId = CurrentHeadQuarterId;
            if (Constants.InstallType == InstallType.Tpi)
            {
                if (!CurrentHeadQuarter.IsCentralizedMode)
                    myCriteria.IsInProgress = false;
            }
            else if (Constants.InstallType == InstallType.Oem)
                myCriteria.IsInProgress = false;
            return new KeySearchCriteria[] { myCriteria };
        }

        private KeySearchCriteria[] GetBoundKeyToMsSearchCriteria(KeySearchCriteria searchCriteria)
        {
            var myCriteria = ConvertSearchCriteria(searchCriteria);
            myCriteria.HasHardwareHash = true;
            myCriteria.KeyType = KeyType.Standard;
            myCriteria.KeyState = KeyState.Bound;
            myCriteria.HqId = CurrentHeadQuarterId;
            myCriteria.IsInProgress = false;

            return new KeySearchCriteria[] { myCriteria };
        }


        private KeySearchCriteria[] GetOhrKeyToMsSearchCriteria(KeySearchCriteria searchCriteria)
        {
            var myCriteria = ConvertSearchCriteria(searchCriteria);
            myCriteria.KeyType = KeyType.Standard;
            myCriteria.KeyState = KeyState.ActivationEnabled;
            myCriteria.HqId = CurrentHeadQuarterId;
            myCriteria.IsInProgress = false;
            //if (searchCriteria.KeyStateIds == null || searchCriteria.KeyStateIds.Count <= 0)
            //{
            //    myCriteria.KeyStates = new List<KeyState> { KeyState.ActivationEnabled };
            //}
            //else
            //{
            //    myCriteria.KeyStates = searchCriteria.KeyStateIds.Select(s => (KeyState)s).ToList();
            //}

            return new KeySearchCriteria[] { myCriteria };
        }

    }
}
