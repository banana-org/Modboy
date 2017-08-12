// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <ModStatus.cs>
//  Created By: Alexey Golub
//  Date: 02/03/2016
// ------------------------------------------------------------------ 

using GalaSoft.MvvmLight;
using Modboy.Models.API;
using Modboy.Models.Database;

namespace Modboy.Models.Internal
{
    public class ModStatus : ObservableObject
    {
        private ModStatusState _state = ModStatusState.Idle;
        private InstalledModEntry _installedModEntry;
        private ModInfo _modInfo;
        private bool _isExpanded;
        private string _statusText;
        private double _statusProgress;
        private bool _isStatusProgressIndeterminate;
        private bool _isStatusVisible;
        private bool _isAbortVisible;
        private bool _isReinstallVisible;
        private bool _isUninstallVisible;
        private bool _isAborted;

        public ModStatusState State
        {
            get { return _state; }
            set { Set(ref _state, value); }
        }

        public ModInfo ModInfo
        {
            get { return _modInfo; }
            set { Set(ref _modInfo, value); }
        }

        public InstalledModEntry InstalledModEntry
        {
            get { return _installedModEntry; }
            set { Set(ref _installedModEntry, value); }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { Set(ref _isExpanded, value); }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { Set(ref _statusText, value); }
        }

        public double StatusProgress
        {
            get { return _statusProgress; }
            set { Set(ref _statusProgress, value); }
        }

        public bool IsStatusProgressIndeterminate
        {
            get { return _isStatusProgressIndeterminate; }
            set { Set(ref _isStatusProgressIndeterminate, value); }
        }

        public bool IsStatusVisible
        {
            get { return _isStatusVisible; }
            set { Set(ref _isStatusVisible, value); }
        }

        public bool IsAbortVisible
        {
            get { return _isAbortVisible; }
            set { Set(ref _isAbortVisible, value); }
        }

        public bool IsReinstallVisible
        {
            get { return _isReinstallVisible; }
            set { Set(ref _isReinstallVisible, value); }
        }

        public bool IsUninstallVisible
        {
            get { return _isUninstallVisible; }
            set { Set(ref _isUninstallVisible, value); }
        }

        public bool IsAborted
        {
            get { return _isAborted; }
            set { Set(ref _isAborted, value); }
        }

        public ModStatus(ModInfo modInfo, InstalledModEntry installedModEntry)
        {
            ModInfo = modInfo;
            InstalledModEntry = installedModEntry;
        }

        public ModStatus(string modID)
            : this(new ModInfo(modID), new InstalledModEntry(modID))
        { }

        public override string ToString()
        {
            return ModInfo.ToString();
        }
    }
}