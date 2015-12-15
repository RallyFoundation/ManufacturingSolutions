using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using DIS.Management.Deployment;
using DIS.Management.Deployment.Model;

namespace MfgSolutionsDeploymentCenter
{
    class Utility
    {
        public static Installation GetCloudInstallationInfo(string DISHome, string DBInstance, string DBLoginName, string DBLoginPassword) 
        {
            Installation installationInfo = new Installation();

            installationInfo.ApplicationRoot = DISHome;
            installationInfo.InstallationType = InstallationType.Cloud;

            installationInfo.Components = new Component[] 
            {
                new Component()
                {
                    Name = "DISConfigurationCloud",

                    DBServerSetting = new DatabaseServerSetting()
                    {
                       IsIncluded = true,
                       AuthenticationMode = DatabaseAuthenticationMode.Mixed,
                       DatabaseName = "DISConfigurationCloud",
                       ServerAddress = DBInstance,
                       ServerLoginName = DBLoginName,
                       ServerPassword = DBLoginPassword
                    } ,

                    AppServerSetting = new ApplicationServerSetting()
                    {
                        IsIncluded = true,
                        ApplicationName = "DISConfigurationCloud",
                        ApplicationPoolIdentityType = ApplicationPoolIdentityType.LocalSystem,
                        HTTPPortNumber = 8818,
                        ApplicationPoolName = "DISConfigurationCloudAppPool",
                        RootDirectory= "DISConfigurationCloud"
                    }
                }
            };

            return installationInfo;
        }

        public static Installation GetTPIInstallationInfo(string DISHome, string CloudServicePoint, string CloudUserName, string CloudPassword, bool IsCentralized, int InternalAPIPortNumber, int ExternalAPIPortNumber, string InternalAPIAppPoolUserName, string InternalAPIAppPoolUserPassword) 
        {
            Installation installationInfo = new Installation();

            installationInfo.ApplicationRoot = DISHome;
            installationInfo.InstallationMode = IsCentralized ? InstallationMode.Centralized : InstallationMode.Decentralized;
            installationInfo.InstallationType = InstallationType.Tpi;

            installationInfo.Components = new Component[] 
            {
                new Component()
                {
                     Name = "KMT",
                     AppSetting = new ApplicationSetting()
                     {
                          InstallationType = InstallationType.Tpi, 
                          IsTPIInCentralizeMode = IsCentralized ? 1 : 0,
                          IsTPIUsingCarbonCopy = IsCentralized ? 1 : 0,
                          ConfigurationCloudServerAddress = CloudServicePoint,
                          ConfigurationCloudUserName = CloudUserName,
                          ConfigurationCloudPassword = CloudPassword,
                          CloudConfigurationCacheStore = "Cloud-Configs.xml",
                          CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly
                     },
                },
                new Component()
                {
                     Name = "DataPollingService",
                     AppSetting = new ApplicationSetting()
                     {
                          InstallationType = InstallationType.Tpi, 
                          ConfigurationCloudServerAddress = CloudServicePoint,
                          ConfigurationCloudUserName = CloudUserName,
                          ConfigurationCloudPassword = CloudPassword,
                          CloudConfigurationCacheStore = "Cloud-Configs.xml",
                          CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly
                     },
                },
                new Component()
                {
                     Name = "InternalAPI",
                     AppServerSetting = new ApplicationServerSetting()
                     {
                         IsIncluded = true,
                         ApplicationName = "InternalWebService",
                         ApplicationPoolName = "InternalWebServiceAppPool",
                         RootDirectory = @"Web Services\Internal Web Service",
                         HTTPPortNumber = InternalAPIPortNumber,
                         ApplicationPoolIdentityType = ApplicationPoolIdentityType.SpecificUser,
                         ApplicationPoolIdentityUserName = InternalAPIAppPoolUserName,
                         ApplicationPoolIdentityPassword = InternalAPIAppPoolUserPassword,
                         IsSecured = true
                     },
                     AppSetting = new ApplicationSetting()
                     {
                          InstallationType = InstallationType.Tpi, 
                          ConfigurationCloudServerAddress = CloudServicePoint,
                          ConfigurationCloudUserName = CloudUserName,
                          ConfigurationCloudPassword = CloudPassword,
                          CloudConfigurationCacheStore = "Cloud-Configs.xml",
                          CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly
                     },
                },
                new Component()
                {
                     Name = "ExternalAPI",
                     AppServerSetting = new ApplicationServerSetting()
                     {
                         IsIncluded = true,
                         ApplicationName = "ExternalWebService",
                         ApplicationPoolName = "ExternalWebServiceAppPool",
                         RootDirectory = @"Web Services\External Web Service",
                         HTTPPortNumber = ExternalAPIPortNumber,
                         ApplicationPoolIdentityType = DIS.Management.Deployment.Model.ApplicationPoolIdentityType.LocalSystem,
                         IsSecured = true
                     },
                     AppSetting = new ApplicationSetting()
                     {
                          InstallationType = InstallationType.Tpi, 
                          ConfigurationCloudServerAddress = CloudServicePoint,
                          ConfigurationCloudUserName = CloudUserName,
                          ConfigurationCloudPassword = CloudPassword,
                          CloudConfigurationCacheStore = "Cloud-Configs.xml",
                          CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly
                     },
                }
            };

            return installationInfo;
        }

