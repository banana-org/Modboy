// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <HistoryService.cs>
//  Created By: Alexey Golub
//  Date: 06/03/2016
// ------------------------------------------------------------------ 

using System;
using System.Linq;
using Modboy.Models.Database;
using Modboy.Models.EventArgs;

namespace Modboy.Services
{
    public class HistoryService
    {
        private readonly TaskExecutionService _taskExecutionService;
        private readonly DatabaseService _databaseService;
        private readonly APIService _apiService = new APIService();

        /// <summary>
        /// Event that triggers when a new history record was added
        /// </summary>
        public event EventHandler<HistoryEntryRecordedArgs> HistoryEntryRecorded;

        public HistoryService(TaskExecutionService taskExecutionService, DatabaseService databaseService)
        {
            _taskExecutionService = taskExecutionService;
            _databaseService = databaseService;
        }

        private void OnTaskEnded(object sender, TaskEndedEventArgs args)
        {
            var modInfo = _apiService.GetModInfo(args.Task.ModID);
            RecordEvent(new HistoryEntry(
                DateTime.Now,
                args.Task.Type,
                args.Success,
                args.Task.ModID,
                modInfo?.Name));
        }

        private void RecordEvent(HistoryEntry entry)
        {
            _databaseService.DB.Insert(entry);
            HistoryEntryRecorded?.Invoke(this, new HistoryEntryRecordedArgs(entry));
        }

        public HistoryEntry[] GetHistory()
        {
            return _databaseService.DB.Table<HistoryEntry>().OrderByDescending(he => he.RowID).ToArray();
        }

        public HistoryEntry[] GetHistory(int top)
        {
            return _databaseService.DB.Table<HistoryEntry>().OrderBy(he => he.Date).Take(top).ToArray();
        }

        public void ClearHistory()
        {
            _databaseService.DB.DeleteAll<HistoryEntry>();
        }

        public void Start()
        {
            Stop();
            _taskExecutionService.TaskEnded += OnTaskEnded;
        }

        public void Stop()
        {
            _taskExecutionService.TaskEnded -= OnTaskEnded;
        }
    }
}