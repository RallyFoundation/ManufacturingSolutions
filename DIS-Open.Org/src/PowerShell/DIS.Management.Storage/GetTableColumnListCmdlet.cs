using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Management.Automation;

namespace DIS.Management.Storage
{
    [Cmdlet(VerbsCommon.Get, "TableColumnList")]
    public class GetTableColumnListCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The name of the server / instance hosting the database.")]
        public string DBServerName { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The user name for logging on to the server / instance hosting the database.")]
        public string DBUserName { get; set; }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The passwrod of the user name for logging on to the server / instance hosting the database.")]
        public string DBPassword { get; set; }

        [Parameter(Position = 3, Mandatory = true, HelpMessage = "The the name of the database.")]
        public string DBName { get; set; }

        [Parameter(Position = 4, Mandatory = true, HelpMessage = "The the name of the data table.")]
        public string TableName { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            string dbConnectionString = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", this.DBServerName, this.DBName, this.DBUserName, this.DBPassword);

            string sqlCmdTextSPColumn = "sp_columns";

            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = sqlCmdTextSPColumn;
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(new SqlParameter[] 
                { 
                    new SqlParameter("@table_name", SqlDbType.NVarChar)
                    {
                         Direction = ParameterDirection.Input,
                         Value = this.TableName
                    }
                });

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                SqlDataReader reader = command.ExecuteReader();

                string columnName = "";

                while (reader.Read())
                {
                    columnName = reader.GetString(3);

                    this.WriteObject(columnName);
                }

                reader.Close();
            }
        }
    }
}
