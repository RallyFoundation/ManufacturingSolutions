using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DataIntegrator.Helpers.Snapshot;
using DataIntegrator.Helpers.Tracing;

namespace DataIntegrator.Helpers.RDBMS
{
    public class RDBMSHelper
    {
        public RDBMSHelper() 
        {
        }

        public RDBMSHelper(bool enableTracing, string traceSourceName) 
        {
            this.EnableTracing = enableTracing;
            this.TraceSourceName = traceSourceName;
        }

        public bool EnableTracing { get; set; }

        public string TraceSourceName { get; set; }

        public object Query(string providerName, string connectionString, string commandText, string commandType, OperationMethod queryMode)
        {
            object returnValue = null;

            DbConnection connection = this.getDbConnection(providerName);

            connection.ConnectionString = connectionString;

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] {connection, commandText, commandType, queryMode}, this.TraceSourceName);
            }

            if ((connection != null) && (!String.IsNullOrEmpty(commandText)))
            {
                //if (queryMode == OperationMethod.Retrieve)
                //{
                //    DbDataAdapter dataAdapter = this.getDbDataAdapter(providerName);

                //    returnValue = this.getDataSetXml(dataAdapter, connection, commandText, null, this.getDbCommandType(commandType));
                //}
                //else
                {
                    returnValue = this.beginQuery(connection, commandText, commandType, queryMode);
                }
            }

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { connection, commandText, commandType, queryMode, returnValue}, this.TraceSourceName);
            }

            return returnValue;
        }

        public object GetData(string providerName, string connectionString, string dataTableName, string commandText, string commandType, string outputEncodingName) 
        {
            object returnValue = null;

            DbConnection connection = this.getDbConnection(providerName);

            connection.ConnectionString = connectionString;

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] {providerName, connectionString, commandText, commandType, outputEncodingName }, this.TraceSourceName);
            }

            DbDataAdapter dataAdapter = this.getDbDataAdapter(providerName);

            Encoding outputEncoding = String.IsNullOrEmpty(outputEncodingName) ? Encoding.Default : Encoding.GetEncoding(outputEncodingName);

            returnValue = this.getDataSetXml(dataAdapter, connection, dataTableName, commandText, null, this.getDbCommandType(commandType), outputEncoding);

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { returnValue, outputEncoding.BodyName }, this.TraceSourceName);
            }

            return returnValue;
        }

        public object Update(string providerName, string connectionString, string dataTableName, string commandText, string commandType, OperationMethod queryMode, string olderSnapshotXml, string newerSnapshotXml, string snapshotEncodingName, Dictionary<string, string[]> primaryKeys, IDictionary<string, IDictionary<string, string>> foreignKeys) 
        {
            Result result = new Result();

            DbConnection connection = this.getDbConnection(providerName);

            connection.ConnectionString = connectionString;

            DbDataAdapter adapter = this.getDbDataAdapter(providerName);

            DbCommand selectCommand = connection.CreateCommand();
            selectCommand.CommandText = commandText;
            selectCommand.CommandType = this.getDbCommandType(commandType);

            adapter.SelectCommand = selectCommand;

            DbCommandBuilder commandBuilder = this.getDbCommandBuilder(providerName);

            commandBuilder.DataAdapter = adapter;

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            Encoding snapshotEncoding = String.IsNullOrEmpty(snapshotEncodingName) ? Encoding.Default : Encoding.GetEncoding(snapshotEncodingName);

            DataSet dataSetOlderSnapshot = new DataSet();

            if (!String.IsNullOrEmpty(olderSnapshotXml))
            {
                dataSetOlderSnapshot.ReadXml(new System.IO.MemoryStream(snapshotEncoding.GetBytes(olderSnapshotXml)), XmlReadMode.ReadSchema);
            }

            dataSetOlderSnapshot.AcceptChanges();

            DataSet dataSetNewerSnapshot = new DataSet();

            if (!String.IsNullOrEmpty(newerSnapshotXml))
            {
                dataSetNewerSnapshot.ReadXml(new System.IO.MemoryStream(snapshotEncoding.GetBytes(newerSnapshotXml)), XmlReadMode.ReadSchema);
            }

            dataSetNewerSnapshot.AcceptChanges();

            DataSet dataSetCurrent = new DataSet(); //dataSetNewerSnapshot.Clone();

            adapter.Fill(dataSetCurrent);

            dataSetCurrent.Tables[0].TableName = dataTableName;

            dataSetCurrent.AcceptChanges();

            //Comparison machinism to be implemented below:
            //

            IList<IDictionary<string, object>> snapshotNewer, snapshotOlder;
            IList<IDictionary<string, object>> added, deleted, updated;

            //string dataTableName;

            List<string> primaryKeyNames;

            SnapshotHelper snapshotHelper;

            //DataRow row;

            for (int i = 0; i < dataSetNewerSnapshot.Tables.Count; i++)
            {
                //dataTableName = dataSetNewerSnapshot.Tables[i].TableName;

                if (dataSetNewerSnapshot.Tables[i].TableName.ToLower() == dataTableName.ToLower()) 
                {
                    primaryKeyNames = new List<string>();

                    for (int j = 0; j < dataSetNewerSnapshot.Tables[i].PrimaryKey.Length; j++)
                    {
                        primaryKeyNames.Add(dataSetNewerSnapshot.Tables[i].PrimaryKey[j].ColumnName);
                    }

                    if ((primaryKeys != null) && (primaryKeys.ContainsKey(dataTableName)))
                    {
                        primaryKeyNames = new List<string>(primaryKeys[dataTableName]);
                    }

                    snapshotNewer = Utility.ConvertDataTableToList(dataSetNewerSnapshot.Tables[dataTableName]);

                    snapshotOlder = Utility.ConvertDataTableToList(dataSetOlderSnapshot.Tables[dataTableName]);

                    snapshotHelper = new Snapshot.SnapshotHelper();

                    snapshotHelper.UniqueIdentifierNames = primaryKeyNames.ToArray();

                    snapshotHelper.SetCurrent(snapshotNewer);

                    snapshotHelper.SetLast(snapshotOlder);

                    if (snapshotHelper.Compare())
                    {
                        added = snapshotHelper.GetNew();
                        deleted = snapshotHelper.GetDeleted();
                        updated = snapshotHelper.GetUpdated();

                        if ((added != null) && (added.Count > 0))
                        {
                            foreach (IDictionary<string, object> item in added)
                            {
                                DataRow row = dataSetCurrent.Tables[dataTableName].NewRow();

                                foreach (string key in item.Keys)
                                {
                                    row[key] = item[key];
                                }

                                dataSetCurrent.Tables[dataTableName].Rows.Add(row);

                                //row.SetAdded();
                            }

                            //commandBuilder.GetInsertCommand();

                            result.CreationCount = adapter.Update(dataSetCurrent.Tables[dataTableName]);
                        }

                        if ((updated != null) && (updated.Count > 0))
                        {
                            //List<DataRow> rows = new List<DataRow>();

                            foreach (IDictionary<string, object> item in updated)
                            {
                                //DataRow row = dataSetCurrent.Tables[dataTableName].NewRow();

                                //dataSetCurrent.Tables[dataTableName].Rows.Add(row);

                                //DataRow row = this.getFilteredDataRow(item, primaryKeyNames.ToArray(), dataSetOlderSnapshot.Tables[dataTableName]);

                                DataRow row = this.getFilteredDataRow(item, primaryKeyNames.ToArray(), dataSetCurrent.Tables[dataTableName]);

                                //dataSetCurrent.Tables[dataTableName].ImportRow(row);

                                row.AcceptChanges();

                                //row.BeginEdit();

                                foreach (string key in item.Keys)
                                {
                                    if (!primaryKeyNames.Contains(key))
                                    {
                                        row[key] = item[key];
                                    }
                                }

                                //row.EndEdit();

                                //row.SetModified();

                                //rows.Add(row);
                            }

                            //commandBuilder.GetUpdateCommand();

                            result.ModificationCount = adapter.Update(dataSetCurrent.Tables[dataTableName]);

                            //result.ModificationCount = adapter.Update(rows.ToArray());
                        }

                        if ((deleted != null) && (deleted.Count > 0))
                        {
                            //List<DataRow> rows = new List<DataRow>();

                            //Detecting foreign key references:
                            dataSetCurrent.Tables[dataTableName].RowDeleting += (s, e) =>
                            {
                                if (e.Row != null)
                                {
                                    IDictionary<string, string> referencedForeignKeys = foreignKeys[dataTableName];

                                    foreach (string key in referencedForeignKeys.Keys)
                                    {
                                        //this.beginCascadedDeletion(connection, dataTableName, referencedForeignKeys[key], e.Row[referencedForeignKeys[key]], referencedForeignKeys);
                                        this.beginCascadedDeletion(connection, key.Substring(0, key.IndexOf(".")), key.Substring(key.IndexOf(".") + 1), e.Row[referencedForeignKeys[key]], foreignKeys);
                                    }
                                }
                            };

                            foreach (IDictionary<string, object> item in deleted)
                            {
                                //DataRow row = dataSetCurrent.Tables[dataTableName].NewRow();

                                //dataSetCurrent.Tables[dataTableName].Rows.Add(row);

                                //DataRow row = this.getFilteredDataRow(item, primaryKeyNames.ToArray(), dataSetOlderSnapshot.Tables[dataTableName]);

                                DataRow row = this.getFilteredDataRow(item, primaryKeyNames.ToArray(), dataSetCurrent.Tables[dataTableName]);

                                //dataSetCurrent.Tables[dataTableName].ImportRow(row);

                                row.AcceptChanges();

                                //foreach (string key in item.Keys)
                                //{
                                //    row[key] = item[key];
                                //}

                                //row.AcceptChanges();

                                row.Delete();

                                //rows.Add(row);
                            }

                            //commandBuilder.GetDeleteCommand();

                            result.DeletionCount = adapter.Update(dataSetCurrent.Tables[dataTableName]);

                            //result.DeletionCount = adapter.Update(rows.ToArray());
                        }
                    }
                }
            }

            //int result =  adapter.Update(dataSetCurrent);

            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { connection, adapter, commandText, commandType, queryMode, result }, this.TraceSourceName);
            }

            return result;
        }

        public object BulkCopy(string sourceDataSetXml, string destinationProviderName, string destinationConnectionString, string destinationTableName, string bulkCopyAssemblyName, string bulkCopyTypeName) 
        {
            DataSet dataSet = new DataSet();

            System.IO.StringReader reader = new System.IO.StringReader(sourceDataSetXml);

            dataSet.ReadXml(reader, XmlReadMode.ReadSchema);

            dataSet.AcceptChanges();

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { sourceDataSetXml, destinationProviderName, destinationConnectionString, destinationTableName, bulkCopyAssemblyName, bulkCopyTypeName }, this.TraceSourceName);
            }

            return this.beginBulkCopy(dataSet.Tables[0], destinationProviderName, destinationConnectionString, destinationTableName, bulkCopyAssemblyName, bulkCopyTypeName);
        }

        private DataRow getFilteredDataRow(IDictionary<string, object> item, string[] columnNames, DataTable table) 
        {
            int matchCount = 0;

            for (int j = 0; j < table.Rows.Count; j++)
            {
                if (table.Rows[j].RowState != DataRowState.Deleted)
                {
                    for (int i = 0; i < columnNames.Length; i++)
                    {
                        if (table.Rows[j][columnNames[i]].ToString() == item[columnNames[i]].ToString())
                        {
                            matchCount++;
                        }
                    }

                    if (matchCount == columnNames.Length)
                    {
                        if (this.EnableTracing)
                        {
                            TracingHelper.Trace(new object[] {table.Rows[j].ItemArray, columnNames }, this.TraceSourceName);
                        }

                        return table.Rows[j];
                    }
                }
            }
            

            return null;
        }

        private object beginBulkCopy(DataTable sourceDataTable, string destinationProviderName, string destinationConnectionString, string destinationTableName, string bulkCopyAssemblyName, string bulkCopyTypeName) 
        {
            object returnValue = null;

            //if (enableTracing)
            //{
            //    this.trace(new object[] { connection, destinationTableName, sourceDataTable});
            //}

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { destinationConnectionString, destinationProviderName, destinationTableName, sourceDataTable, bulkCopyAssemblyName, bulkCopyTypeName }, this.TraceSourceName);
            }

            //if ((connection != null) && (!String.IsNullOrEmpty(destinationTableName)))
            //{
            //if (destinationProviderName.ToLower() == "system.data.sqlclient")
            //{
            //    using (System.Data.SqlClient.SqlBulkCopy sqlBulkCopy = new System.Data.SqlClient.SqlBulkCopy(destinationConnectionString))
            //    {
            //        sqlBulkCopy.DestinationTableName = destinationTableName;

            //        foreach (DataColumn colunm in sourceDataTable.Columns)
            //        {
            //            sqlBulkCopy.ColumnMappings.Add(colunm.ColumnName, colunm.ColumnName);
            //        }

            //        sqlBulkCopy.WriteToServer(sourceDataTable);
            //    }
            //}
            //    else if(destinationProviderName.ToLower() == "oracle.dataaccess.client")
            //    {
            //        using (Oracle.DataAccess.Client.OracleBulkCopy oracleBulkCopy = new OracleBulkCopy(destinationConnectionString))
            //        {
            //            oracleBulkCopy.DestinationTableName = destinationTableName;
            //            oracleBulkCopy.WriteToServer(sourceDataTable);
            //        }
            //    }
            //}

            if (sourceDataTable != null)
            {
                returnValue= new Result() { CreationCount = sourceDataTable.Rows.Count };
            }

            using (IDisposable bulkCopyInstance = Utility.EnitObject(bulkCopyAssemblyName, bulkCopyTypeName, new object[] { destinationConnectionString }) as IDisposable)
            {
                bulkCopyInstance.GetType().GetProperty("DestinationTableName").SetValue(bulkCopyInstance, destinationTableName);
                
                object colunmMappings = null;

                foreach (DataColumn colunm in sourceDataTable.Columns)
                {
                   colunmMappings = bulkCopyInstance.GetType().GetProperty("ColumnMappings").GetValue(bulkCopyInstance);

                   colunmMappings.GetType().GetMethod("Add", new Type[]{typeof(string), typeof(string)}).Invoke(colunmMappings, new object[] { colunm.ColumnName, colunm.ColumnName });
                }

                if (this.EnableTracing)
                {
                    TracingHelper.Trace(new object[]{colunmMappings}, this.TraceSourceName);
                }

                bulkCopyInstance.GetType().GetMethod("WriteToServer", new Type[] { typeof(DataTable) }).Invoke(bulkCopyInstance, new object[] { sourceDataTable });
            }

            //if (enableTracing)
            //{
            //    this.trace(new object[] { connection, destinationTableName, returnValue });
            //}

            return returnValue;
        }

        //private void trace(object[] data)
        //{
        //    try
        //    {
        //        System.Diagnostics.TraceSource trace = new System.Diagnostics.TraceSource("DataIntegratorTraceSource");

        //        trace.TraceData(System.Diagnostics.TraceEventType.Information, new Random().Next(), data);

        //        trace.Flush();
        //    }
        //    catch (Exception)
        //    {
        //        //If you want to handle this exception, add your exception handling code here, else you may uncomment the following line to throw this exception out.
        //        throw;
        //    }
        //}

        private System.Data.Common.DbConnection getDbConnection(string providerName)
        {
            System.Data.Common.DbConnection returnValue = null;

            if (!String.IsNullOrEmpty(providerName))
            {
                System.Data.Common.DbProviderFactory factory = System.Data.Common.DbProviderFactories.GetFactory(providerName);

                if (factory != null)
                {
                    returnValue = factory.CreateConnection();
                }
            }

            return returnValue;
        }

        private System.Data.Common.DbDataAdapter getDbDataAdapter(string providerName)
        {
            System.Data.Common.DbDataAdapter returnValue = null;

            if (!String.IsNullOrEmpty(providerName))
            {
                System.Data.Common.DbProviderFactory factory = System.Data.Common.DbProviderFactories.GetFactory(providerName);

                if (factory != null)
                {
                    returnValue = factory.CreateDataAdapter();
                }
            }

            return returnValue;
        }

        private System.Data.Common.DbCommandBuilder getDbCommandBuilder(string providerName)
        {
            System.Data.Common.DbCommandBuilder returnValue = null;

            if (!String.IsNullOrEmpty(providerName))
            {
                System.Data.Common.DbProviderFactory factory = System.Data.Common.DbProviderFactories.GetFactory(providerName);

                if (factory != null)
                {
                    returnValue = factory.CreateCommandBuilder();
                }
            }

            return returnValue;
        }

        private object beginQuery(DbConnection connection, string commandText, string commandType, OperationMethod queryMode)
        {
            object returnValue = null;

            System.Data.CommandType dbCommandType = this.getDbCommandType(commandType);

            switch (queryMode)
            {
                case OperationMethod.Delete:
                    {
                        returnValue = this.ExcecuteNonQueryCommand(connection, commandText, null, dbCommandType);
                        break;
                    }
                case OperationMethod.Retrieve:
                    {
                        returnValue = new XmlDocument();
                        (returnValue as XmlDocument).Load(this.ExcecuteXmlReader(connection, commandText, null, dbCommandType));
                        break;
                    }
                case OperationMethod.Create:
                    {
                        returnValue = this.ExcecuteNonQueryCommand(connection, commandText, null, dbCommandType);
                        break;
                    }
                case OperationMethod.Modify:
                    {
                        returnValue = this.ExcecuteNonQueryCommand(connection, commandText, null, dbCommandType);
                        break;
                    }
                default:
                    {
                        returnValue = new XmlDocument();
                        (returnValue as XmlDocument).Load(this.ExcecuteXmlReader(connection, commandText, null, dbCommandType));
                        break;
                    }
            }

            return returnValue;
        }

        private CommandType getDbCommandType(string typeName)
        {
            System.Data.CommandType returnValue = System.Data.CommandType.Text;

            if (String.IsNullOrEmpty(typeName))
            {
                return CommandType.Text;
            }

            switch (typeName.ToLower())
            {
                case ("text"):
                    {
                        returnValue = System.Data.CommandType.Text;
                        break;
                    }
                case ("sp"):
                    {
                        returnValue = System.Data.CommandType.StoredProcedure;
                        break;
                    }
                case ("table"):
                    {
                        returnValue = System.Data.CommandType.TableDirect;
                        break;
                    }
                default:
                    {
                        returnValue = System.Data.CommandType.Text;
                        break;
                    }
            }

            return returnValue;
        }

        private string getDataSetXml(DbDataAdapter dataAdapter, DbConnection connection, string dataTableName, string commandText, DbParameter[] parameters, CommandType commandType, Encoding outputEncoding) 
        {
            string returnValue = null;

            DbCommand selectCommand = connection.CreateCommand();
            selectCommand.CommandText = commandText;
            selectCommand.CommandType = commandType;

            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    selectCommand.Parameters.AddRange(parameters);
                }
            }

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            dataAdapter.SelectCommand = selectCommand;

            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet);

            //System.IO.StringWriter writer = new System.IO.StringWriter();

            //dataSet.WriteXml(writer, XmlWriteMode.WriteSchema);

            //writer.Flush();

            //returnValue = writer.ToString();

            //writer.Close();

            dataSet.Tables[0].TableName = dataTableName;

            dataSet.AcceptChanges();

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                dataSet.WriteXml(stream, XmlWriteMode.WriteSchema);

                stream.Flush();

                returnValue = outputEncoding.GetString(stream.GetBuffer());
            }

            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { connection, outputEncoding, commandText, commandType, parameters, returnValue }, this.TraceSourceName);
            }

            return returnValue;
        }

        private XmlReader ExcecuteXmlReader(DbConnection connection, string commandText, DbParameter[] parameters, CommandType commandType)
        {
            XmlReader returnValue = null;

            DbCommand selectCommand = connection.CreateCommand();
            selectCommand.CommandText = commandText;
            selectCommand.CommandType = commandType;

            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    selectCommand.Parameters.AddRange(parameters);
                }
            }

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            //if (selectCommand is System.Data.SqlClient.SqlCommand)
            //{
            //    returnValue = (selectCommand as System.Data.SqlClient.SqlCommand).ExecuteXmlReader();
            //}
            //else if (selectCommand is Oracle.DataAccess.Client.OracleCommand)
            //{
            //    returnValue = (selectCommand as Oracle.DataAccess.Client.OracleCommand).ExecuteXmlReader();
            //}

            returnValue = selectCommand.GetType().GetMethod("ExecuteXmlReader").Invoke(selectCommand, null) as XmlReader;

            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { connection, commandText, commandType, parameters, returnValue }, this.TraceSourceName);
            }

            return returnValue;
        }

        private object ExcecuteNonQueryCommand(DbConnection connection, string commandText, IDictionary<string, object> inputParameters, CommandType commandType)
        {
            object returnValue = null;
            DbCommand dbCommand = connection.CreateCommand();
            dbCommand.CommandText = commandText;
            dbCommand.CommandType = commandType;

            //if (parameters != null)
            //{
            //    if (parameters.Length > 0)
            //    {
            //        dbCommand.Parameters.AddRange(parameters);
            //    }
            //}

            if (inputParameters != null)
            {
                if (inputParameters.Count > 0)
                {
                    foreach (string paramName in inputParameters.Keys)
                    {
                        DbParameter parameter = dbCommand.CreateParameter();

                        parameter.Direction = ParameterDirection.Input;
                        parameter.ParameterName = paramName;
                        parameter.Value = inputParameters[paramName];

                        dbCommand.Parameters.Add(parameter);
                    }
                }
            }

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            returnValue = dbCommand.ExecuteNonQuery();

            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { connection, commandText, commandType, inputParameters, returnValue }, this.TraceSourceName);
            }

            return returnValue;
        }

        private IList<IDictionary<string, object>> ExcecuteDataReader(DbConnection connection, string commandText, IDictionary<string, object> inputParameters, CommandType commandType)
        {
            IList<IDictionary<string, object>> returnValue = null;
            DbCommand dbCommand = connection.CreateCommand();
            dbCommand.CommandText = commandText;
            dbCommand.CommandType = commandType;

            if (inputParameters != null)
            {
                if (inputParameters.Count > 0)
                {
                    foreach (string paramName in inputParameters.Keys)
                    {
                        DbParameter parameter = dbCommand.CreateParameter();

                        parameter.Direction = ParameterDirection.Input;
                        parameter.ParameterName = paramName;
                        parameter.Value = inputParameters[paramName];

                        dbCommand.Parameters.Add(parameter);
                    }
                }
            }

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            DbDataReader dataReader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

            returnValue = new List<IDictionary<string, object>>();

            while (dataReader.Read())
            {
                IDictionary<string, object> record = new Dictionary<string, object>();

                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    record.Add(dataReader.GetName(i), dataReader.GetValue(i));
                }

                returnValue.Add(record);
            }

            dataReader.Close();

            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }

            if (this.EnableTracing)
            {
                TracingHelper.Trace(new object[] { connection, commandText, commandType, inputParameters, returnValue }, this.TraceSourceName);
            }

            return returnValue;
        }

        private object beginCascadedDeletion(DbConnection connection, string dataTableName, string referencingKeyName, object referencingKeyValue, IDictionary<string, IDictionary<string, string>> foreignKeys) 
        {
            IDictionary<string, object> inputParameters = new Dictionary<string, object>();

            string parameterNamePrefix = connection.GetType().FullName.ToLower().Contains("oracle") ? ":" : "@";

            inputParameters.Add((parameterNamePrefix + referencingKeyName), referencingKeyValue);

            //IList<IDictionary<string, object>> dataList = this.ExcecuteDataReader(connection, dataTableName, inputParameters, CommandType.TableDirect);

            string commandText = String.Format("select * from {0} where {1} = {2}{3}", dataTableName, referencingKeyName, parameterNamePrefix, referencingKeyName);

            IList<IDictionary<string, object>> dataList = this.ExcecuteDataReader(connection, commandText, inputParameters, CommandType.Text);

            if ((dataList != null) && (dataList.Count > 0))
            {
                if (foreignKeys.ContainsKey(dataTableName))
                {
                    IDictionary<string, string> referencedForeignKeys = foreignKeys[dataTableName];

                    foreach (string foreignKey in referencedForeignKeys.Keys)
                    {
                        string tableName = foreignKey.Substring(0, foreignKey.IndexOf("."));

                        string refKeyName = foreignKey.Substring(foreignKey.IndexOf(".") + 1);

                        //this.beginCascadedDeletion(connection, tableName, refKeyName, dataList[0][referencedForeignKeys[foreignKey]], foreignKeys);

                        foreach (IDictionary<string, object> data in dataList)
                        {
                            this.beginCascadedDeletion(connection, tableName, refKeyName, data[referencedForeignKeys[foreignKey]], foreignKeys);
                        }
                    }
                }

                commandText = String.Format("delete from {0} where {1} = {2}{3}", dataTableName, referencingKeyName, parameterNamePrefix, referencingKeyName);

                return this.ExcecuteNonQueryCommand(connection, commandText, inputParameters, CommandType.Text);
            }

            return -9;
        }
    }
}
