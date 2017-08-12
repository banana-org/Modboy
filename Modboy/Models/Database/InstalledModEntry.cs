// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <InstalledModEntry.cs>
//  Created By: Alexey Golub
//  Date: 21/02/2016
// ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using NegativeLayer.Extensions;
using SQLite;

namespace Modboy.Models.Database
{
    public class InstalledModEntry
    {
        [PrimaryKey, AutoIncrement]
        public int RowID { get; set; }

        public string ModID { get; set; }
        public string AffectedFilesStr { get; set; }
        public string[] FileChanges => AffectedFilesStr.SplitTrim(Constants.UniformSeparator);
        public DateTime InstallDate { get; set; }

        public InstalledModEntry(string modID, string fileChangesStr, DateTime installDate)
        {
            ModID = modID;
            AffectedFilesStr = fileChangesStr;
            InstallDate = installDate;
        }

        public InstalledModEntry(string modID, IEnumerable<string> fileChanges, DateTime installDate)
        {
            ModID = modID;
            AffectedFilesStr = fileChanges.JoinToString(Constants.UniformSeparator);
            InstallDate = installDate;
        }

        public InstalledModEntry(string modID)
        {
            ModID = modID;
        }

        public InstalledModEntry() { }
    }
}