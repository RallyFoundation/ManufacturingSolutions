using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace PowerShellDataProcessing
{
    [Cmdlet(VerbsCommon.Get, "InputPath")]
    public class GetInputPathCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = false, HelpMessage = "The title text to be displayed in the title bar of the dialog.")]
        public string Title { get; set; }

        [Parameter(Position = 1, Mandatory = false, HelpMessage = "The message text to be displayed in the message area of the dialog.")]
        public string Message { get; set; }

        [Parameter(Position = 2, Mandatory = false, HelpMessage = "The message text to be displayed when error encountered.")]
        public string ErrorMessage { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            FormGetInputPath formInputPath = new FormGetInputPath(Title, Message, ErrorMessage);

            if (formInputPath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { 
                string inputPath = formInputPath.InputPath;

                this.WriteObject(inputPath);
            }
        }
    }
}
