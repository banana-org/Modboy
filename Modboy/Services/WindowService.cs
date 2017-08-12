// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <WindowService.cs>
//  Created By: Alexey Golub
//  Date: 18/08/2016
// ------------------------------------------------------------------ 

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using Modboy.Models.Internal;
using Modboy.ViewModels;
using Modboy.Views;
using NegativeLayer.WPFExtensions;
using Task = System.Threading.Tasks.Task;

namespace Modboy.Services
{
    public class WindowService
    {
        private readonly Window _mainWindow = Application.Current.MainWindow;
        private bool _mainWindowShown = true;
        private Dispatcher _dispatcher => DispatcherHelper.UIDispatcher;

        public void MainWindowShow()
        {
            _dispatcher.InvokeSafe(() =>
            {
                _mainWindow.Show();
                _mainWindow.Activate();
                _mainWindow.Focus();
                _mainWindowShown = true;
            });
        }

        public void MainWindowHide()
        {
            _dispatcher.InvokeSafe(() =>
            {
                _mainWindow.Hide();
                _mainWindowShown = false;
            });
        }

        public void MainWindowToggleShowHide()
        {
            if (_mainWindowShown)
                MainWindowHide();
            else
                MainWindowShow();
        }

        public async Task ShowErrorWindowAsync(string text)
        {
            MainWindowShow();

            var viewModel = Locator.Resolve<MessageBoxViewModel>();
            viewModel.ButtonSet = MessageBoxButtonSet.Ok;
            viewModel.Icon = MessageBoxIcon.Error;
            viewModel.Title = "Modboy";
            viewModel.Message = text;

            await _dispatcher.InvokeSafeAsync(() => new MessageBoxWindow().ShowDialog());
            MainWindowShow();
        }

        public async Task<bool> ShowPromptWindowAsync(string text)
        {
            MainWindowShow();

            var viewModel = Locator.Resolve<MessageBoxViewModel>();
            viewModel.ButtonSet = MessageBoxButtonSet.YesNo;
            viewModel.Icon = MessageBoxIcon.Prompt;
            viewModel.Title = "Modboy";
            viewModel.Message = text;

            var result = await _dispatcher.InvokeSafeAsync(() => new MessageBoxWindow().ShowDialog().GetValueOrDefault());
            MainWindowShow();

            return result;
        }

        public async Task<bool> ShowErrorPromptWindowAsync(string text)
        {
            MainWindowShow();

            var viewModel = Locator.Resolve<MessageBoxViewModel>();
            viewModel.ButtonSet = MessageBoxButtonSet.YesNo;
            viewModel.Icon = MessageBoxIcon.Error;
            viewModel.Title = "Modboy";
            viewModel.Message = text;

            var result = await _dispatcher.InvokeSafeAsync(() => new MessageBoxWindow().ShowDialog().GetValueOrDefault());
            MainWindowShow();

            return result;
        }

        public async Task ShowNotificationWindowAsync(string text)
        {
            MainWindowShow();

            var viewModel = Locator.Resolve<MessageBoxViewModel>();
            viewModel.ButtonSet = MessageBoxButtonSet.Ok;
            viewModel.Icon = MessageBoxIcon.Notification;
            viewModel.Title = "Modboy";
            viewModel.Message = text;

            await _dispatcher.InvokeSafeAsync(() => new MessageBoxWindow().ShowDialog());
            MainWindowShow();
        }

        public async Task ShowAboutWindowAsync()
        {
            await _dispatcher.InvokeSafeAsync(() => new AboutWindow().ShowDialog());
            MainWindowShow();
        }

        public async Task<int> ShowComboSelectWindowAsync(string text, string[] items, int defaultValueIndex = 0)
        {
            var viewModel = Locator.Resolve<ComboSelectViewModel>();
            viewModel.Text = text;
            viewModel.Items = items;
            viewModel.DefaultValueIndex = defaultValueIndex;

            bool success =
                await _dispatcher.InvokeSafeAsync(() => new ComboSelectWindow().ShowDialog().GetValueOrDefault());
            MainWindowShow();

            return success ? viewModel.SelectedValueIndex : -1;
        }

        public async Task<string> ShowAliasPromptWindowAsync(AliasPromptType type, string text,
            string defaultValue = null, bool targetShouldExist = true)
        {
            var viewModel = Locator.Resolve<AliasPromptViewModel>();
            viewModel.Type = type;
            viewModel.Text = text;
            viewModel.DefaultValue = defaultValue;
            viewModel.TargetShouldExist = targetShouldExist;

            bool success =
                await _dispatcher.InvokeSafeAsync(() => new AliasPromptWindow().ShowDialog().GetValueOrDefault());
            MainWindowShow();

            return success ? viewModel.SelectedValue : null;
        }

        public async Task ShowBugReportWindowAsync(Exception exception = null)
        {
            var viewModel = Locator.Resolve<BugReportViewModel>();
            viewModel.Exception = exception;

            await _dispatcher.InvokeSafeAsync(() => new BugReportWindow().ShowDialog());
            MainWindowShow();
        }

        public async Task ShowUpdaterWindowAsync()
        {
            await _dispatcher.InvokeSafeAsync(() => new UpdaterWindow().ShowDialog());
            MainWindowShow();
        }
    }
}