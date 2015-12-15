using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;

namespace DISDeploymentCenter
{
    class Utility
    {
        public static List<USBDeviceInfo> GetUSBDevices()
        {
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();

            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                devices.Add(new USBDeviceInfo(
                (string)device.GetPropertyValue("DeviceID"),
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Description")
                ));
            }

            collection.Dispose();
            return devices;
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

    class USBDeviceInfo
    {
        public USBDeviceInfo(string deviceID, string pnpDeviceID, string description)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
        }
        public string DeviceID { get; private set; }
        public string PnpDeviceID { get; private set; }
        public string Description { get; private set; }
    }

}
