# File Monitoring Windows Service

## 1. Project Overview

File Monitoring Service is a Windows Service developed using C# and .NET Framework 4.8. It continuously monitors a specified source folder for newly created files, renames them using a unique GUID, and moves them to a destination folder.

The service also maintains timestamped logs to record file-processing activities and errors.

## 2. Features

- Monitors a folder using `FileSystemWatcher`.
- Detects newly created files.
- Renames files using GUIDs while preserving their extensions.
- Moves processed files to a destination folder.
- Records events and errors in daily log files.
- Loads folder configurations dynamically from `App.config`.
- Automatically creates required directories.
- Supports console mode for debugging.
- Supports Windows Service installation and automatic startup.

## 3. Technologies Used

- C#
- .NET Framework 4.8
- Windows Services
- FileSystemWatcher
- System.Configuration
- Windows Service Installer

## 4. Folder Configuration

The application uses `App.config` to configure its folders:

```xml
<appSettings>
    <add key="SourceFolder" value="C:\FileMonitoring\Source" />
    <add key="DestinationFolder" value="C:\FileMonitoring\Destination" />
    <add key="LogFolder" value="C:\FileMonitoring\Logs" />
</appSettings>
```

The application creates these directories automatically if they do not exist.

## 5. How It Works

1. The service loads its configuration from `App.config`.
2. It initializes the required directories and logger.
3. `FileSystemWatcher` monitors the source folder.
4. When a new file is detected, the service attempts to process it.
5. The original filename is replaced with a GUID while preserving the extension.
6. The file is moved to the destination folder.
7. The operation is recorded in the log file.

### Example

Original file:

`document.pdf`

Processed file:

`a1b2c3d4-e5f6-7890-abcd-123456789012.pdf`

## 6. Logging

The application creates daily log files inside the configured log folder.

Example:

`FileMonitoringService_2026-11-12.log`

Log entries include timestamps and information about service initialization, file detection, successful processing, and errors.

## 7. Running in Debug Mode

The application supports console execution for development and debugging.

Run the executable from Visual Studio or the build output directory. The console displays service messages and allows the developer to stop execution using a keyboard key.

## 8. Installing the Windows Service

Build the project using the **Release** configuration.

Open Command Prompt as Administrator and execute:

```cmd
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe "PATH_TO_FileMonitoringService.exe"
```

Start the service:

```cmd
sc start FileMonitoringService
```

Check its status:

```cmd
sc query FileMonitoringService
```

The service is configured to start automatically with Windows.

## 9. Stopping and Uninstalling

Stop the service:

```cmd
sc stop FileMonitoringService
```

Uninstall it using:

```cmd
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe /u "PATH_TO_FileMonitoringService.exe"
```

Replace `PATH_TO_FileMonitoringService.exe` with the actual executable path.

## 10. Testing

The project was tested in both console debugging mode and installed Windows Service mode.

Tests included:

- Processing individual files.
- Processing multiple files.
- Preserving file extensions after renaming.
- Moving files to the destination folder.
- Verifying log entries.
- Starting and stopping the Windows Service.
- Restarting the service and verifying that monitoring resumes.

## 11. Requirements

- Windows operating system.
- .NET Framework 4.8.
- Visual Studio (for development).
- Administrator privileges for service installation.

## 12. Author

**Abubakar Yimam**

GitHub: [Ibnu-Imam03](https://github.com/Ibnu-Imam03)
