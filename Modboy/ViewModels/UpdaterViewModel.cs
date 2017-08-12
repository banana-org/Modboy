// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <SettingsViewModel.cs>
//  Created By: Alexey Golub
//  Date: 24/02/2016
// ------------------------------------------------------------------ 

using GalaSoft.MvvmLight;
using Modboy.Services;

namespace Modboy.ViewModels
{
    public class UpdaterViewModel : ViewModelBase
    {
        public Localization Localization { get; } = Localization.Current;

        // Fields
        private double _progress;
        public double Progress
        {
            get { return _progress; }
            private set { Set(ref _progress, value); }
        }

        public UpdaterViewModel(UpdaterService updaterService)
        {
            // Events
            updaterService.ProgressChanged += (sender, args) => Progress = args.Progress;
        }
    }
}