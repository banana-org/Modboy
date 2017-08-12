// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <BugReportViewModel.cs>
//  Created By: Alexey Golub
//  Date: 28/06/2016
// ------------------------------------------------------------------ 

using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Modboy.Services;
using NegativeLayer.Extensions;

namespace Modboy.ViewModels
{
    public class BugReportViewModel : ViewModelBase
    {
        private readonly DatabaseService _databaseService;
        private readonly WindowService _windowService;
        private readonly APIService _apiService = new APIService();

        private readonly TaskFactory _taskFactory = new TaskFactory(TaskCreationOptions.PreferFairness,
            TaskContinuationOptions.PreferFairness);

        private Window _window;
        private bool _isBusy;

        public Localization Localization { get; } = Localization.Current;

        // Fields
        public Exception Exception { get; set; }
        public bool IsCrashReport => Exception != null;
        public string Message { get; set; }
        public bool IncludeLogs { get; set; } = true;
        public bool IncludeDB { get; set; } = true;
        public string MailBack { get; set; }
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                Set(ref _isBusy, value);
                SendReportCommand.RaiseCanExecuteChanged();
                DiscardReportCommand.RaiseCanExecuteChanged();
            }
        }

        // Commands
        public RelayCommand<Window> WindowShownCommand { get; }
        public RelayCommand SendReportCommand { get; }
        public RelayCommand DiscardReportCommand { get; }

        public BugReportViewModel(DatabaseService databaseService, WindowService windowService)
        {
            _databaseService = databaseService;
            _windowService = windowService;

            // Commands
            WindowShownCommand = new RelayCommand<Window>(w => _window = w);
            SendReportCommand = new RelayCommand(SendReportAsync, () => !IsBusy);
            DiscardReportCommand = new RelayCommand(DiscardReport, () => !IsBusy);
        }

        private string GetSystemInfo()
        {
            var buffer = new StringBuilder();

            buffer.AppendLine("===== System Info =====");
            buffer.AppendLine($"OS: {Environment.OSVersion} ({Environment.OSVersion.GetFriendlyName()})");
            buffer.AppendLine($"CPU Cores: {Environment.ProcessorCount}");
            buffer.AppendLine($"System Page Size: {Environment.SystemPageSize}");
            buffer.AppendLine($"Working Set: {Environment.WorkingSet}");
            buffer.AppendLine("=======================");

            return buffer.ToString();
        }

        private async void SendReportAsync()
        {
            IsBusy = true;

            // The report must have at least either the message or the exception object
            if (Message.IsBlank() && Exception == null)
            {
                await _windowService.ShowErrorWindowAsync(Localization.BugReport_NoMessageSpecified);
                IsBusy = false;
                return;
            }

            // Check if mailback is set
            if (MailBack.IsBlank())
            {
                bool shouldContinue = await _windowService.ShowPromptWindowAsync(Localization.BugReport_NoMailBackSpecified);
                if (!shouldContinue)
                {
                    IsBusy = false;
                    return;
                }
            }

            // Get system info
            string systemInfo = GetSystemInfo();

            // Get the log file
            string logDump = IncludeLogs ? Logger.GetLogDump() : null;
            string databaseDump = IncludeDB ? _databaseService.GetDatabaseDump() : null;

            // Send
            await _taskFactory.StartNew(
                () => _apiService.ReportException(Settings.Stager.Current.UserID, Message, systemInfo, Exception, logDump, databaseDump, MailBack));

            // Report completion
            await _windowService.ShowNotificationWindowAsync(Localization.BugReport_ReportSent);
            IsBusy = false;

            // Close window
            _window?.Close();
        }

        private void DiscardReport()
        {
            // Close window
            _window?.Close();
        }
    }
}