// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <UnhandledExceptionService.cs>
//  Created By: Alexey Golub
//  Date: 28/06/2016
// ------------------------------------------------------------------ 

using System;
using System.Windows;
using Modboy.Models.Internal;
using NegativeLayer.Extensions;
using NegativeLayer.WPFExtensions;

namespace Modboy.Services
{
    public class UnhandledExceptionService
    {
        private bool _shouldShowBugReportWindow;

        public UnhandledExceptionService(WindowService windowService)
        {
            // Register global exception handling
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var ex = (Exception) e.ExceptionObject;

                Logger.Record("Unhandled exception occured");
                Logger.Record(ex);

                if (_shouldShowBugReportWindow)
                    windowService.ShowBugReportWindowAsync(ex).GetResult();

                Application.Current.ShutdownSafe(ExitCode.UnhandledException);
            };
        }

        public void Start()
        {
            _shouldShowBugReportWindow = true;
        }

        public void Stop()
        {
            _shouldShowBugReportWindow = false;
        }
    }
}