using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;

namespace DIS.Management.Deployment
{
    [Cmdlet(VerbsCommon.Get, "HomeDirectory")]
    public class GetHomeDirectoryCmdlet : Cmdlet
    {
        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            string assemblyLocation = assemblies.First((o) => (o.FullName.ToLower().Contains("dis.management.deployment"))).Location;

            this.WriteObject(System.IO.Directory.GetParent(assemblyLocation).FullName);
        }
    }
}
