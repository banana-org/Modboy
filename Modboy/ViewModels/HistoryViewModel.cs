// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <HistoryViewModel.cs>
//  Created By: Alexey Golub
//  Date: 06/03/2016
// ------------------------------------------------------------------ 

using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using Modboy.Models.Database;
using Modboy.Services;
using NegativeLayer.Extensions;
using NegativeLayer.WPFExtensions;

namespace Modboy.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        public class HistoryViewEntry
        {
            public DateTime Date { get; } 
            public string EventDescription { get; }

            public HistoryViewEntry(DateTime date, string eventDescription)
            {
                Date = date;
                EventDescription = eventDescription;
            }
        }

        private readonly HistoryService _historyService;

        public Localization Localization { get; } = Localization.Current;

        // History entries
        public ObservableCollection<HistoryViewEntry> History { get; } = new ObservableCollection<HistoryViewEntry>();

        // Commands
        public RelayCommand ClearHistoryCommand { get; }

        public HistoryViewModel(HistoryService historyService)
        {
            _historyService = historyService;

            // Commands
            ClearHistoryCommand = new RelayCommand(() =>
            {
                _historyService.ClearHistory();
                PopulateHistory();
            });

            // Initial population
            PopulateHistory();

            // Events
            Localization.PropertyChanged += (sender, args) => PopulateHistory();
            _historyService.HistoryEntryRecorded += (sender, args) =>
            {
                DispatcherHelper.UIDispatcher.InvokeSafe(
                    () => History.Insert(0, new HistoryViewEntry(args.Entry.Date, Localization.Localize(args.Entry))));
            };
        }

        private void PopulateHistory()
        {
            History.Clear();
            foreach (var entry in _historyService.GetHistory())
                History.Add(new HistoryViewEntry(entry.Date, Localization.Localize(entry)));
        }
    }
}