        public static Installation GetCKIInstallationInfo(string DISHome, string CloudServicePoint, string CloudUserName, string CloudPassword, int InternalAPIPortNumber, int ExternalAPIPortNumber, string InternalAPIAppPoolUserName, string InternalAPIAppPoolUserPassword) 
        {
            Installation installationInfo = new Installation();

            installationInfo.ApplicationRoot = DISHome;
            installationInfo.InstallationType = InstallationType.Oem;

            installationInfo.Components = new Component[] 
            {
                new Component()
                {
                     Name = "KMT",
                     AppSetting = new ApplicationSetting()
                     {
                          InstallationType = InstallationType.Oem, 
                          ConfigurationCloudServerAddress = CloudServicePoint,
                          ConfigurationCloudUserName = CloudUserName,
                          ConfigurationCloudPassword = CloudPassword,
                          CloudConfigurationCacheStore = "Cloud-Configs.xml",
                          CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly
                     },
                },
                new Component()
                {
                     Name = "DataPollingService",
                     AppSetting = new ApplicationSetting()
                     {
                          InstallationType = InstallationType.Oem, 
                          ConfigurationCloudServerAddress = CloudServicePoint,
                          ConfigurationCloudUserName = CloudUserName,
                          ConfigurationCloudPassword = CloudPassword,
                          CloudConfigurationCacheStore = "Cloud-Configs.xml",
                          CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly
                     },
                },
                new Component()
                {
                     Name = "InternalAPI",
                     AppServerSetting = new ApplicationServerSetting()
                     {
                         IsIncluded = true,
                         ApplicationName = "InternalWebService",
                         ApplicationPoolName = "InternalWebServiceAppPool",
                         RootDirectory = @"Web Services\Internal Web Service",
                         HTTPPortNumber = InternalAPIPortNumber,
                         ApplicationPoolIdentityType = ApplicationPoolIdentityType.SpecificUser,
                         ApplicationPoolIdentityUserName = InternalAPIAppPoolUserName,
                         ApplicationPoolIdentityPassword = InternalAPIAppPoolUserPassword,
                         IsSecured = true
                     },
                     AppSetting = new ApplicationSetting()
                     {
                          InstallationType = InstallationType.Oem, 
                          ConfigurationCloudServerAddress = CloudServicePoint,
                          ConfigurationCloudUserName = CloudUserName,
                          ConfigurationCloudPassword = CloudPassword,
                          CloudConfigurationCacheStore = "Cloud-Configs.xml",
                          CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly
                     },
                },
                new Component()
                {
                     Name = "ExternalAPI",
                     AppServerSetting = new ApplicationServerSetting()
                     {
                         IsIncluded = true,
                         ApplicationName = "ExternalWebService",
                         ApplicationPoolName = "ExternalWebServiceAppPool",
                         RootDirectory = @"Web Services\External Web Service",
                         HTTPPortNumber = ExternalAPIPortNumber,
                         ApplicationPoolIdentityType = DIS.Management.Deployment.Model.ApplicationPoolIdentityType.LocalSystem,
                         IsSecured = true
                     },
                     AppSetting = new ApplicationSetting()
                     {
                          InstallationType = InstallationType.Oem, 
                          ConfigurationCloudServerAddress = CloudServicePoint,
                          ConfigurationCloudUserName = CloudUserName,
                          ConfigurationCloudPassword = CloudPassword,
                          CloudConfigurationCacheStore = "Cloud-Configs.xml",
                          CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly
                     },
                }
            };

            return installationInfo;
        }

