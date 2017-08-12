// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <AuthService.cs>
//  Created By: Alexey Golub
//  Date: 26/03/2016
// ------------------------------------------------------------------ 

using System;
using System.ComponentModel;
using Modboy.Models.API;
using NegativeLayer.Extensions;

namespace Modboy.Services
{
    public class AuthService
    {
        private readonly TaskExecutionService _taskExecutionService;
        private readonly WindowService _windowService;
        private readonly APIService _apiService = new APIService();

        public AuthService(TaskExecutionService taskExecutionService, WindowService windowService)
        {
            _taskExecutionService = taskExecutionService;
            _windowService = windowService;
        }

        private void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(Settings.IsAuthorized))
                PerformAuthCheck();
        }

        /// <summary>
        /// Attempts to login with given credentials
        /// </summary>
        /// <returns>Null if failed, userID string if succeeded</returns>
        public string LogIn(AuthorizationInfo auth)
        {
            // Validate
            if (auth.Username.IsBlank() || auth.Password.IsBlank())
                return null;

            // Spell check the auth object
            auth.Username = auth.Username.Trim();

            try
            {
                // Try to log in
                if (!_apiService.CheckCredentials(auth))
                    return null;

                // Get user id
                return _apiService.GetUserID(auth.Username);
            }
            catch (Exception ex)
            {
                Logger.Record("Could not perform a login operation");
                Logger.Record(ex);

                return null;
            }
        }

        /// <summary>
        /// Checks whether the user is authorized and if not - prompts them to log in
        /// </summary>
        public void PerformAuthCheck()
        {
            // HACK: this is kind of a pattern-reversal
            if (Settings.Stager.Current.IsAuthorized)
            {
                _taskExecutionService.IsEnabled = true;
            }
            else
            {
                _taskExecutionService.IsEnabled = false;
                _windowService.ShowErrorWindowAsync(Localization.Current.Auth_NotLoggedIn).GetResult();
            }
        }

        public void Start()
        {
            // Events
            Settings.Stager.Current.PropertyChanged += OnSettingsPropertyChanged;

            // Initial
            PerformAuthCheck();
        }

        public void Stop()
        {
            // Events
            Settings.Stager.Current.PropertyChanged -= OnSettingsPropertyChanged;
        }
    }
}