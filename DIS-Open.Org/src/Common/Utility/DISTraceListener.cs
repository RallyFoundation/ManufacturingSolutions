using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace DIS.Common.Utility
{
    /// <summary>
    /// 
    /// </summary>
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class DISTraceListener : CustomTraceListener 
    {
        public DISTraceListener()
            : base()
        {

        }

        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }

        public override void Write(string message)
        {
            throw new NotImplementedException();
        }

        public override void TraceData(System.Diagnostics.TraceEventCache eventCache, string source, System.Diagnostics.TraceEventType eventType, int id, object data)
        {
            if (data is DISLogEntry)
            {
                this.Write(data);
            }
            else
            {
                base.TraceData(eventCache, source, eventType, id, data);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        public override void Write(object o)
        {
            if (o is DISLogEntry)
            {
                DISLogEntry logEntry = o as DISLogEntry;

                using (SqlConnection connection = new SqlConnection(logEntry.DbConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = this.Attributes["writeLogStoredProcName"];

                    command.Parameters.AddRange(new SqlParameter[] 
                    {
                        new SqlParameter("@eventID", SqlDbType.Int){ Value = logEntry.EventId, Direction = ParameterDirection.Input},
                        new SqlParameter("@priority", SqlDbType.Int){ Value = logEntry.Priority, Direction = ParameterDirection.Input},
                        new SqlParameter("@severity", SqlDbType.NVarChar){ Value = logEntry.Severity, Direction = ParameterDirection.Input},
                        new SqlParameter("@title", SqlDbType.NVarChar){ Value = logEntry.Title, Direction = ParameterDirection.Input},
                        new SqlParameter("@timestamp", SqlDbType.DateTime){ Value = logEntry.TimeStamp, Direction = ParameterDirection.Input},
                        new SqlParameter("@machineName", SqlDbType.NVarChar){ Value = logEntry.MachineName, Direction = ParameterDirection.Input},
                        new SqlParameter("@AppDomainName", SqlDbType.NVarChar){ Value = logEntry.AppDomainName, Direction = ParameterDirection.Input},
                        new SqlParameter("@ProcessID", SqlDbType.NVarChar){ Value = logEntry.ProcessId, Direction = ParameterDirection.Input},
                        new SqlParameter("@ProcessName", SqlDbType.NVarChar){ Value = logEntry.ProcessName, Direction = ParameterDirection.Input},
                        new SqlParameter("@ThreadName", SqlDbType.NVarChar){ Value = logEntry.ManagedThreadName, Direction = ParameterDirection.Input},
                        new SqlParameter("@Win32ThreadId", SqlDbType.NVarChar){ Value = logEntry.Win32ThreadId, Direction = ParameterDirection.Input},
                        new SqlParameter("@message", SqlDbType.NVarChar){ Value = logEntry.Message, Direction = ParameterDirection.Input},
                        new SqlParameter("@formattedmessage", SqlDbType.NText){ Value = this.Formatter.Format(logEntry), Direction = ParameterDirection.Input},
                        new SqlParameter("@LogId", SqlDbType.Int){ Value = logEntry.LogID, Direction = ParameterDirection.Output}
                    });

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    if (command.ExecuteNonQuery() > 0) 
                    {
                        logEntry.LogID = (int)(command.Parameters["@LogId"].Value);

                        if (logEntry.Categories != null)
                        {
                            List<int> categoryIDs = new List<int>();

                            //command.CommandText = "select top 1 CategoryID from Category where CategoryName = @CategoryName";
                            //command.CommandType = CommandType.Text;
                            //command.Parameters.Clear();
                            //command.Parameters.Add(new SqlParameter("@CategoryName", SqlDbType.NVarChar));

                            //SqlDataReader reader = null;

                            foreach (string categoryName in logEntry.Categories)
                            {
                                //command.Parameters[0].Value = categoryName;

                                //reader = command.ExecuteReader();

                                //while (reader.Read())
                                //{
                                //    categoryIDs.Add((int)(reader.GetValue(0)));
                                //}

                                //reader.Close();

                                categoryIDs.Add((int)(Enum.Parse(typeof(DISLogCategory), categoryName, true)));
                            }

                            if (categoryIDs.Count > 0)
                            {
                                command.CommandText = "InsertCategoryLog";
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Clear();
                                command.Parameters.AddRange(new SqlParameter[]
                                {
                                    new SqlParameter("@LogID", SqlDbType.Int){ Value = logEntry.LogID},
                                    new SqlParameter("@CategoryID", SqlDbType.Int)
                                });

                                foreach (int categoryID in categoryIDs)
                                {
                                    command.Parameters[1].Value = categoryID;

                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                base.Write(o);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DISLogEntry : LogEntry
    {
        public int LogID { get; set; }

        public string DbConnectionString { get; set; }
    }

    public enum DISLogCategory 
    {
        System = 1,
        Operation = 2
    }
}