        public static Installation GetFFInstallationInfo(string DISHome, string CloudServicePoint, string CloudUserName, string CloudPassword, int InternalAPIPortNumber, int ExternalAPIPortNumber)
        {
            Installation installationInfo = new Installation();

            installationInfo.ApplicationRoot = DISHome;
            installationInfo.InstallationType = InstallationType.FactoryFloor;

            installationInfo.Components = new Component[] 
            {
                new Component()
                {
                     Name = "KMT",
                     AppSetting = new ApplicationSetting()
                     {
                          InstallationType = InstallationType.FactoryFloor, 
                          ConfigurationCloudServerAddress = CloudServicePoint,
                          ConfigurationCloudUserName = CloudUserName,
                          ConfigurationCloudPassword = CloudPassword,
                          CloudConfigurationCacheStore = "Cloud-Configs.xml",
                          CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly
                     },
                },
                new Component()
                {
                     Name = "KeyProviderService",
                     AppSetting = new ApplicationSetting()
                     {
                          InstallationType = InstallationType.FactoryFloor, 
                          ConfigurationCloudServerAddress = CloudServicePoint,
                          ConfigurationCloudUserName = CloudUserName,
                          ConfigurationCloudPassword = CloudPassword,
                          CloudConfigurationCacheStore = "Cloud-Configs.xml",
                          CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly,
                          ServicePortNumber = 8765
                     },
                },
                new Component()
                {
                     Name = "DataPollingService",
                     AppSetting = new ApplicationSetting()
                     {
                          InstallationType = InstallationType.FactoryFloor, 
                          ConfigurationCloudServerAddress = CloudServicePoint,
                          ConfigurationCloudUserName = CloudUserName,
                          ConfigurationCloudPassword = CloudPassword,
                          CloudConfigurationCacheStore = "Cloud-Configs.xml",
                          CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly
                     },
                },
                new Component()
                {
                     Name = "InternalAPI",
                     AppServerSetting = new ApplicationServerSetting()
                     {
                         IsIncluded = true,
                         ApplicationName = "InternalWebService",
                         ApplicationPoolName = "InternalWebServiceAppPool",
                         RootDirectory = @"Web Services\Internal Web Service",
                         HTTPPortNumber = InternalAPIPortNumber,
                         ApplicationPoolIdentityType = ApplicationPoolIdentityType.LocalSystem,
                         IsSecured = true
                     },
                     AppSetting = new ApplicationSetting()
                     {
                          InstallationType = InstallationType.FactoryFloor, 
                          ConfigurationCloudServerAddress = CloudServicePoint,
                          ConfigurationCloudUserName = CloudUserName,
                          ConfigurationCloudPassword = CloudPassword,
                          CloudConfigurationCacheStore = "Cloud-Configs.xml",
                          CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly
                     },
                },
                new Component()
                {
                     Name = "ExternalAPI",
                     AppServerSetting = new ApplicationServerSetting()
                     {
                         IsIncluded = true,
                         ApplicationName = "ExternalWebService",
                         ApplicationPoolName = "ExternalWebServiceAppPool",
                         RootDirectory = @"Web Services\External Web Service",
                         HTTPPortNumber = ExternalAPIPortNumber,
                         ApplicationPoolIdentityType = DIS.Management.Deployment.Model.ApplicationPoolIdentityType.LocalSystem,
                         IsSecured = true
                     },
                     AppSetting = new ApplicationSetting()
                     {
                          InstallationType = InstallationType.FactoryFloor, 
                          ConfigurationCloudServerAddress = CloudServicePoint,
                          ConfigurationCloudUserName = CloudUserName,
                          ConfigurationCloudPassword = CloudPassword,
                          CloudConfigurationCacheStore = "Cloud-Configs.xml",
                          CloudConfigurationCachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly
                     },
                }
            };

            return installationInfo;
        }

        public static string GenerateUnattendFile(Installation InstallationInfo, string FilePath) 
        {
            Type[] types = new Type[]
            {
                typeof(Component),
                typeof(ApplicationServerSetting),
                typeof(DatabaseServerSetting),
                typeof(ApplicationSetting),
                typeof(ApplicationPoolIdentityType),
                typeof(InstallationMode),
                typeof(InstallationType)
            };

            string xml = XmlSerialize(InstallationInfo, types, "utf-8");

            using (FileStream stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.Write(xml);
                }
            }

            return FilePath;
        }

        public static string[] GetRemovableDrives() 
        {
            List<string> drives = null;

            DriveInfo[] driveInfoes = DriveInfo.GetDrives();

            if ((driveInfoes != null) && (driveInfoes.Length > 0))
            {
                drives = new List<string>();

                foreach (var driveInfo in driveInfoes)
                {
                    if ((driveInfo.DriveType == DriveType.Removable) && (driveInfo.IsReady == true))
                    {
                        drives.Add(driveInfo.Name);
                    }
                }

                return drives.ToArray();
            }

            

            return null;
        }
        public static void CopyDirectory(string Source, string Destination)
        {
            String[] Files;

            if (Destination[Destination.Length - 1] != Path.DirectorySeparatorChar)
            {
                Destination += Path.DirectorySeparatorChar;
            }

            if (!Directory.Exists(Destination)) 
            {
                Directory.CreateDirectory(Destination);
            }

            Files = Directory.GetFileSystemEntries(Source);

            foreach (string Element in Files)
            {
                // Sub directories
                if (Directory.Exists(Element))
                {
                    CopyDirectory(Element, Destination + Path.GetFileName(Element));
                }
                // Files in directory
                else
                {
                    File.Copy(Element, Destination + Path.GetFileName(Element), true);
                }
            }
        }

