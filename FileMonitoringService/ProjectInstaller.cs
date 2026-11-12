using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceProcess;
namespace FileMonitoringService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            // Configure the service account.
            ServiceProcessInstaller processInstaller =new ServiceProcessInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;

            // Configure the Windows Service.
            ServiceInstaller serviceInstaller =new ServiceInstaller();

            serviceInstaller.ServiceName = "FileMonitoringService";

            serviceInstaller.DisplayName = "File Monitoring Service";

            serviceInstaller.Description =
                "Monitors a folder and moves files after renaming them with GUIDs.";

            serviceInstaller.StartType = ServiceStartMode.Automatic;

            // Register both installers.
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
