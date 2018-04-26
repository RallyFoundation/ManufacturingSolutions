﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using QA.Facade;

namespace QA.PowerShell.Validation
{
    [Cmdlet(VerbsCommon.Get, "Result")]
    public class GetResultCommand : Cmdlet
    {
        protected override void ProcessRecord()
        {
            Facade.Facade.ValidateData();

            this.WriteObject(Facade.Facade.Result);
        }
    }
}