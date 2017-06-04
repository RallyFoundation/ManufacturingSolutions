using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Platform.DAAS.OData.Core.VamtManagement;

namespace Platform.DAAS.OData.VamtManagement
{
    public class VamtManager : IDisposable, IVamtManager
    {
        private PowerShell powerShell = null;
        private Runspace runspace = null;
        private InitialSessionState initialSessionState = null;

        private string defaultVamtDBConnectionString;

        public VamtManager(string VamtPSModulePath) 
        {
            this.initialSessionState = InitialSessionState.Create();
            this.initialSessionState.ImportPSModule(new String[] { VamtPSModulePath });

            this.runspace = RunspaceFactory.CreateRunspace(this.initialSessionState);

            this.powerShell = PowerShell.Create();

            this.powerShell.Runspace = this.runspace;

            this.runspace.Open();
        }

        public void Initialize(string VamtPSModulePath) 
        {
            this.initialSessionState = InitialSessionState.Create();
            this.initialSessionState.ImportPSModule(new String[] { VamtPSModulePath });

            this.runspace = RunspaceFactory.CreateRunspace(this.initialSessionState);

            this.powerShell = PowerShell.Create();

            this.powerShell.Runspace = this.runspace;

            this.runspace.Open();
        }

        public void Cleanup() 
        {
            this.runspace.Close();
        }

        public void SetDefaultVamtDBConnectionString(string connectionString) 
        {
            this.defaultVamtDBConnectionString = connectionString;
        }

        public void Dispose() 
        {
            if (this.runspace != null)
            {
                this.runspace.Close();
                this.runspace.Dispose();
            }
        }

        public Collection<PSObject> FindVamtManegedMachineByHostName(string hostName, string userName, string password) 
        {
            this.powerShell.AddCommand("Find-VamtManagedMachine", true);
            this.powerShell.AddParameter("QueryType", "Manual");
            this.powerShell.AddParameter("QueryValue", hostName);
            this.powerShell.AddParameter("Username", userName);
            this.powerShell.AddParameter("Password", password);

            if (!String.IsNullOrEmpty(this.defaultVamtDBConnectionString))
            {
                this.powerShell.AddParameter("DbConnectionString", this.defaultVamtDBConnectionString);
            }
            
            return this.powerShell.Invoke();
        }

        public Collection<PSObject> GetVamtConfirmationID(Collection<PSObject> products) 
        {
            this.powerShell.AddCommand("Get-VamtConfirmationId", true);

            this.powerShell.AddParameter("Products", products);

            return this.powerShell.Invoke();
        }

        public Collection<PSObject> InstallVamtConfirmationID(Collection<PSObject> products, string userName, string password) 
        {
            this.powerShell.AddCommand("Install-VamtConfirmationId", true);

            this.powerShell.AddParameter("Products", products);
            this.powerShell.AddParameter("Username", userName);
            this.powerShell.AddParameter("Password", password);

            return this.powerShell.Invoke();
        } 

    }
}
