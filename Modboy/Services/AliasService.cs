// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <AliasService.cs>
//  Created By: Alexey Golub
//  Date: 19/02/2016
// ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using Modboy.Models.Internal;
using NegativeLayer.Extensions;
using NegativeLayer.WPFExtensions;
using Newtonsoft.Json;

namespace Modboy.Services
{
    public class AliasService
    {
        private readonly object _lock = new object();

        public Dictionary<string, string> InternalAliases { get; } = new Dictionary<string, string>();
        public Dictionary<string, string> UserAliases { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> LocalAliases { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Event that triggers when user alises have been changed
        /// </summary>
        public event EventHandler UserAliasesChanged;

        public AliasService()
        {
            // Events
            DispatcherHelper.UIDispatcher.InvokeSafeAsync(() => Application.Current.Exit += (sender, args) => TrySave()).Forget();

            // Load
            TryLoad();
        }

        /// <summary>
        /// Populates global aliases from a list
        /// </summary>
        public void Populate(IEnumerable<GlobalAlias> globalAliases)
        {
            lock (_lock)
            {
                UserAliases.Clear();
                foreach (var alias in globalAliases)
                    Set(alias);
                UserAliasesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Sets an internal alias
        /// </summary>
        public void Set(InternalAlias alias)
        {
            lock (_lock)
            {
                InternalAliases.SetOrAdd(alias.Keyword, alias.Value);
            }
        }

        /// <summary>
        /// Sets a global alias
        /// </summary>
        public void Set(GlobalAlias alias)
        {
            lock (_lock)
            {
                UserAliases.SetOrAdd(alias.Keyword, alias.Value);
                UserAliasesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Sets a local alias
        /// </summary>
        public void Set(LocalAlias alias)
        {
            lock (_lock)
            {
                LocalAliases.SetOrAdd(alias.ContextID + "_" + alias.Keyword, alias.Value);
            }
        }

        /// <summary>
        /// Removes a global alias by keyword
        /// </summary>
        public void Remove(string keyword)
        {
            keyword = keyword.ToUpperInvariant().Trim();
            lock (_lock)
            {
                UserAliases.Remove(keyword);
                UserAliasesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Clears all global aliases
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                UserAliases.Clear();
            }
        }

        /// <summary>
        /// Clears all local aliases for the given context ID
        /// </summary>
        public void Clear(string contextID)
        {
            lock (_lock)
            {
                foreach (string key in LocalAliases.Keys.Where(k => k.StartsWith(contextID)).ToArray())
                    LocalAliases.Remove(key);
            }
        }

        /// <summary>
        /// Returns true if a global alias is known by the given keyword
        /// </summary>
        public bool IsKnown(string keyword)
        {
            lock (_lock)
            {
                keyword = keyword.ToUpperInvariant().Trim();
                return UserAliases.ContainsKey(keyword);
            }
        }

        /// <summary>
        /// Returns true if either a global or local alias is known by the given keyword
        /// </summary>
        public bool IsKnown(string keyword, string contextID)
        {
            lock (_lock)
            {
                keyword = keyword.ToUpperInvariant().Trim();
                return UserAliases.ContainsKey(keyword) || LocalAliases.ContainsKey(contextID + "_" + keyword);
            }
        }

        /// <summary>
        /// Takes a string and converts all alias keys to their value and then returns the resulting string
        /// </summary>
        public string Process(string input)
        {
            if (input.IsBlank()) return input;

            lock (_lock)
            {
                // Internal aliases
                foreach (var alias in InternalAliases)
                {
                    input = input.Replace(alias.Key, alias.Value);
                }

                // Global aliases
                foreach (var alias in UserAliases)
                {
                    input = input.Replace(alias.Key, alias.Value);
                }

                // Local aliases
                foreach (var alias in LocalAliases)
                {
                    string key = alias.Key.SubstringAfter("_");
                    input = input.Replace(key, alias.Value);
                }
            }

            return input;
        }

        /// <summary>
        /// Loads global aliases from file
        /// </summary>
        public void Load()
        {
            if (!File.Exists(FileSystem.AliasFilePath))
                return;

            lock (_lock)
            {
                string data = File.ReadAllText(FileSystem.AliasFilePath);
                UserAliases = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                UserAliasesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Loads global aliases from file and supresses all errors
        /// </summary>
        public void TryLoad()
        {
            try
            {
                Load();
            }
            catch (Exception ex)
            {
                Logger.Record("Could not load the aliases from a file");
                Logger.Record(ex);
            }
        }

        /// <summary>
        /// Saves global aliases to file
        /// </summary>
        public void Save()
        {
            lock (_lock)
            {
                string data = JsonConvert.SerializeObject(UserAliases);
                File.WriteAllText(FileSystem.AliasFilePath, data);
            }
        }

        /// <summary>
        /// Saves global aliases to file and supresses all errors
        /// </summary>
        public void TrySave()
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                Logger.Record("Could not save the aliases to a file");
                Logger.Record(ex);
            }
        }
    }
}