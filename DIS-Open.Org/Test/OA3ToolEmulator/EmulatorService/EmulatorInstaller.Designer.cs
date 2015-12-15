namespace EmulatorService
{
    partial class EmulatorInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.EmulatorProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // EmulatorProcessInstaller
            // 
            this.EmulatorProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.EmulatorProcessInstaller.Password = null;
            this.EmulatorProcessInstaller.Username = null;
            // 
            // ServiceInstaller
            // 
            this.ServiceInstaller.Description = "OA3 Tool Emulator";
            this.ServiceInstaller.DisplayName = "OA3ToolEmulatorService";
            this.ServiceInstaller.ServiceName = "EmulatorService";
            this.ServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // EmulatorInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.EmulatorProcessInstaller,
            this.ServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller EmulatorProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ServiceInstaller;
    }
}