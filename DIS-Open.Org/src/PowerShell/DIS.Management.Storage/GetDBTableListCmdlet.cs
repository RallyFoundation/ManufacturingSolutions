using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Management.Automation;

namespace DIS.Management.Storage
{
    [Cmdlet(VerbsCommon.Get, "DBTableList")]
    public class GetDBTableListCmdlet : Cmdlet
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

            string sqlCmdText = "select TABLE_NAME from INFORMATION_SCHEMA.TABLES";

            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = sqlCmdText;
                command.CommandType = CommandType.Text;

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                SqlDataReader reader = command.ExecuteReader();

                string tableName = "";

                while (reader.Read())
                {
                    tableName = reader.GetString(0);

                    this.WriteObject(tableName);
                }

                reader.Close();
            }
        }
    }
}
