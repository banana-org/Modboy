// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <MainViewModel.cs>
//  Created By: Alexey Golub
//  Date: 02/03/2016
// ------------------------------------------------------------------ 

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Hardcodet.Wpf.TaskbarNotification;
using Modboy.Models.Internal;
using Modboy.Services;
using Modboy.Views;
using NegativeLayer.Extensions;
using NegativeLayer.WPFExtensions;

namespace Modboy.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public Localization Localization { get; } = Localization.Current;
        public Settings Settings { get; } = Settings.Stager.Current;

        private TaskbarIcon _taskbarIcon;

        public string AppVersion => App.CurrentVersion.ToString();

        // Commands
        public RelayCommand<Window> WindowShownCommand { get; }
        public RelayCommand<CancelEventArgs> WindowClosingCommand { get; }
        public RelayCommand OpenProfileCommand { get; }
        public RelayCommand ShowHideWindowCommand { get; }
        public RelayCommand ShowAboutCommand { get; }
        public RelayCommand ShowHelpCommand { get; }
        public RelayCommand SubmitBugReportCommand { get; }
        public RelayCommand ExitComand { get; }

        public MainViewModel(TaskExecutionService taskExecutionService, HistoryService historyService, WindowService windowService)
        {
            // Commands
            WindowShownCommand = new RelayCommand<Window>(w =>
            {
                _taskbarIcon = (w as MainWindow)?.MainTaskbarIcon; // HACK: slightly MVVM breaking
                Locator.StartServicesAsync().Forget(); // Start services after the main window is loaded
            });
            WindowClosingCommand = new RelayCommand<CancelEventArgs>(args =>
            {
                args.Cancel = true;
                windowService.MainWindowHide();
            });

            OpenProfileCommand = new RelayCommand(OpenProfile);
            ShowHideWindowCommand = new RelayCommand(windowService.MainWindowToggleShowHide);
            ShowAboutCommand = new RelayCommand(async () => await windowService.ShowAboutWindowAsync());
            ShowHelpCommand = new RelayCommand(ShowHelp);
            SubmitBugReportCommand = new RelayCommand(async () => await windowService.ShowBugReportWindowAsync());
            ExitComand = new RelayCommand(() => Application.Current.ShutdownSafe(ExitCode.UserExit));

            // Events
            taskExecutionService.TaskAddedToQueue += (sender, args) => windowService.MainWindowShow();
            historyService.HistoryEntryRecorded += (sender, args) =>
            {
                if (!Settings.ShowNotifications) return;
                _taskbarIcon?.ShowBalloonTip("Modboy", Localization.Localize(args.Entry),
                    args.Entry.Success ? BalloonIcon.Info : BalloonIcon.Error);
            };
        }

        private void OpenProfile()
        {
            try
            {
                Process.Start(string.Format(Constants.URLProfilePage, Settings.UserID));
            }
            catch (Exception ex)
            {
                Logger.Record("Could not open profile");
                Logger.Record(ex);
            }
        }

        private void ShowHelp()
        {
            try
            {
                Process.Start(Constants.URLHelp);
            }
            catch (Exception ex)
            {
                Logger.Record("Could not navigate to MoreInfo on About window");
                Logger.Record(ex);
            }
        }
    }
}