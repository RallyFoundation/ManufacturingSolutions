using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using OA3DPKIDSNManager;

namespace PowerShellOA3DPKSNBinder
{
    [Cmdlet(VerbsCommon.Add, "DPKIDSNBinding")]
    public class AddDPKIDSNBindingCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The value of the product key ID.")]
        public long ProductKeyID { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The value of the machine serial number that is associated with the product key ID.")]
        public string SerialNumber { get; set; }

        [Parameter(Position = 2, Mandatory = false, HelpMessage = "The value of the trasaction ID (optional).")]
        public string TransactionID { get; set; }

        [Parameter(Position = 3, Mandatory = true, HelpMessage = "The persistency mode for storing the DPKID-SN pairs.")]
        public PersistencyMode PersistencyMode { get; set; }

        [Parameter(Position = 4, Mandatory = true, ParameterSetName="DataStoreDB", HelpMessage = "The connection string to the database that stores the DPKID-SN pairs.")]
        public string DBConnectionString { get; set; }

        [Parameter(Position = 4, Mandatory = true, ParameterSetName = "DataStoreFile", HelpMessage = "The path to the disk file that stores the DPKID-SN pairs.")]
        public string FilePath { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            OA3DPKIDSNManager.ProductKeySerialBinder binder = new ProductKeySerialBinder();

            binder.PersistencyMode = this.PersistencyMode;

            switch (this.PersistencyMode)
            {
                case PersistencyMode.RDBMSSQLServer:
                    binder.SetDBConnectionString(this.DBConnectionString);
                    break;
                case PersistencyMode.FileSystemXML:
                    binder.SetFilePath(this.FilePath);
                    break;
                case PersistencyMode.FileSystemJSON:
                    binder.SetFilePath(this.FilePath);
                    break;
                default:
                    break;
            }

            string result = String.IsNullOrEmpty(this.TransactionID) ? binder.Bind(this.ProductKeyID, this.SerialNumber) : binder.Bind(this.ProductKeyID, this.SerialNumber, this.TransactionID);

            this.WriteObject(result);
        }
    }
}
