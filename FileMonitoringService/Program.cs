using System;
using System.ServiceProcess;

namespace FileMonitoringService
{
    internal static class Program
    {
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                // Run in console debugging mode.
                FileMonitoringService service =new FileMonitoringService();

                service.StartForDebugging();

                Console.WriteLine();
                Console.WriteLine("File Monitoring Service is running.");
                Console.WriteLine("Press any key to stop...");

                Console.ReadKey();

                service.StopForDebugging();
            }
            else
            {
                // Run as a Windows Service.
                ServiceBase.Run(
                    new ServiceBase[]
                    {
                        new FileMonitoringService()
                    });
            }
        }
    }
}