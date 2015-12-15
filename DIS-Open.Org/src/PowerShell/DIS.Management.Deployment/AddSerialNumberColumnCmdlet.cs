using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Management.Automation;


namespace DIS.Management.Deployment
{
    [Cmdlet(VerbsCommon.Add, "SerialNumberColumn")]
    public class AddSerialNumberColumnCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The name of the server / instance hosting the database.")]
        public string DBServerName { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The user name for logging on to the server / instance hosting the database.")]
        public string DBUserName { get; set; }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The passwrod of the user name for logging on to the server / instance hosting the database.")]
        public string DBPassword { get; set; }

        [Parameter(Position = 3, Mandatory = true, HelpMessage = "The the name of the database.")]
        public string DBName { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            string dbConnectionString = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", this.DBServerName, this.DBName, this.DBUserName, this.DBPassword);

            string sqlCmdTextSPColumn = "sp_columns";

            string sqlCmdTextAlterTable = "ALTER TABLE ProductKeyInfo ADD SerialNumber NVARCHAR(36) NULL";

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
                         Value = "ProductKeyInfo"
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

                   if (columnName.ToLower() == "serialnumber")
                   {
                       break;
                   }
                }

                reader.Close();

                if (columnName.ToLower() == "serialnumber")
                {
                    this.WriteObject("The SerialNumber column already exists in talbe ProductKeyInfo!");
                }
                else
                {
                    command.CommandText = sqlCmdTextAlterTable;
                    command.CommandType = CommandType.Text;
                    command.Parameters.Clear();

                    int result = command.ExecuteNonQuery();

                    this.WriteObject(result);
                }
            }
        }
    }
}
