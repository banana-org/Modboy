// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <DatabaseService.cs>
//  Created By: Alexey Golub
//  Date: 14/02/2016
// ------------------------------------------------------------------ 

using System.Linq;
using System.Text;
using Modboy.Models.Database;
using Newtonsoft.Json;
using SQLite;

namespace Modboy.Services
{
    public class DatabaseService
    {
        // ReSharper disable once InconsistentNaming
        public SQLiteConnection DB { get; } = new SQLiteConnection(FileSystem.DatabaseFilePath, true);

        public DatabaseService()
        {
            // Create tables if they don't exist yet
            DB.CreateTable<BackupEntry>(CreateFlags.AutoIncPK);
            DB.CreateTable<HistoryEntry>(CreateFlags.AutoIncPK);
            DB.CreateTable<InstalledModEntry>(CreateFlags.AutoIncPK);
        }

        public string GetDatabaseDump()
        {
            var buffer = new StringBuilder();

            buffer.AppendLine("==========================================================");
            buffer.AppendLine("== Backup Entries ==");
            buffer.AppendLine("==========================================================");
            buffer.AppendLine(JsonConvert.SerializeObject(DB.Table<BackupEntry>().ToArray()));
            buffer.AppendLine("==========================================================");
            buffer.AppendLine();
            buffer.AppendLine("==========================================================");
            buffer.AppendLine("== History Entries ==");
            buffer.AppendLine("==========================================================");
            buffer.AppendLine(JsonConvert.SerializeObject(DB.Table<HistoryEntry>().ToArray()));
            buffer.AppendLine("==========================================================");
            buffer.AppendLine();
            buffer.AppendLine("==========================================================");
            buffer.AppendLine("== Installed Mod Entries ==");
            buffer.AppendLine("==========================================================");
            buffer.AppendLine(JsonConvert.SerializeObject(DB.Table<InstalledModEntry>().ToArray()));
            buffer.AppendLine("==========================================================");

            return buffer.ToString();
        }
    }
}