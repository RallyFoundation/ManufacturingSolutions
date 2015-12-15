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
using DIS.Common.Utility;

namespace DIS.Business.Library
{
    public partial class LocalKeyManager : ILocalKeyManager
    {

        public List<KeyInfo> SearchAssignKeys(KeySearchCriteria searchCriteria)
        {
            return keyRepository.SearchKeys(GetAssignkeysSearchCriteria(searchCriteria));
        }

        public List<KeyGroup> SearchAssignKeyGroups(KeySearchCriteria searchCriteria)
        {
            return keyRepository.SearchKeyGroups(GetAssignkeysSearchCriteria(searchCriteria));
        }

        public List<KeyInfo> SearchAssignKeys(List<KeyGroup> keyGroups)
        {
            return keyRepository.SearchKeys(keyGroups);
        }

        #region Assign & unassign keys

        public List<KeyInfo> SearchUnassignKeys(KeySearchCriteria searchCriteria)
        {
            if (searchCriteria.SsId == null)
                throw new ArgumentException("SsId is null.");

            KeySearchCriteria searchCriteriaMe = ConvertSearchCriteria(searchCriteria);
            searchCriteriaMe.KeyState = KeyState.Assigned;
            searchCriteriaMe.IsInProgress = false;
            if (CurrentHeadQuarterId != null)
                searchCriteriaMe.HqId = CurrentHeadQuarterId;
            searchCriteriaMe.PageSize = Constants.BatchLimit;
            return keyRepository.SearchKeys(searchCriteriaMe);
        }

        public List<KeyOperationResult> AssignKeys(List<KeyInfo> keys, int ssId)
        {
            return Execute(keys,
                k => ValidateAssignKey(k),
                k => keyRepository.UpdateKeys(k, false, ssId));
        }

        public List<KeyOperationResult> AssignKeys(List<KeyGroup> groupKeys, int ssId)
        {
            var keysToAssign = keyRepository.SearchKeys(groupKeys);

            if (keysToAssign == null || keysToAssign.Count != groupKeys.Sum(g => g.Quantity))
                throw new DisException("AssignKeysViewModel_KeyStateNotValid");

            return AssignKeys(keysToAssign, ssId);
        }

        public List<KeyOperationResult> UnassignKeys(List<KeyInfo> keys)
        {
            return Execute(keys,
                k => ValidateUnassignKey(k),
                k => keyRepository.UpdateKeys(k, false, null, true));
        }

        #endregion

        #region Private Methods

        private List<KeyOperationResult> Execute(List<KeyInfo> keys,
            Func<KeyInfo, KeyErrorType> validate, Action<List<KeyInfo>> update)
        {
            if (keys == null || keys.Count == 0)
                throw new ApplicationException("No keys to operate!");

            List<KeyOperationResult> results = new List<KeyOperationResult>();
            List<KeyInfo> keysInDb = GetKeysInDb(keys);
            foreach (KeyInfo key in keys)
            {
                KeyInfo keyInDb = GetKey(key.KeyId, keysInDb);
                KeyErrorType errorType = validate(keyInDb);
                results.Add(new KeyOperationResult()
                {
                    Failed = errorType != KeyErrorType.None,
                    Key = key,
                    KeyInDb = keyInDb,
                    FailedType = errorType
                });
            }
            List<KeyInfo> keysToUpdate = results.Where(r => !r.Failed).Select(r => r.KeyInDb).ToList();
            update(keysToUpdate);
            return results;
        }

        private KeyErrorType ValidateAssignKey(KeyInfo key)
        {
            if (key == null)
                return KeyErrorType.NotFound;
            else if (key.KeyInfoEx.IsInProgress)
                return KeyErrorType.StateInvalid;
            else if (key.KeyInfoEx.SsId.HasValue)
                return KeyErrorType.AlreadyAssigned;
            else if (!ValidateKeyState(key.UlsAssigningKey))
                return KeyErrorType.StateInvalid;
            else if (ValidateIfCarbonCopyKey(key))
                return KeyErrorType.Invalid;
            else
                return KeyErrorType.None;
        }

        private bool ValidateIfCarbonCopyKey(KeyInfo key)
        {
            if (Constants.InstallType != InstallType.Oem)
                return false;
            if (key.KeyInfoEx.ShouldCarbonCopy.HasValue)
                return true;
            return false;
        }

        private KeyErrorType ValidateUnassignKey(KeyInfo key)
        {
            if (key == null)
                return KeyErrorType.NotFound;
            else if (key.KeyInfoEx.IsInProgress)
                return KeyErrorType.AlreadyAssigned;
            else if (!ValidateKeyState(key.UlsUnassigningKey))
                return KeyErrorType.StateInvalid;
            else
                return KeyErrorType.None;
        }

        protected bool ValidateKeyState(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, this.keyRepository.GetDBConnectionString());
                return false;
            }
        }

        private KeySearchCriteria[] GetAssignkeysSearchCriteria(KeySearchCriteria searchCriteria)
        {
            KeySearchCriteria searchCriteriaUp = ConvertSearchCriteria(searchCriteria);
            searchCriteriaUp.KeyState = KeyState.Fulfilled;
            searchCriteriaUp.IsInProgress = false;
            searchCriteriaUp.IsAssign = false;
            searchCriteriaUp.SsId = null;
            searchCriteriaUp.HqId = CurrentHeadQuarterId;
            return new KeySearchCriteria[] { searchCriteriaUp };
        }

        #endregion
    }
}
