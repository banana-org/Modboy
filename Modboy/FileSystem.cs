// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <FileSystem.cs>
//  Created By: Alexey Golub
//  Date: 13/02/2016
// ------------------------------------------------------------------ 

using NegativeLayer.Extensions;
using System;
using System.IO;
using System.Reflection;

namespace Modboy
{
    public static class FileSystem
    {
        public static string ProgramFilesDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string StorageDirectory =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Modboy");

        public static string TempStorageDirectory => Settings.Stager.Current.TempDownloadPath;
        public static string BackupStorageDirectory => Settings.Stager.Current.BackupPath;
        public static string LanguagesStorageDirectory => Path.Combine(ProgramFilesDirectory, "Resources", "Languages");

        public static string DatabaseFilePath => Path.Combine(StorageDirectory, "Modboy.sqlite");
        public static string AliasFilePath => Path.Combine(StorageDirectory, "Aliases.dat");
        public static string TaskBufferFilePath => Path.Combine(StorageDirectory, "Buffer.dat");
        public static string LogFilePath => Path.Combine(StorageDirectory, "Trace.log");

        /// <summary>
        /// Creates a temporary directory and returns its path
        /// </summary>
        public static string CreateTempDirectory(string name = null)
        {
            if (name.IsBlank())
                name = DateTime.UtcNow.ToFileTimeUtc().ToString();
            return Path.Combine(TempStorageDirectory, name);
        }

        /// <summary>
        /// Creates a temporary file and returns its path
        /// </summary>
        public static string CreateTempFile(string name = null)
        {
            if (name.IsBlank())
                name = DateTime.UtcNow.ToFileTimeUtc() + ".tmp";
            if (Path.GetExtension(name).IsBlank())
                name += ".tmp";
            return Path.Combine(TempStorageDirectory, name);
        }

        /// <summary>
        /// Removes temporary files
        /// </summary>
        public static void ClearTemporaryStorage()
        {
            try
            {
                Directory.Delete(TempStorageDirectory, true);
            }
            catch
            {
                // Ignored
            }
        }
    }
}