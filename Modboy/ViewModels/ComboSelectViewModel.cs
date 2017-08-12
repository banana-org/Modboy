// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <ComboSelectViewModel.cs>
//  Created By: Alexey Golub
//  Date: 19/02/2016
// ------------------------------------------------------------------ 

using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Modboy.ViewModels
{
    public class ComboSelectViewModel : ViewModelBase
    {
        private Window _window;
        private int _selectedValueIndex;

        public Localization Localization { get; } = Localization.Current;

        // Fields
        public string Text { get; set; }
        public string[] Items { get; set; }
        public int DefaultValueIndex { get; set; }
        public int SelectedValueIndex
        {
            get { return _selectedValueIndex; }
            set { Set(ref _selectedValueIndex, value); }
        }

        // Commands
        public RelayCommand<Window> WindowShownCommand { get; }
        public RelayCommand OkCommand { get; }
        public RelayCommand CancelCommand { get; }

        public ComboSelectViewModel()
        {
            // Commands
            WindowShownCommand = new RelayCommand<Window>(w => _window = w);
            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Ok()
        {
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