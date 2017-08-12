// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <ArchivingService.cs>
//  Created By: Alexey Golub
//  Date: 13/02/2016
// ------------------------------------------------------------------ 

using System;
using System.IO;
using Modboy.Models.EventArgs;
using SharpCompress.Archives;
using SharpCompress.Readers;

namespace Modboy.Services
{
    public class ArchivingService
    {
        private double _progress;

        public double Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(value));
            }
        }

        /// <summary>
        /// Event that triggers when the progress changes
        /// </summary>
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        /// <summary>
        /// Function, that will be periodically executed to determine if an abort is issued
        /// </summary>
        public Func<bool> AbortChecker { get; set; } = () => false;

        /// <summary>
        /// Extracts all contents of the given archive file to the destination directory
        /// </summary>
        public void ExtractFiles(string archiveFilePath, string destinationDirectory)
        {
            Progress = 0;

            // Initialize
            if (!Directory.Exists(destinationDirectory))
                Directory.CreateDirectory(destinationDirectory);

            // Open reader for all entries
            using (var archive = ArchiveFactory.Open(archiveFilePath))
            {
                // Closure
                long archiveSize = new FileInfo(archiveFilePath).Length;

                // Extract
                foreach (var entry in archive.Entries)
                {
                    // Abort check
                    if (AbortChecker()) return;

                    // Extract file
                    try
                    {
                        var extractOptions = new ExtractionOptions {ExtractFullPath = true, Overwrite = true};
                        entry.WriteToDirectory(destinationDirectory, extractOptions);
                        Progress += 100.0 * entry.CompressedSize / archiveSize;
                    }
                    catch (Exception ex)
                    {
                        Logger.Record($"Could not unpack an entry from an archive ({entry})");
                        Logger.Record(ex);
                    }
                }
            }
        }
    }
}