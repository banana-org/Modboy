// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <PersistenceService.cs>
//  Created By: Alexey Golub
//  Date: 21/02/2016
// ------------------------------------------------------------------ 

using System;
using System.Linq;
using Modboy.Models.Database;

namespace Modboy.Services
{
    public class PersistenceService
    {
        private readonly DatabaseService _databaseService;

        public PersistenceService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Get the list of all currently installed mods
        /// </summary>
        public InstalledModEntry[] GetInstalledMods()
        {
            return _databaseService.DB.Table<InstalledModEntry>().ToArray();
        }

        /// <summary>
        /// Find a record for the given mod
        /// </summary>
        public InstalledModEntry GetInstalledMod(string modID)
        {
            return _databaseService.DB.Find<InstalledModEntry>(e => e.ModID == modID);
        }

        /// <summary>
        /// Record the fact of successful installation to the database
        /// </summary>
        public void RecordInstall(string modID, string[] fileChanges, DateTime date)
        {
            _databaseService.DB.Insert(new InstalledModEntry(modID, fileChanges, date));

            Logger.Record($"Added installed mod entry to the database (#{modID})");
        }

        /// <summary>
        /// Record the fact of successful installation to the database
        /// </summary>
        public void RecordInstall(string modID, string[] fileChanges)
        {
            RecordInstall(modID, fileChanges, DateTime.Now);
        }

        /// <summary>
        /// Record the fact of successful uninstallation to the database
        /// </summary>
        public void RecordUninstall(string modID)
        {
            var entry = GetInstalledMod(modID);
            if (entry != null)
                _databaseService.DB.Delete(entry);

            Logger.Record($"Removed installed mod entry from the database (#{modID})");
        }

        /// <summary>
        /// Get the files affected by given mod's installation
        /// </summary>
        public string[] GetAffectedFiles(string modID)
        {
            var entry = GetInstalledMod(modID);
            return entry?.FileChanges;
        }

        /// <summary>
        /// Checks if the given mod is installed
        /// </summary>
        public bool IsInstalled(string modID)
        {
            return GetInstalledMod(modID) != null;
        }
    }
}