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
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DIS.Data.DataContract
{
    public class KeyDuplicated
    {
        public int KeyDuplicatedId { get; set; }
        public long KeyId { get; set; }
        public string ProductKey { get; set; }
        public bool Handled { get; set; }
        public Nullable<int> OperationId { get; set; }
        public virtual KeyOperationHistory KeyOperationHistory { get; set; }
        public virtual KeyInfo KeyInfo { get; set; }


        private ReuseOperation duplicatedOperation;

        public KeyState CurrentState
        {
            get { return KeyState.NotifiedBound; }
        }

        public KeyState NewState
        {
            get { return ReuseOperation == ReuseOperation.Ignore ? CurrentState : KeyState.Fulfilled; }
        }

        public ReuseOperation ReuseOperation
        {
            get { return duplicatedOperation; }
            set { duplicatedOperation = value; }
        }

        public string[] ReuseOperations
        {
            get { return Enum.GetNames(typeof(ReuseOperation)); }
        }

    }

    public enum ReuseOperation
    {
        Reuse,
        Ignore,
        None,
    }
}

