using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace EmulatorService
{
    [RunInstaller(true)]
    public partial class EmulatorInstaller : System.Configuration.Install.Installer
    {
        public EmulatorInstaller()
        {
            InitializeComponent();
        }
    }
}