        public static string XmlSerialize(object objectToSerialize, Type[] extraTypes, string outputEncodingName)
        {
            string returnValue = String.Empty;

            XmlSerializer serializer = new XmlSerializer(objectToSerialize.GetType(), extraTypes);

            using (MemoryStream stream = new MemoryStream())
            {

                serializer.Serialize(stream, objectToSerialize);

                //byte[] bytes = stream.GetBuffer();

                //returnValue = Encoding.UTF8.GetString(bytes);

                byte[] bytes = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(bytes, 0, bytes.Length);

                Encoding outputEncoding = String.IsNullOrEmpty(outputEncodingName) ? Encoding.Default : Encoding.GetEncoding(outputEncodingName);

                returnValue = outputEncoding.GetString(bytes);
            }

            return returnValue;
        }

        public static object XmlDeserialize(string xml, Type type, Type[] extraTypes, string inputEncodingName)
        {
            object returnValue = null;

            XmlSerializer serializer = new XmlSerializer(type, extraTypes);

            //MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

            Encoding inputEncoding = String.IsNullOrEmpty(inputEncodingName) ? Encoding.Default : Encoding.GetEncoding(inputEncodingName);

            using (MemoryStream stream = new MemoryStream(inputEncoding.GetBytes(xml)))
            {
                returnValue = serializer.Deserialize(stream);
            }

            return returnValue;
        }
        public static string StartProcess(string AppPath, string AppParams, bool IsCreatingNewWindow, bool IsUsingShellExecute)
        {
            Process process = new Process();

            process.StartInfo.FileName = AppPath;
            process.StartInfo.Arguments = AppParams;
            process.StartInfo.UseShellExecute = IsUsingShellExecute;
            process.StartInfo.RedirectStandardError = !IsUsingShellExecute;
            process.StartInfo.RedirectStandardOutput = !IsUsingShellExecute;
            //process.StartInfo.CreateNoWindow = !IsCreatingNewWindow;

            process.Start();

            process.WaitForExit();

            string result = "";

            if (!IsUsingShellExecute)
            {
                using (process.StandardOutput)
                {
                    result = process.StandardOutput.ReadToEnd();
                }
            }

            return result;
        }
        public static List<object> ExecutePSScriptFile(string PSScriptFilePath, IDictionary<string, object> PSParameters)
        {
            List<object> returnValue = null;

            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();

            Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            runspace.Open();

            RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);

            Pipeline pipeline = runspace.CreatePipeline();

            Command psCommand = new Command(PSScriptFilePath);

            if ((PSParameters != null) && (PSParameters.Count > 0))
            {
                foreach (string parameterName in PSParameters.Keys)
                {
                    psCommand.Parameters.Add(new CommandParameter(parameterName, PSParameters[parameterName]));
                }
            }

            pipeline.Commands.Add(psCommand);

            // Execute PS script file
            Collection<PSObject> results = pipeline.Invoke();

            if ((results != null) && (results.Count > 0))
            {
                returnValue = new List<object>();

                foreach (var result in results)
                {
                    returnValue.Add(result.BaseObject);
                }
            }

            return returnValue;
        }

        public static void ExecutePSScriptFileAsync(string PSScriptFilePath, IDictionary<string, object> PSParameters, Func<List<object>, Type> Callback)
        {
            List<object> callbackInput = null;

            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();

            Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            runspace.Open();

            RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);

            Pipeline pipeline = runspace.CreatePipeline();

            Command psCommand = new Command(PSScriptFilePath);

            if ((PSParameters != null) && (PSParameters.Count > 0))
            {
                foreach (string parameterName in PSParameters.Keys)
                {
                    psCommand.Parameters.Add(new CommandParameter(parameterName, PSParameters[parameterName]));
                }
            }

            pipeline.Commands.Add(psCommand);

            pipeline.InvokeAsync();

            while (!pipeline.Output.EndOfPipeline)
            {
                Collection<PSObject> results = pipeline.Output.ReadToEnd();

                if ((results != null) && (results.Count > 0))
                {
                    callbackInput = new List<object>();

                    foreach (var result in results)
                    {
                        callbackInput.Add(result.BaseObject);
                    }
                }

                if (Callback != null)
                {
                    Callback.Invoke(callbackInput);
                }
            }
        }
    }
}
