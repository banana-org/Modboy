// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <HistoryEntryRecordedArgs.cs>
//  Created By: Alexey Golub
//  Date: 06/03/2016
// ------------------------------------------------------------------ 

using Modboy.Models.Database;

namespace Modboy.Models.EventArgs
{
    public class HistoryEntryRecordedArgs : System.EventArgs
    {
        public HistoryEntry Entry { get; }

        public HistoryEntryRecordedArgs(HistoryEntry entry)
        {
            Entry = entry;
        }
    }
}