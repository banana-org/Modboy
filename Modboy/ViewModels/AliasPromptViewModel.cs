// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <AliasPromptViewModel.cs>
//  Created By: Alexey Golub
//  Date: 18/02/2016
// ------------------------------------------------------------------ 

using System.Windows;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Modboy.Models.Internal;
using Modboy.Services;
using NegativeLayer.Extensions;

namespace Modboy.ViewModels
{
    public class AliasPromptViewModel : ViewModelBase
    {
        private readonly WindowService _windowService;

        private Window _window;
        private string _selectedValue;

        public Localization Localization { get; } = Localization.Current;

        // Fields
        public AliasPromptType Type { get; set; }
        public string Text { get; set; }
        public string DefaultValue { get; set; }
        public bool TargetShouldExist { get; set; }
        public bool LocateButtonVisible => Type.IsEither(AliasPromptType.Directory, AliasPromptType.File);
        public string SelectedValue
        {
            get { return _selectedValue; }
            set { Set(ref _selectedValue, value); }
        }

        // Commands
        public RelayCommand<Window> WindowShownCommand { get; }
        public RelayCommand LocateCommand { get; }
        public RelayCommand OkCommand { get; }
        public RelayCommand CancelCommand { get; }

        public AliasPromptViewModel(WindowService windowService)
        {
            _windowService = windowService;

            // Commands
            WindowShownCommand = new RelayCommand<Window>(w => _window = w);
            LocateCommand = new RelayCommand(Locate);
            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Locate()
        {
            string path = null;

            // Get file path
            if (Type == AliasPromptType.File)
            {
                var dialog = new OpenFileDialog {CheckPathExists = true, CheckFileExists = true};
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                path = dialog.FileName;
            }
            // Or get directory path
            else if (Type == AliasPromptType.Directory)
            {
                var dialog = new FolderBrowserDialog {ShowNewFolderButton = true};
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                path = dialog.SelectedPath;
            }

            // Set the result
            SelectedValue = path;
        }

        private void Ok()
        {
            // Validate the result
            if (Type == AliasPromptType.String)
            {
                // No validation implemented
            }
            else if (Type == AliasPromptType.File)
            {
                bool valid = Ext.IsValidFilePath(SelectedValue, TargetShouldExist, TargetShouldExist);
                if (!valid)
                {
                    _windowService.ShowErrorWindowAsync(Localization.Prompt_InvalidFile).GetResult();
                    return;
                }
            }
            else if (Type == AliasPromptType.Directory)
            {
                bool valid = Ext.IsValidDirectoryPath(SelectedValue, TargetShouldExist);
                if (!valid)
                {
                    _windowService.ShowErrorWindowAsync(Localization.Prompt_InvalidDir).GetResult();
                    return;
                }
            }

            // Close the window
            if (_window != null)
            {
                _window.DialogResult = true;
                _window.Close();
            }
        }

        private void Cancel()
        {
            // Close the window
            if (_window != null)
            {
                _window.DialogResult = false;
                _window.Close();
            }
        }
    }
}