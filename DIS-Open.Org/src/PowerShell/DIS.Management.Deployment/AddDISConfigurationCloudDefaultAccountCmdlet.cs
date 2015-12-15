using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.Data.SqlClient;

namespace DIS.Management.Deployment
{
    [Cmdlet(VerbsCommon.Add, "DISConfigurationCloudDefaultAccount")]
    public class AddDISConfigurationCloudDefaultAccountCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The connection string to the database.")]
        public string DBConnectionString { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            int result = -9;

            using (SqlConnection connection = new SqlConnection(this.DBConnectionString))
            {
                SqlCommand command = new SqlCommand() 
                {
                    CommandText = "dbo.aspnet_Membership_CreateUser",
                    CommandType= System.Data.CommandType.StoredProcedure,
                    Connection = connection
                };

                command.Parameters.AddRange(new SqlParameter[] 
                {
                    new SqlParameter("@ApplicationName", System.Data.DbType.String)
                    {
                         Direction = System.Data.ParameterDirection.Input,
                         Value = "DISConfigurationCloud"
                    },

                    new SqlParameter("@UserName", System.Data.DbType.String)
                    {
                         Direction = System.Data.ParameterDirection.Input,
                         Value = "DIS"
                    },

                    new SqlParameter("@Password", System.Data.DbType.String)
                    {
                         Direction = System.Data.ParameterDirection.Input,
                         Value = "l7oqz9/3Z4DTP0R52UeU5569XKI="
                    },

                    new SqlParameter("@PasswordSalt", System.Data.DbType.String)
                    {
                         Direction = System.Data.ParameterDirection.Input,
                         Value = "uo3SR95UqgOnTQSLuFDfWg=="
                    },

                    new SqlParameter("@PasswordFormat", System.Data.DbType.Int32)
                    {
                         Direction = System.Data.ParameterDirection.Input,
                         Value = 1
                    },

                    new SqlParameter("@PasswordQuestion", System.Data.DbType.String)
                    {
                         Direction = System.Data.ParameterDirection.Input,
                         Value = "Where were you born?"
                    },

                    new SqlParameter("@PasswordAnswer", System.Data.DbType.String)
                    {
                         Direction = System.Data.ParameterDirection.Input,
                         Value = "Microsoft"
                    },

                    new SqlParameter("@Email", System.Data.DbType.String)
                    {
                         Direction = System.Data.ParameterDirection.Input,
                         Value = "DIS@Microsoft.com"
                    },

                    new SqlParameter("@IsApproved", System.Data.DbType.Int32)
                    {
                         Direction = System.Data.ParameterDirection.Input,
                         Value = 1
                    },

                    new SqlParameter("@CurrentTimeUtc", System.Data.DbType.DateTime)
                    {
                         Direction = System.Data.ParameterDirection.Input,
                         Value = DateTime.Now
                    },

                    new SqlParameter("@CreateDate", System.Data.DbType.DateTime)
                    {
                         Direction = System.Data.ParameterDirection.Input,
                         Value = DateTime.Now
                    },

                    new SqlParameter("@UserId", System.Data.DbType.Guid)
                    {
                         Direction = System.Data.ParameterDirection.Output,
                         Value = Guid.Empty
                    }
                });

                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                result = command.ExecuteNonQuery();

                this.WriteObject(command.Parameters["@UserId"].Value);
            }

            this.WriteObject(result);
        }
    }
}
