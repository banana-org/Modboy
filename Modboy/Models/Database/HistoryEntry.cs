// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <HistoryEntry.cs>
//  Created By: Alexey Golub
//  Date: 06/03/2016
// ------------------------------------------------------------------ 

using System;
using Modboy.Models.Internal;
using SQLite;

namespace Modboy.Models.Database
{
    public class HistoryEntry
    {
        [PrimaryKey, AutoIncrement]
        public int RowID { get; set; }

        public DateTime Date { get; set; }
        public TaskType TaskType { get; set; }
        public bool Success { get; set; }
        public string ModID { get; set; }
        public string ModName { get; set; }

        public HistoryEntry(DateTime date, TaskType taskType, bool success, string modID, string modName)
        {
            Date = date;
            TaskType = taskType;
            Success = success;
            ModID = modID;
            ModName = modName ?? modID;
        }

        public HistoryEntry() { }
    }
}