using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileMonitoringService
{
    public partial class FileMonitoringService : ServiceBase
    {
        public FileMonitoringService()
        {
            InitializeComponent();

            ServiceName = "FileMonitoringService";
            CanStop = true;
            CanPauseAndContinue = false;
            AutoLog = false;

        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
