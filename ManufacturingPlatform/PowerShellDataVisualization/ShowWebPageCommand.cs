using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace PowerShellDataVisualization
{
    [Cmdlet(VerbsCommon.Show, "WebPage")]
    public class ShowWebPageCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The URL pointing to the location of the web page.")]
        public string Url { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The work directory of the current application.")]
        public string RootDirectory { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            FormWebView webView = new FormWebView(Url, RootDirectory);

            webView.Navigate(Url);

            webView.ShowDialog();

            this.WriteObject(webView);
        }
    }
}
