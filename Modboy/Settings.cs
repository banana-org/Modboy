// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <Settings.cs>
//  Created By: Alexey Golub
//  Date: 14/02/2016
// ------------------------------------------------------------------ 

using System;
using System.IO;
using NegativeLayer.Extensions;
using NegativeLayer.Settings;

namespace Modboy
{
    public class Settings : SettingsManager
    {
        public static SettingsManagerStager<Settings> Stager { get; } = new SettingsManagerStager<Settings>();

        #region Properties

        private bool _isAuthorized;
        private string _userID;
        private string _userName;
        private string _language;
        private string _computerName;
        private string _tempDownloadPath;
        private bool _useBackup;
        private string _backupPath;
        private bool _showNotifications;
        private bool _autoUpdate;

        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            private set { Set(ref _isAuthorized, value); }
        }

        public string UserID
        {
            get { return _userID; }
            set
            {
                IsAuthorized = value.IsNotBlank();
                Set(ref _userID, value);
            }
        }

        public string UserName
        {
            get { return _userName; }
            set { Set(ref _userName, value); }
        }

        public string Language
        {
            get { return _language; }
            set { Set(ref _language, value); }
        }

        public string ComputerName
        {
            get { return _computerName; }
            set {Set(ref _computerName, value); }
        }

        public string TempDownloadPath
        {
            get { return _tempDownloadPath; }
            set { Set(ref _tempDownloadPath, value); }
        }

        public bool UseBackup
        {
            get { return _useBackup; }
            set { Set(ref _useBackup, value); }
        }

        public string BackupPath
        {
            get { return _backupPath; }
            set { Set(ref _backupPath, value); }
        }

        public bool ShowNotifications
        {
            get { return _showNotifications; }
            set { Set(ref _showNotifications, value); }
        }

        public bool AutoUpdate
        {
            get { return _autoUpdate; }
            set { Set(ref _autoUpdate, value); }
        }

        #endregion

        public Settings()
        {
            // Defaults
            _language = Localization.DefaultLanguage;
            _computerName = Environment.MachineName;
            _tempDownloadPath = Path.Combine(FileSystem.StorageDirectory, "Temp");
            _useBackup = true;
            _backupPath = Path.Combine(FileSystem.StorageDirectory, "Backups");
            _autoUpdate = true;
            _showNotifications = true;
        }
    }
}