using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Platform.DAAS.OData.Core.WdsManagement;
using Platform.DAAS.OData.Core.DomainModel;

namespace Platform.DAAS.OData.WdsManagement
{
    public class WdsManager : IDisposable, IWdsManager
    {
        private PowerShell powerShell = null;
        private Runspace runspace = null;
        private InitialSessionState initialSessionState = null;

        public WdsManager()
        {
            this.initialSessionState = InitialSessionState.Create();
            this.initialSessionState.ImportPSModule(new String[] { "WDS" });

            this.runspace = RunspaceFactory.CreateRunspace(this.initialSessionState);

            this.powerShell = PowerShell.Create();

            this.powerShell.Runspace = this.runspace;

            this.runspace.Open();
        }

        public void Initialize()
        {
            this.initialSessionState = InitialSessionState.Create();
            this.initialSessionState.ImportPSModule(new String[] { "WDS" });

            this.runspace = RunspaceFactory.CreateRunspace(this.initialSessionState);

            this.powerShell = PowerShell.Create();

            this.powerShell.Runspace = this.runspace;

            this.runspace.Open();
        }

        public void Cleanup()
        {
            this.runspace.Close();
        }

        public void Dispose()
        {
            if (this.runspace != null)
            {
                this.runspace.Close();
                this.runspace.Dispose();
            }
        }

        public object AddInstallImageGroup(string GroupName)
        {
            this.powerShell.AddCommand("New-WdsInstallImageGroup", true);
            this.powerShell.AddParameter("Name", GroupName);

            return this.powerShell.Invoke();
        }

        public object GetRawImageContent(string ImageFilePath)
        {
            this.powerShell.AddCommand("Get-WindowsImage", true);
            this.powerShell.AddParameter("ImagePath", ImageFilePath);

            return this.powerShell.Invoke();
        }

        public string GetWdsImageNamespace(OSImage Image)
        {
            string imageNamespace = String.Format("WDS:{0}/{1}/1", Image.ImageGroupName, Image.NewFileName);

            return imageNamespace;
        }

        public object ImportBootImage(OSImage BootImage)
        {
            this.powerShell.AddCommand("Import-WdsBootImage", true);
            //this.powerShell.AddParameter("ImageGroup ", BootImage.ImageGroupName);
            this.powerShell.AddParameter("Path", BootImage.ImageFilePath);
            //this.powerShell.AddParameter("ImageName", BootImage.RawImageNameInFile);
            this.powerShell.AddParameter("NewImageName", BootImage.NewImageName);
            this.powerShell.AddParameter("NewDescription", BootImage.NewDescription);
            this.powerShell.AddParameter("NewFileName", BootImage.NewFileName);

            if (BootImage.EnableMulticastTransmission)
            {
                this.powerShell.AddParameter("Multicast");
                this.powerShell.AddParameter("TransmissionName", BootImage.MulticastTransmissionName);
            }

            return this.powerShell.Invoke();
        }

        public object ImportInstallImage(OSImage InstallImage)
        {
            this.powerShell.AddCommand("Import-WdsInstallImage", true);
            this.powerShell.AddParameter("ImageGroup ", InstallImage.ImageGroupName);
            this.powerShell.AddParameter("Path", InstallImage.ImageFilePath);
            this.powerShell.AddParameter("ImageName", InstallImage.RawImageNameInFile);
            this.powerShell.AddParameter("NewImageName", InstallImage.NewImageName);
            this.powerShell.AddParameter("NewDescription", InstallImage.NewDescription);
            this.powerShell.AddParameter("NewFileName", InstallImage.NewFileName);

            if (InstallImage.EnableMulticastTransmission)
            {
                this.powerShell.AddParameter("Multicast");
                this.powerShell.AddParameter("TransmissionName", InstallImage.MulticastTransmissionName);
            }

            return this.powerShell.Invoke();
        }
    }
}
