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

namespace DIS.Business.Library
{
    public partial class LocalKeyManager : ILocalKeyManager
    {
        /// <summary>
        /// search keys to revert 
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public List<KeyInfo> SearchKeysToRevert(KeySearchCriteria searchCriteria)
        {
            if (searchCriteria.KeyType == null)
                throw new NotSupportedException();

            searchCriteria.HqId = CurrentHeadQuarterId;
            searchCriteria.IsInProgress = false;
            if (searchCriteria.KeyType == KeyType.All)
            {
                if (searchCriteria.KeyStateIds != null)
                    throw new DisException("SearchKey_InvalidKeyType");
                else
                {
                    var searchCriteriaStandard = ConvertSearchCriteria(searchCriteria);
                    var searchCriteriaMBR = ConvertSearchCriteria(searchCriteria);
                    searchCriteriaStandard.KeyType = KeyType.Standard;
                    searchCriteriaStandard.KeyStates = new List<KeyState> { KeyState.Consumed, KeyState.Bound };
                    searchCriteriaMBR.KeyType = KeyType.MBR;
                    searchCriteriaMBR.KeyStates = new List<KeyState> { KeyState.ActivationEnabled };
                    return keyRepository.SearchKeys(new KeySearchCriteria[] { searchCriteriaStandard, searchCriteriaMBR });
                }
            }
            else
            {
                switch (searchCriteria.KeyType)
                {
                    case KeyType.Standard:
                        if (searchCriteria.KeyStateIds == null || searchCriteria.KeyStateIds.Count <= 0)
                            searchCriteria.KeyStates = new List<KeyState> { KeyState.Consumed, KeyState.Bound };
                        break;
                    case KeyType.MBR:
                        if (searchCriteria.KeyStateIds == null || searchCriteria.KeyStateIds.Count <= 0)
                            searchCriteria.KeyStates = new List<KeyState> { KeyState.ActivationEnabled };
                        break;
                    default:
                        break;
                }
                return keyRepository.SearchKeys(searchCriteria);
            }
        }

        public List<KeyOperationResult> RevertKeys(List<KeyInfo> keys, string operateMsg, string operater)
        {
            if (keys == null || keys.Count == 0)
                throw new ApplicationException("No keys to revert!");

            List<KeyOperationResult> results = new List<KeyOperationResult>();
            List<KeyInfo> keysInDb = GetKeysInDb(keys);
            foreach (KeyInfo key in keys)
            {
                KeyInfo keyInDb = GetKey(key.KeyId, keysInDb);
                KeyErrorType errorType = ValidateRevertKey(keyInDb);
                results.Add(new KeyOperationResult()
                {
                    Failed = errorType != KeyErrorType.None,
                    Key = key,
                    KeyInDb = keyInDb,
                    FailedType = errorType
                });
            }
            List<KeyInfo> keysToUpdate = results.Where(r => !r.Failed).Select(r => r.KeyInDb).ToList();
            keyRepository.UpdateKeys(keysToUpdate);

            List<KeyInfo> boundKeys = keys.Where(k => !string.IsNullOrEmpty(k.HardwareHash)).ToList();    
            if (boundKeys.Any())
            {
                miscRepository.InsertKeyOperationHistories(boundKeys, KeyState.Fulfilled, operater, operateMsg);
            }
            return results;
        }

        private KeyErrorType ValidateRevertKey(KeyInfo key)
        {
            if (key == null)
                return KeyErrorType.NotFound;
            else if (key.KeyInfoEx.IsInProgress)
                return KeyErrorType.Invalid;
            else if (!ValidateKeyState(
                () =>
                {
                    key.FactoryFloorRevertKey();
                    key.HardwareHash = string.Empty;
                    key.OemOptionalInfo = new OemOptionalInfo();
                    key.TrackingInfo = string.Empty;
                }))
                return KeyErrorType.StateInvalid;
            else
                return KeyErrorType.None;
        }

    }
}
