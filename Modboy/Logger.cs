// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <Logger.cs>
//  Created By: Alexey Golub
//  Date: 14/02/2016
// ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NegativeLayer.Extensions;
using Newtonsoft.Json;

namespace Modboy
{
    public static class Logger
    {
        private static readonly TaskFactory TaskFactory = new TaskFactory(TaskCreationOptions.PreferFairness,
            TaskContinuationOptions.PreferFairness);
        private static readonly object Lock = new object();
        private static readonly Queue<string> Queue = new Queue<string>();
        private static bool _busy;

        static Logger()
        {
            // Keep only the log files from today and delete all others
            lock (Lock)
            {
                var fileInfo = new FileInfo(FileSystem.LogFilePath);
                if (fileInfo.Exists && fileInfo.LastWriteTimeUtc.DayOfYear != DateTime.UtcNow.DayOfYear)
                    fileInfo.Delete();
            }
        }

        private static void DoWork()
        {
            lock (Lock)
            {
                _busy = true;
                while (Queue.AnySafe())
                {
                    string entry = Queue.Dequeue();
                    if (entry.IsNotBlank())
                        InternalRecord(entry);
                }
                _busy = false;
            }
        }

        
        private static void InternalRecord(string entry)
        {
            // Compose the string
            string data = ">>" + DateTime.Now + " | " + entry + Environment.NewLine + Environment.NewLine;

            // Create directory
            if (!Directory.Exists(FileSystem.StorageDirectory))
                Directory.CreateDirectory(FileSystem.StorageDirectory);

            // Output to file
            File.AppendAllText(FileSystem.LogFilePath, data);
        }

        /// <summary>
        /// Record an entry to log file
        /// </summary>
        public static void Record(string entry)
        {
            lock (Lock)
            {
                Queue.Enqueue(entry);
                if (!_busy) TaskFactory.StartNew(DoWork);
            }
        }

        /// <summary>
        /// Record an entry to log file
        /// </summary>
        public static void Record(Exception exception)
        {
            // Serialize and dump the exception
            string serialized = JsonConvert.SerializeObject(exception, Formatting.Indented);
            string filePath = Path.Combine(FileSystem.StorageDirectory,
                $"{DateTime.UtcNow.ToFileTimeUtc()}_{exception.GetType().Name}.log");
            File.WriteAllText(filePath, serialized);

            // Log
            string data = $"Exception encountered and dumped to {filePath}";
            Record(data);
        }

        /// <summary>
        /// Record an entry to log file
        /// </summary>
        public static void Record(object obj)
        {
            Record(obj.ToString());
        }

        /// <summary>
        /// Get the log content dump
        /// </summary>
        /// <returns></returns>
        public static string GetLogDump()
        {
            return File.Exists(FileSystem.LogFilePath)
                ? File.ReadAllText(FileSystem.LogFilePath)
                : "";
        }
    }
}