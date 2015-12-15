using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.Data.SqlClient;

namespace DIS.Management.Deployment
{
    [Cmdlet(VerbsCommon.Clear, "DISConfigurationCloudMembershipDB")]
    public class ClearDISConfigurationCloudMembershipDBCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The connection string to the database.")]
        public string DBConnectionString { get; set; }

        [Parameter(Position = 1, Mandatory = false, HelpMessage = "The name of the application being served.")]
        public string ApplicationName { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            int result = -9;

            using (SqlConnection connection = new SqlConnection(this.DBConnectionString))
            {
                SqlCommand command = new SqlCommand()
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.Text
                };

                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                Guid applicationId = Guid.Empty;

                if (!String.IsNullOrEmpty(this.ApplicationName))
                {
                    command.CommandText = "SELECT ApplicationId FROM aspnet_Applications WHERE ApplicationName = @ApplicationName";
                    command.Parameters.Add(new SqlParameter("@ApplicationName", System.Data.SqlDbType.NVarChar) { Value = this.ApplicationName, Direction = System.Data.ParameterDirection.Input });

                    applicationId = (Guid)(command.ExecuteScalar());

                    this.WriteObject(applicationId);

                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@ApplicationId", System.Data.SqlDbType.UniqueIdentifier) { Value = applicationId, Direction = System.Data.ParameterDirection.Input });
                }

                command.CommandText = String.IsNullOrEmpty(this.ApplicationName) ? "DELETE FROM aspnet_Membership" : "DELETE FROM aspnet_Membership WHERE ApplicationId = @ApplicationId";
                result = command.ExecuteNonQuery();
                this.WriteObject(result);

                command.CommandText = String.IsNullOrEmpty(this.ApplicationName) ? "DELETE FROM aspnet_Users" : "DELETE FROM aspnet_Users WHERE ApplicationId = @ApplicationId";
                result = command.ExecuteNonQuery();
                this.WriteObject(result);

                command.CommandText = String.IsNullOrEmpty(this.ApplicationName) ? "DELETE FROM aspnet_Applications" : "DELETE FROM aspnet_Applications WHERE ApplicationId = @ApplicationId";
                result = command.ExecuteNonQuery();
                this.WriteObject(result);
            }
        }
    }
}
