
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FileMonitoringService
{
    public static class Logger
    {
        private static readonly object _lock = new object();

        private static string _logFilePath;
        private static bool _writeToConsole;

        // Initialize the logger with the configured log directory.
        public static void Initialize(string logFolder, bool writeToConsole = false)
        {
            lock (_lock)
            {
                Directory.CreateDirectory(logFolder);

                string fileName = $"FileMonitoringService_{DateTime.Now:yyyy-MM-dd}.log";

                _logFilePath = Path.Combine(logFolder, fileName);

                _writeToConsole = writeToConsole;
            }
        }

        // Write a message to the log file and optionally to the console.
        public static void Log(string message)
        {
            string logMessage =$"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

            lock (_lock)
            {
                // Display the message during console debugging.
                if (_writeToConsole)
                {
                    Console.WriteLine(logMessage);
                }

                try
                {
                    if (string.IsNullOrEmpty(_logFilePath))
                    {
                        throw new InvalidOperationException("Logger has not been initialized.");
                    }

                    File.AppendAllText(_logFilePath,logMessage + Environment.NewLine,Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Logging failed: {ex.Message}");
                }
            }
        }
    }
}
