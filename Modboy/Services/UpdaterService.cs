// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <UpdaterService.cs>
//  Created By: Alexey Golub
//  Date: 24/02/2016
// ------------------------------------------------------------------ 

using NegativeLayer.Extensions;
using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using Modboy.Models.EventArgs;
using Modboy.Models.Internal;
using NegativeLayer.WPFExtensions;

namespace Modboy.Services
{
    public class UpdaterService
    {
        private readonly TaskExecutionService _taskExecutionService;
        private readonly WindowService _windowService;
        private readonly APIService _apiService = new APIService();
        private readonly WebService _webService = new WebService();

        private readonly Timer _timer = new Timer();

        private double _progress;
        public double Progress
        {
            get { return _progress; }
            private set
            {
                _progress = value;
                ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(value));
            }
        }

        /// <summary>
        /// Event that triggers when the update process has begun
        /// </summary>
        public event EventHandler UpdateProcessStarted;

        /// <summary>
        /// Event that triggers when the update download progress has changed
        /// </summary>
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        public UpdaterService(TaskExecutionService taskExecutionService, WindowService windowService)
        {
            _taskExecutionService = taskExecutionService;
            _windowService = windowService;

            // Events
            _webService.ProgressChanged += (sender, args) => Progress = args.Progress;

            // Timer
            _timer.Interval = TimeSpan.FromHours(1).TotalMilliseconds;
            _timer.Elapsed += (sender, args) => PerformUpdate();
        }

        private void PerformUpdate()
        {
#if DEBUG
            // Don't check for updates in debug
            return;
#endif

            // Check if settings allow updates
            if (!Settings.Stager.Current.AutoUpdate) return;

            // Check for updates
            bool updatesAvailable = CheckForUpdates();
            if (!updatesAvailable) return;

            // Prompt
            bool userAgreed = _windowService.ShowPromptWindowAsync(Localization.Current.Updater_UpdatePrompt).GetResult();
            if (!userAgreed) return;

            // Notify that the update started and show a dialog
            UpdateProcessStarted?.Invoke(this, EventArgs.Empty);
            _windowService.ShowUpdaterWindowAsync().Forget();

            // Abort all tasks
            _taskExecutionService.AbortAllTasks();

            // Get download URL
            string downloadURL = GetDownloadURL();
            if (downloadURL.IsBlank())
            {
                _windowService.ShowErrorWindowAsync(Localization.Current.Updater_UpdateDownloadFailed).GetResult();
                return;
            }

            // Download the file
            var downloadedFile = _webService.Download(downloadURL, FileSystem.CreateTempFile("ModboyUpdate.exe"));
            if (downloadedFile == null || !downloadedFile.Exists)
            {
                _windowService.ShowErrorWindowAsync(Localization.Current.Updater_UpdateDownloadFailed).GetResult();
                return;
            }

            // Launch the installer
            var process = Process.Start(downloadedFile.FullName, "/SP- /SILENT /SUPPRESSMSGBOXES /CLOSEAPPLICATIONS /RESTARTAPPLICATIONS");
            if (process == null)
            {
                _windowService.ShowErrorWindowAsync(Localization.Current.Updater_UpdateInstallFailed).GetResult();
                return;
            }

            // Shutdown application
            Logger.Record("Update is being installed, shutting down");
            Application.Current.ShutdownSafe(ExitCode.UpdateInstallerExecuted);
        }

        /// <summary>
        /// Checks the server for newer client version
        /// </summary>
        /// <returns>True if available</returns>
        private bool CheckForUpdates()
        {
            try
            {
                var latestVersion = _apiService.GetLatestAvailableVersion();
                if (latestVersion == null) return false;
                return latestVersion > App.CurrentVersion;
            }
            catch (Exception ex)
            {
                Logger.Record("Could not check for updates");
                Logger.Record(ex);

                return false;
            }
        }

        /// <summary>
        /// Get download URL for latest version
        /// </summary>
        private string GetDownloadURL()
        {
            try
            {
                string downloadURL = _apiService.GetLatestAvailableVersionDownloadURL();
                return downloadURL;
            }
            catch (Exception ex)
            {
                Logger.Record("Could not get the download URL for the update installer");
                Logger.Record(ex);

                return null;
            }
        }

        /// <summary>
        /// Starts monitoring for updates
        /// </summary>
        public void Start()
        {
            PerformUpdate();
            _timer.Start();
        }

        /// <summary>
        /// Stops monitoring for updates
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }
    }
}