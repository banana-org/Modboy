// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <App.xaml.cs>
//  Created By: Alexey Golub
//  Date: 13/02/2016
// ------------------------------------------------------------------ 

using GalaSoft.MvvmLight.Threading;
using NegativeLayer.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using Modboy.Models.Internal;

namespace Modboy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static Mutex _identityMutex;
        private static bool _isElevated;

        /// <summary>
        /// Gets or sets whether the app initialized successfully
        /// </summary>
        private static bool IsInitialized
        {
            get
            {
                const string mutexName = "Modboy_IdentityMutex";
                return Mutex.TryOpenExisting(mutexName, out _identityMutex);
            }
            set
            {
                const string mutexName = "Modboy_IdentityMutex";
                if (value && !IsInitialized)
                    _identityMutex = new Mutex(true, mutexName);
                else
                    _identityMutex?.ReleaseMutex();
            }
        }

        /// <summary>
        /// Current version of the app
        /// </summary>
        public static Version CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version;

        static App()
        {
            DispatcherHelper.Initialize();
        }

        /// <summary>
        /// Parse the command line arguments and take appropriate actions
        /// </summary>
        private static void ParseArguments(string[] args)
        {
            if (!args.AnySafe()) return;

            bool launchedViaProtocolHandle = args[0].ContainsInvariant(Constants.ProtocolHandle);

            // Launched via protocol handle - parse task
            if (launchedViaProtocolHandle)
            {
                // Extract the input from the arguments
                var regex = new Regex(Constants.ProtocolHandle + "(.+)",
                    RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                string input = regex.Match(args[0]).Groups[1].Value;

                // Get the valuable data
                string taskType = input.SubstringUntil("/");
                string modID = input.SubstringAfter("/");

                // Create directory if necessary
                if (!Directory.Exists(FileSystem.StorageDirectory))
                    Directory.CreateDirectory(FileSystem.StorageDirectory);

                // Append to buffer
                using (var fs = Ext.RetryOpenForWrite(FileSystem.TaskBufferFilePath, FileMode.Append))
                using (var sw = new StreamWriter(fs))
                    sw.WriteLine($"{taskType}{Constants.UniformSeparator}{modID}");
            }
            // Launched separately - parse special commands
            else
            {
                // Check for special commands
                _isElevated = args[0].EqualsInvariant("ELEVATED");
            }
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Logger.Record($"Initial startup (version: {CurrentVersion})");

            // Parse the cmd arguments
            ParseArguments(e.Args);

            Logger.Record("Parsed arguments");

            // Close if running already
            if (IsInitialized)
            {
                Current.Shutdown((int) ExitCode.AlreadyRunning);
                return;
            }

#if !DEBUG
            // Re-start if not running as administrator already
            if (!_isElevated)
            {
                Logger.Record("Restarting as admin");

                // Re-start as admin
                var startInfo = new ProcessStartInfo
                {
                    FileName = Assembly.GetEntryAssembly().Location,
                    Arguments = "ELEVATED",
                    UseShellExecute = true,
                    Verb = "runas"
                };
                new Action(() => Process.Start(startInfo)).Try();

                // Close this instance
                Current.Shutdown((int) ExitCode.NotElevated);
                return;
            }
#endif
            // Confirm that the app is running
            IsInitialized = true;

            // Delete temp files
            FileSystem.ClearTemporaryStorage();

            // Load settings, language
            Settings.Stager.TryLoad();
            Localization.Current.LoadLanguageFile(Settings.Stager.Current.Language);

            // Log launch
            Logger.Record("Started successfully");
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            FileSystem.ClearTemporaryStorage();
            Settings.Stager.Current.Save();

            // Log shutdown
            Logger.Record($"Application shut down ({e.ApplicationExitCode} = {e.ApplicationExitCode.ToEnumOrDefault<ExitCode>()})");
        }
    }
}