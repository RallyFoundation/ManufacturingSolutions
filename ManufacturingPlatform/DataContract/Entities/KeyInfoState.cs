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
using System.Diagnostics;
using System.Linq;
using System.Text;
using DIS.Common.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIS.Data.DataContract
{
    public partial class KeyInfo
    {
        [NotMapped]
        private KeyState KeyStateWrapper
        {
            set
            {
                if (KeyState != value)
                {
                    KeyState = value;
                    KeyStateChanged = true;
                }
            }
        }

        [NotMapped]
        public bool KeyStateChanged { get; set; }

        public KeyInfo(KeyState keyState)
            : this()
        {
            KeyState = keyState;
        }

        public void CopyKeyState(KeyInfo key)
        {
            KeyStateWrapper = key.KeyState;
        }

        public void RetrievingKeys()
        {
            KeyStateWrapper = KeyState.Fulfilled;
        }

        public void OemReceivingFulfilledCarbonCopyKey()
        {
            InstallTypeShouldBe(InstallType.Oem);

            KeyStateWrapper = KeyState.Fulfilled;
        }

        public void OemReceivingReportedCarbonCopyKey(bool isEnabled)
        {
            InstallTypeShouldBe(InstallType.Oem);

            KeyStateShouldBe(KeyState.Fulfilled, KeyState.ActivationEnabled);

            if (KeyState == KeyState.Fulfilled)
                KeyStateWrapper = isEnabled ? KeyState.ActivationEnabled : KeyState.ActivationDenied;
        }

        public void OemReceivingReturnReportedCarbonCopyKey()
        {
            InstallTypeShouldBe(InstallType.Oem);

            KeyStateShouldBe(KeyState.Fulfilled, KeyState.ActivationEnabled, KeyState.ActivationDenied);

            KeyStateWrapper = KeyState.Returned;
        }

        public void UlsReceivingFulfilledKeySync()
        {
            InstallTypeShouldBe(InstallType.Uls);
            KeyTypeShouldBe(KeyType.All);

            KeyStateShouldBe(KeyType.All, KeyState.Assigned);

            switch (KeyInfoEx.KeyType)
            {
                case KeyType.Standard:
                case KeyType.MAT:
                    KeyStateWrapper = KeyState.Retrieved;
                    break;
                case KeyType.MBR:
                    KeyStateWrapper = KeyState.ActivationEnabled;
                    break;
            }
        }

        public void UlsReceivingBoundKey()
        {
            InstallTypeShouldBe(InstallType.Uls);
            KeyTypeShouldBe(KeyType.Standard);

            KeyStateShouldBe(KeyType.Standard, KeyState.Retrieved);

            KeyStateWrapper = KeyState.Bound;
        }

        public void UlsExportingFulfilledKey()
        {
            InstallTypeShouldBe(InstallType.Uls);
            KeyTypeShouldBe(KeyType.All);

            KeyStateShouldBe(KeyType.All, KeyState.Fulfilled);

            switch (KeyInfoEx.KeyType)
            {
                case KeyType.Standard:
                case KeyType.MAT:
                    KeyStateWrapper = KeyState.Retrieved;
                    break;
                case KeyType.MBR:
                    KeyStateWrapper = KeyState.ActivationEnabled;
                    break;
            }
        }

        public void UlsReceivingRecallRequest()
        {
            InstallTypeShouldBe(InstallType.Uls);
            KeyTypeShouldBe(KeyType.All);

            KeyStateShouldBe(KeyType.Standard, KeyState.Retrieved);
            KeyStateShouldBe(KeyType.MBR, KeyState.ActivationEnabled);
            KeyStateShouldBe(KeyType.MAT, KeyState.Retrieved);

            KeyStateWrapper = KeyState.Fulfilled;
        }

        public void DlsReportingBoundKeyToUls()
        {
            InstallTypeShouldBe(InstallType.Dls);
            KeyTypeShouldBe(KeyType.Standard);

            KeyStateShouldBe(KeyType.Standard, KeyState.Bound);

            KeyStateWrapper = KeyState.NotifiedBound;
        }

        public void UlsReportingBoundKeyToMs()
        {
            InstallTypeShouldBe(InstallType.Uls);
            KeyTypeShouldBe(KeyType.Standard);

            KeyStateShouldBe(KeyType.Standard, KeyState.Bound);

            KeyStateWrapper = KeyState.ReportedBound;
        }

        public void UlsReceivingCbrAck(bool isEnabled, bool isDuplicated = false)
        {
            InstallTypeShouldBe(InstallType.Uls);
            KeyTypeShouldBe(KeyType.Standard);

            if (isDuplicated)
                KeyStateShouldBe(KeyType.Standard, KeyState.ActivationDenied,KeyState.ActivationEnabled);
            else
                KeyStateShouldBe(KeyType.Standard, KeyState.ReportedBound);

            KeyStateWrapper = isEnabled ? KeyState.ActivationEnabled : KeyState.ActivationDenied;
        }

        public void UlsExportOHRData()
        {
            InstallTypeShouldBe(InstallType.Uls);
            KeyTypeShouldBe(KeyType.Standard);
            KeyStateShouldBe(KeyType.Standard, KeyState.ActivationDenied, KeyState.ActivationEnabled);
        }

        public void FactoryFloorAssembleKey()
        {
            InstallTypeShouldBe(InstallType.FactoryFloor);
            KeyTypeShouldBe(KeyType.All);

            KeyStateShouldBe(KeyType.All, KeyState.Fulfilled);

            switch (KeyInfoEx.KeyType)
            {
                case KeyType.Standard:
                    KeyStateWrapper = KeyState.Consumed;
                    break;
                case KeyType.MBR:
                    KeyStateWrapper = KeyState.ActivationEnabled;
                    break;
                case KeyType.MAT:
                    KeyStateWrapper = KeyState.Fulfilled;
                    break;
            }
        }

        public void FactoryFloorBoundKey(bool isBound)
        {
            InstallTypeShouldBe(InstallType.FactoryFloor);

            if (isBound) //invoked by oa3tool /report
            {
                KeyTypeShouldBe(KeyType.All);
                KeyStateShouldBe(KeyType.Standard, KeyState.Consumed);
                if (KeyInfoEx.KeyType == KeyType.Standard)
                {
                    KeyStateWrapper = KeyState.Bound;
                }
            }
            else //invoked by oa3tool /return
            {
                KeyTypeShouldBe(KeyType.All);
                KeyStateShouldBe(KeyType.Standard, KeyState.Consumed, KeyState.Bound);
                KeyStateShouldBe(KeyType.MBR, KeyState.ActivationEnabled);
                KeyStateShouldBe(KeyType.MAT, KeyState.Fulfilled);
                KeyStateWrapper = KeyState.Fulfilled;
            }


        }

        public void UlsAssigningKey()
        {
            InstallTypeShouldBe(InstallType.Uls);
            KeyTypeShouldBe(KeyType.All);

            KeyStateShouldBe(KeyType.All, KeyState.Fulfilled);

            KeyStateWrapper = KeyState.Assigned;
        }

        public void FactoryFloorRevertKey()
        {
            InstallTypeShouldBe(InstallType.FactoryFloor);

            KeyTypeShouldBe(KeyType.Standard | KeyType.MBR);

            KeyStateShouldBe(KeyType.Standard, KeyState.Bound, KeyState.Consumed);
            KeyStateShouldBe(KeyType.MBR, KeyState.ActivationEnabled);

            KeyStateWrapper = KeyState.Fulfilled;
        }

        public void UlsUnassigningKey()
        {
            InstallTypeShouldBe(InstallType.Uls);
            KeyTypeShouldBe(KeyType.All);

            KeyStateShouldBe(KeyType.All, KeyState.Assigned);

            KeyStateWrapper = KeyState.Fulfilled;
        }

        public void OemReceivingReturnedSync()
        {
            InstallTypeShouldBe(InstallType.Oem);
            KeyStateWrapper = KeyState.Returned;
        }

        public void ULsReturningKey()
        {
            InstallTypeShouldBe(InstallType.Uls);
            KeyTypeShouldBe(KeyType.All);

            KeyStateShouldBe(KeyType.Standard, KeyState.Fulfilled, KeyState.Bound, KeyState.ActivationEnabled, KeyState.ActivationDenied);
            KeyStateShouldBe(KeyType.MBR, KeyState.Fulfilled, KeyState.ActivationEnabled);
            KeyStateShouldBe(KeyType.MAT, KeyState.Fulfilled);

            KeyStateWrapper = KeyState.ReportedReturn;
        }

        public void UlsReceivingReturnAck(ReturnReportKey returnReportKey)
        {
            InstallTypeShouldBe(InstallType.Uls);
            KeyTypeShouldBe(KeyType.All);

            KeyStateShouldBe(KeyType.All, KeyState.ReportedReturn);

            if (returnReportKey.ReturnReasonCode.StartsWith("O") || returnReportKey.ReturnReasonCode.StartsWith("Q"))
                KeyStateWrapper = DataContract.KeyState.Returned;
            else
                KeyStateWrapper = (DataContract.KeyState)returnReportKey.PreProductKeyStateId;
        }

        public void DlsRecieveSync(KeyState keyState)
        {
            InstallTypeShouldBe(InstallType.Dls);
            KeyTypeShouldBe(KeyType.All);

            KeyStateShouldBe(KeyType.Standard, KeyState.NotifiedBound, KeyState.ActivationEnabled, KeyState.ActivationDenied);
            KeyStateShouldBe(KeyType.MBR, KeyState.Fulfilled, KeyState.ActivationEnabled);
            KeyStateShouldBe(KeyType.MAT, KeyState.Fulfilled);

            KeyStateWrapper = keyState;
        }

        public void DlsReceivingReturnSync()
        {
            InstallTypeShouldBe(InstallType.Dls);
            KeyTypeShouldBe(KeyType.Standard | KeyType.MBR);

            KeyStateShouldBe(KeyType.Standard, KeyState.NotifiedBound, KeyState.ActivationEnabled, KeyState.ActivationDenied);
            KeyStateShouldBe(KeyType.MBR, KeyState.ActivationEnabled);

            KeyStateWrapper = KeyState.Returned;
        }

        public void UlsReportingOhrToMs()
        {
            InstallTypeShouldBe(InstallType.Uls);
            KeyTypeShouldBe(KeyType.Standard);

            KeyStateShouldBe(KeyType.Standard, KeyState.ActivationEnabled);

            KeyStateWrapper = KeyState.ActivationEnabledPendingUpdate;
        }

        public void UlsReceivingOhrAck()
        {
            InstallTypeShouldBe(InstallType.Uls);
            KeyTypeShouldBe(KeyType.Standard);

            KeyStateShouldBe(KeyType.Standard, KeyState.ActivationEnabledPendingUpdate);

            KeyStateWrapper = KeyState.ActivationEnabled;
        }

        private void KeyTypeShouldBe(KeyType keyType)
        {
            if ((KeyInfoEx.KeyType & keyType) != KeyInfoEx.KeyType)
                throw new ApplicationException(string.Format("{0} of '{1}' cannot be called for invalid key type {2}",
                                    new StackTrace().GetFrame(2).GetMethod().Name, ProductKey, KeyState));
        }

        private void KeyStateShouldBe(params KeyState[] expectedKeyStates)
        {
            if (!expectedKeyStates.Contains(KeyState))
                throw new ApplicationException(string.Format("{0} of '{1}' cannot be called for invalid key state {2}",
                                    new StackTrace().GetFrame(2).GetMethod().Name, ProductKey, KeyState));
        }

        private void KeyStateShouldBe(KeyType keyType, params KeyState[] expectedKeyStates)
        {
            if ((KeyInfoEx.KeyType & keyType) == KeyInfoEx.KeyType && !expectedKeyStates.Contains(KeyState))
                throw new ApplicationException(string.Format("{0} of '{1}' cannot be called for invalid key state {2}",
                                    new StackTrace().GetFrame(2).GetMethod().Name, ProductKey, KeyState));
        }

        private void InstallTypeShouldBe(InstallType expectedInstallType)
        {
            if ((Constants.InstallType & expectedInstallType) == 0)
                throw new ApplicationException(string.Format("{0} of '{1}' cannot be called in {2}",
                    new StackTrace().GetFrame(2).GetMethod().Name, ProductKey, Constants.InstallType));
        }
    }
}
