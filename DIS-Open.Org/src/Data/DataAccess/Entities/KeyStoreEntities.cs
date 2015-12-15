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
using System.Data.Objects;
using System.Data;
using DIS.Data.DataContract;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace DIS.Data.DataAccess
{
    /// <summary>
    /// Partial class implementation of IKeyStoreEntities.
    /// Coding to interface will allow fake context to be passed for unit testing.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "IDisposable is specified by IKeyStoreEntities and the implementation is inherited from ObjectContext")]
    public partial class KeyStoreEntities : ObjectContext
    {

        public const string PitAdmin = "Admin";

        #region Constructors

        public KeyStoreEntities(EntityConnection connection, string containerName)
            : base(connection, containerName)
        {
            this.ContextOptions.LazyLoadingEnabled = false;
        }
    
        #endregion
        /// <summary>
        /// Commit changes to the DB.
        /// </summary>
        /// <returns></returns>
        public new void SaveChanges()
        {
            SaveOrUpdateBase();
            base.SaveChanges();
        }

        /// <summary>
        /// Inserts or Updates the Entites in Context but the data will not be saved.
        /// </summary>
        /// <returns></returns>
        private void SaveOrUpdateBase()
        {
            foreach (ObjectStateEntry entry in ObjectStateManager.GetObjectStateEntries(EntityState.Added))
            {
                CurrentValueRecord currentRecord = entry.CurrentValues;
                for (int i = 0; i < currentRecord.FieldCount; i++)
                {
                    string name = currentRecord.GetName(i);
                    if (name == Constants.CommonFields.CreatedDateField || name == Constants.CommonFields.ModifiedDateField)
                        currentRecord.SetValue(i, DateTime.Now);
                    else if (name == Constants.CommonFields.CreatedByField || name == Constants.CommonFields.ModifiedByField)
                        currentRecord.SetValue(i, PitAdmin);
                    else if (name == Constants.CommonFields.ActionCodeField)
                        currentRecord.SetValue(i, Constants.ActionCode.Inserted);
                }
            }

            foreach (ObjectStateEntry entry in ObjectStateManager.GetObjectStateEntries(EntityState.Modified))
            {
                CurrentValueRecord currentRecord = entry.CurrentValues;
                for (int i = 0; i < currentRecord.FieldCount; i++)
                {
                    string name = currentRecord.GetName(i);
                    if (name == DataContract.Constants.CommonFields.ModifiedDateField)
                        currentRecord.SetValue(i, DateTime.Now);
                    else if (name == DataContract.Constants.CommonFields.ModifiedByField)
                        currentRecord.SetValue(i, PitAdmin);
                    else if (name == DataContract.Constants.CommonFields.ActionCodeField)
                    {
                        if (currentRecord.GetValue(i) != null && currentRecord.GetValue(i).ToString() == Constants.ActionCode.Deleted)
                            currentRecord.SetValue(i, Constants.ActionCode.Deleted);
                        else
                            currentRecord.SetValue(i, Constants.ActionCode.Updated);
                    }
                }
            }
        }
    }
}
