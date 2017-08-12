// -------------------------------------------------------------------------
// Solution: GameBananaClient
// Project: Modboy
// File: MessageBoxViewModel.cs
// 
// Created by: Tyrrrz
// On: 06.09.2016
// -------------------------------------------------------------------------

using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Modboy.Models.Internal;

namespace Modboy.ViewModels
{
    public class MessageBoxViewModel : ViewModelBase
    {
        private Window _window;
        private bool _result;

        public Localization Localization { get; } = Localization.Current;

        // Fields
        public MessageBoxButtonSet ButtonSet { get; set; }
        public MessageBoxIcon Icon { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public bool Button1Visible => true;
        public bool Button2Visible => ButtonSet == MessageBoxButtonSet.YesNo;
        public string Button1Text
        {
            get
            {
                if (ButtonSet == MessageBoxButtonSet.YesNo)
                    return Localization.Global_Yes;
                return Localization.Global_OK;
            }
        }
        public string Button2Text
        {
            get
            {
                if (ButtonSet == MessageBoxButtonSet.YesNo)
                    return Localization.Global_No;
                return Localization.Global_Cancel;
            }
        }
        public bool Result
        {
            get { return _result; }
            set { Set(ref _result, value); }
        }

        // Commands
        public RelayCommand<Window> WindowShownCommand { get; }
        public RelayCommand Button1Command { get; }
        public RelayCommand Button2Command { get; }

        public MessageBoxViewModel()
        {
            // Commands
            WindowShownCommand = new RelayCommand<Window>(w => _window = w);
            Button1Command = new RelayCommand(Button1);
            Button2Command = new RelayCommand(Button2);
        }

        private void Button1()
        {
            Result = true;
            if (_window != null)
            {
                _window.DialogResult = true;
                _window.Close();
            }
        }

        private void Button2()
        {
            Result = false;
            if (_window != null)
            {
                _window.DialogResult = false;
                _window.Close();
            }
        }
    }
}