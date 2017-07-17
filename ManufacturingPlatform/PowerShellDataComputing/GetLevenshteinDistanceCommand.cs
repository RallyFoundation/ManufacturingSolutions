using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using F23.StringSimilarity;

namespace PowerShellDataComputing
{
    [Cmdlet(VerbsCommon.Get, "LevenshteinDistance")]
    public class GetLevenshteinDistanceCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The string to be compared.")]
        public string ComparedString { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The string to be compared with.")]
        public string ComparingString { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            Levenshtein levenshtein = new Levenshtein();

            double distance = levenshtein.Distance(ComparedString, ComparingString);

            this.WriteObject(distance);
        }
    }
}
