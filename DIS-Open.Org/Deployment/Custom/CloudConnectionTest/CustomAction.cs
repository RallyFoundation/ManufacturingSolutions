using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using DISConfigurationCloud.Client;

namespace DIS.Deployment.Custom.CloudConnectionTest
{
    public static class CustomActions
    {
        private const string DISConfigurationCloudServicePoint = "CLOUDSERVICEPOINT";
        private const string DISConfigurationCloudUsername = "CLOUDUSERNAME";
        private const string DISConfigurationCloudPassword = "CLOUDPASSWORD";

        [CustomAction]
        public static ActionResult TestCloudConnection(Session session) 
        {
            IManager mananger = new Manager(false, null);

            string servicePoint = session[DISConfigurationCloudServicePoint];
            string username = session[DISConfigurationCloudUsername];
            string password = session[DISConfigurationCloudPassword];

            DISConfigurationCloud.Client.ModuleConfiguration.ServicePoint = servicePoint;

            DISConfigurationCloud.Client.ModuleConfiguration.AuthorizationHeaderValue = string.Format("{0}:{1}", username, password);

            string result = "";

            try
            {
                result = mananger.Test();

                if (result.ToLower() == "hello!")
                {
                    return ActionResult.Success;
                }
            }
            catch (Exception ex)
            {
                return ActionResult.Failure;
            }

            return ActionResult.Failure;
        }
    }
}
