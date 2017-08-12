// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <AboutViewModel.cs>
//  Created By: Alexey Golub
//  Date: 25/02/2016
// ------------------------------------------------------------------ 

using System;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Modboy.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public Localization Localization { get; } = Localization.Current;

        public string VersionText { get; } = $"ver. {App.CurrentVersion}";
        public string CreditsText { get; }

        // Commands
        public RelayCommand MoreInfoCommand { get; }

        public AboutViewModel()
        {
            // Defaults
            CreditsText =
                "Alexey Golub --- Client-Side Programmer" + Environment.NewLine +
                "Tom Pittlik --- Server-Side Programmer" + Environment.NewLine +
                Environment.NewLine +
                "Tatu Eugen --- Romanian Translation";

            // Commands
            MoreInfoCommand = new RelayCommand(MoreInfo);
        }

        private void MoreInfo()
        {
            try
            {
                Process.Start(Constants.URLAboutPage);
            }
            catch (Exception ex)
            {
                Logger.Record("Could not navigate to MoreInfo on About window");
                Logger.Record(ex);
            }
        }
    }
}