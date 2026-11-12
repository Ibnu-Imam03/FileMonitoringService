using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace FileMonitoringService
{
    public partial class FileMonitoringService : ServiceBase
    {

        private string _sourceFolder;
        private string _destinationFolder;
        private string _logFolder;
        private FileSystemWatcher _watcher;
        public void StartForDebugging()
        {
            OnStart(new string[0]);
        }

        public void StopForDebugging()
        {
            OnStop();
        }
        private void InitializeLogger(bool writeToConsole = false)
        {
            Logger.Initialize(_logFolder, writeToConsole);

            Logger.Log("File Monitoring Service initialized.");
        }
        private void InitializeDirectories()
        {
            Directory.CreateDirectory(_sourceFolder);
            Directory.CreateDirectory(_destinationFolder);
            Directory.CreateDirectory(_logFolder);
        }
        private void LoadConfiguration()
        {
            _sourceFolder = ConfigurationManager.AppSettings["SourceFolder"];

            _destinationFolder = ConfigurationManager.AppSettings["DestinationFolder"];

            _logFolder = ConfigurationManager.AppSettings["LogFolder"];

            // Validate the configuration values.
            if (string.IsNullOrWhiteSpace(_sourceFolder))
                throw new ConfigurationErrorsException("SourceFolder is missing from App.config.");

            if (string.IsNullOrWhiteSpace(_destinationFolder))
                throw new ConfigurationErrorsException("DestinationFolder is missing from App.config.");

            if (string.IsNullOrWhiteSpace(_logFolder))
                throw new ConfigurationErrorsException("LogFolder is missing from App.config.");
        }
        public FileMonitoringService()
        {
            InitializeComponent();

            ServiceName = "FileMonitoringService";
            CanStop = true;
            CanPauseAndContinue = false;
            AutoLog = false;

        }

        private void ProcessFile(string sourcePath)
        {
            const int maxAttempts = 30;

            // Get the original file extension.
            string extension = Path.GetExtension(sourcePath);

            // Generate a new GUID filename.
            string newFileName = Guid.NewGuid().ToString() + extension;

            // Build the destination path.
            string destinationPath =
                Path.Combine(_destinationFolder, newFileName);

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    // Move the file and give it its new name.
                    File.Move(sourcePath, destinationPath);

                    Logger.Log(
                        $"File processed successfully: {destinationPath}");

                    return;
                }
                catch (IOException ex)
                {
                    // The file may still be copying or in use.
                    if (attempt == maxAttempts)
                    {
                        Logger.Log(
                            $"File processing failed after {maxAttempts} attempts: {sourcePath}. {ex.Message}");

                        return;
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Logger.Log(
                        $"Unexpected processing error: {sourcePath}. {ex.Message}");

                    return;
                }
            }
        }
        private void InitializeWatcher()
        {
            _watcher = new FileSystemWatcher();

            // The directory to monitor.
            _watcher.Path = _sourceFolder;

            // Monitor all file types.
            _watcher.Filter = "*.*";

            // Do not monitor subdirectories.
            _watcher.IncludeSubdirectories = false;

            // Detect file-name changes.
            _watcher.NotifyFilter = NotifyFilters.FileName;

            // Subscribe to the Created event.
            _watcher.Created += OnFileCreated;
            _watcher.Error += OnWatcherError;

            // Start monitoring.
            _watcher.EnableRaisingEvents = true;

            Logger.Log($"Monitoring started: {_sourceFolder}");
        }
        private void OnWatcherError(object sender, ErrorEventArgs e)
        {
            Exception exception = e.GetException();

            Logger.Log(
                $"FileSystemWatcher error: {exception.Message}");
        }
        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            // Ignore the event if the path is not a file.
            if (!File.Exists(e.FullPath))
                return;

            Logger.Log($"New file detected: {e.FullPath}");

            // Process the detected file.
            ProcessFile(e.FullPath);
        }
        protected override void OnStart(string[] args)
        {
            try
            {
                // 1. Load folder paths from App.config.
                LoadConfiguration();

                // 2. Create the required directories.
                InitializeDirectories();

                // 3. Initialize logging.
                InitializeLogger();

                InitializeWatcher();

                Logger.Log("File Monitoring Service started successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Service startup failed: {ex.Message}");

                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (_watcher != null)
                {
                    // Stop receiving filesystem events.
                    _watcher.EnableRaisingEvents = false;

                    // Unsubscribe from the event.
                    _watcher.Created -= OnFileCreated;

                    _watcher.Error -= OnWatcherError;

                    // Release watcher resources.
                    _watcher.Dispose();

                    _watcher = null;
                }

                Logger.Log("File Monitoring Service stopped successfully.");
            }
            catch (Exception ex)
            {
                Logger.Log($"Service shutdown failed: {ex.Message}");

                throw;
            }
        }
    }
}
