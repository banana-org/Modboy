// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <BackupService.cs>
//  Created By: Alexey Golub
//  Date: 14/02/2016
// ------------------------------------------------------------------ 

using NegativeLayer.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Modboy.Models.Database;

namespace Modboy.Services
{
    // i honestly don't remember how it works
    public class BackupService
    {
        private readonly DatabaseService _databaseService;

        public BackupService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Gets the next non-occupied install order for given file
        /// </summary>
        private long GetNextFreeInstallOrder(string originalFilePath)
        {
            // Get every entry that has this path
            var entries = _databaseService.DB.Table<BackupEntry>()
                .Where(e => e.OriginalFilePath == originalFilePath);
            // Determine next install order
            return !entries.AnySafe() ? 0 : entries.Max(e => e.InstallOrder + 1);
        }

        /// <summary>
        /// Find backup entry for given file path and mod
        /// </summary>
        private BackupEntry Find(string originalFilePath, string modID)
        {
            return
                _databaseService.DB.Find<BackupEntry>(
                    e => e.OriginalFilePath == originalFilePath && e.ModID == modID);
        }

        /// <summary>
        /// Find a backup of the given file with higher install order then the given order
        /// </summary>
        private BackupEntry FindHigherInstallOrder(string originalFilePath, long minInstallOrder)
        {
            return
                _databaseService.DB.Find<BackupEntry>(
                    e => e.OriginalFilePath == originalFilePath && e.InstallOrder > minInstallOrder);
        }

        /// <summary>
        /// Find all backups made by given mod installation
        /// </summary>
        private IEnumerable<BackupEntry> FindAll(string modID)
        {
            return _databaseService.DB.Table<BackupEntry>().Where(e => e.ModID == modID).AsEnumerable();
        }

        /// <summary>
        /// Stores entry to database
        /// </summary>
        private void Store(BackupEntry entry)
        {
            // Check if already exists
            if (Find(entry.OriginalFilePath, entry.ModID) != null) return;
            _databaseService.DB.Insert(entry);
        }

        /// <summary>
        /// Removes an entry from the database
        /// </summary>
        private void Remove(BackupEntry entry)
        {
            _databaseService.DB.Delete(entry);
        }

        /// <summary>
        /// Adds file to backup storage
        /// </summary>
        public void BackupFile(string filePath, string modID)
        {
            // Check settings
            if (!Settings.Stager.Current.UseBackup) return;

            // File must exist
            if (!File.Exists(filePath)) return;

            // Find entry or create new
            var entry = Find(filePath, modID);
            if (entry == null)
            {
                long installOrder = GetNextFreeInstallOrder(filePath);
                entry = new BackupEntry(filePath, installOrder, modID);
                Store(entry);
            }

            // Copy file
            string backupFilePath = Path.Combine(FileSystem.BackupStorageDirectory, entry.BackupFileName);
            Ext.MakeDirectoryAtPath(backupFilePath);
            File.Copy(filePath, backupFilePath, true);

            // Logging
            Logger.Record($"Made backup of file: {filePath}");
        }

        /// <summary>
        /// Add all files from directory to backup storage
        /// </summary>
        public void BackupDirectory(string dirPath, string modID)
        {
            // Directory must exist
            if (!Directory.Exists(dirPath)) return;

            // Enumerate all files
            foreach (string file in Directory.EnumerateFiles(dirPath, "*.*", SearchOption.AllDirectories))
                BackupFile(file, modID);
        }

        /// <summary>
        /// Restores file from backup if necessary
        /// <returns>True if any change was commited</returns>
        /// </summary>
        public bool RestoreFile(string filePath, string modID)
        {
            // Check settings
            if (!Settings.Stager.Current.UseBackup) return false;

            // Find backup for current mod
            var current = Find(filePath, modID);
            if (current == null) return false;

            // Find next order
            var higherOrder = FindHigherInstallOrder(filePath, current.InstallOrder);

            // Path to the current backup file
            string currentBackupFilePath = Path.Combine(FileSystem.BackupStorageDirectory, current.BackupFileName);
            if (!File.Exists(currentBackupFilePath)) return false;

            // If yes - move the current backup there
            if (higherOrder != null)
            {
                string higherOrderBackupFilePath = Path.Combine(FileSystem.BackupStorageDirectory,
                    higherOrder.BackupFileName);
                Ext.MakeDirectoryAtPath(higherOrderBackupFilePath);
                File.Copy(currentBackupFilePath, higherOrderBackupFilePath, true);
            }
            // If not - restore the file
            else
            {
                Ext.MakeDirectoryAtPath(filePath);
                File.Copy(currentBackupFilePath, filePath, true);
            }

            // Delete the backup
            File.Delete(Path.Combine(FileSystem.BackupStorageDirectory, current.BackupFileName));
            Remove(current);

            // Logging
            Logger.Record($"Restored file from backup: {filePath}");

            return true;
        }

        /// <summary>
        /// Restores all backups made by given mod
        /// </summary>
        public void RestoreAll(string modID)
        {
            foreach (var entry in FindAll(modID))
                RestoreFile(entry.OriginalFilePath, modID);
        }

        /// <summary>
        /// Removes all backups by given mod
        /// </summary>
        public void DropAll(string modID)
        {
            foreach (var entry in FindAll(modID))
            {
                Remove(entry);
                string backupFile = Path.Combine(FileSystem.BackupStorageDirectory, entry.BackupFileName);
                if (File.Exists(backupFile)) File.Delete(backupFile);
            }

            // Logging
            Logger.Record($"Dropped all backups for mod #{modID}");
        }
    }
}