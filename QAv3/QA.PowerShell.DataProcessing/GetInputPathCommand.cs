using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using QA.Utility;

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

        [Parameter(Position = 3, Mandatory = false, HelpMessage = "The option to specify if the dialog result should be Abort on closing.")]
        public bool AbortOnCancel { get; set; }

        [Parameter(Position = 4, Mandatory = false, HelpMessage = "The option to specify if the path specified should be transformed to short path.")]
        public bool UseShortPath { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            FormGetInputPath formInputPath = new FormGetInputPath(Title, Message, ErrorMessage);

            formInputPath.AbortOnCancel = AbortOnCancel;

            System.Windows.Forms.DialogResult dialogResult = formInputPath.ShowDialog();

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            { 
                string inputPath = formInputPath.InputPath;

                if (UseShortPath)
                {
                    //inputPath = CommonUtility.GetShortPath(inputPath);
                }

                this.WriteObject(inputPath);
            }
            else if(dialogResult == System.Windows.Forms.DialogResult.Abort)
            {
                this.WriteObject("Cancel_Abort");
            }
            else
            {
                this.WriteObject(String.Empty);
            }
        }
    }
}
