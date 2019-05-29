using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace PowerShellDataProcessing
{
    [Cmdlet(VerbsCommon.Show, "Report")]
    public class ShowReportCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The URI pointing to the location of the report file.")]
        public string Uri { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            FormReport report = new FormReport();

            report.Navigate(Uri);

            report.ShowDialog();

            this.WriteObject(report);
        }
    }
}
