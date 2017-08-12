// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <Localization.cs>
//  Created By: Alexey Golub
//  Date: 25/02/2016
// ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.IO;
using GalaSoft.MvvmLight;
using Modboy.Models.Database;
using Modboy.Models.Internal;
using NegativeLayer.Extensions;
using Newtonsoft.Json;

namespace Modboy
{
    public class Localization : ObservableObject
    {
        public const string DefaultLanguage = "English";

        /// <summary>
        /// Localization service that is currently in use
        /// </summary>
        public static Localization Current { get; } = new Localization();

        /// <summary>
        /// Get the list of available languages
        /// </summary>
        public static IEnumerable<string> GetAvailableLanguages()
        {
            // Default language
            yield return DefaultLanguage;

            // Custom files
            if (!Directory.Exists(FileSystem.LanguagesStorageDirectory))
                yield break;
            foreach (string languageFile in Directory.EnumerateFiles(FileSystem.LanguagesStorageDirectory, "*.lng"))
            {
                string name = Path.GetFileNameWithoutExtension(languageFile);
                yield return name;
            }
        }

        /// <summary>
        /// Gets the file, defining given language or null if not found
        /// </summary>
        public static string ResolveLanguageFilePath(string language)
        {
            string path = Path.Combine(FileSystem.LanguagesStorageDirectory, language + ".lng");
            return !File.Exists(path) ? null : path;
        }

        #region Language Strings
        // ReSharper disable InconsistentNaming
        public string LocalizationStringNotFound { get; set; } = "[STR_NOT_FOUND]";

        public string Global_OK { get; set; } = "OK";
        public string Global_Cancel { get; set; } = "Cancel";
        public string Global_Yes { get; set; } = "Yes";
        public string Global_No { get; set; } = "No";

        public string Auth_NotLoggedIn { get; set; } =
            "You are not logged in. Please log in to your GameBanana account to use Modboy. Proceed to Settings to authorize.";

        public string Command_Copy_SelectSource { get; set; } = "Select the copy source from one of the options";

        public string Task_Verify { get; set; } = "Verifying...";
        public string Task_Verify_GetVerificationPairs { get; set; } = "Obtaining verification pairs...";
        public string Task_Verify_Execute { get; set; } = "Verifying files...";
        public string Task_Install { get; set; } = "Installing...";
        public string Task_Install_Download { get; set; } = "Downloading...";
        public string Task_Install_Unpack { get; set; } = "Unpacking...";
        public string Task_Install_Execute { get; set; } = "Executing installation...";
        public string Task_Uninstall { get; set; } = "Uninstalling...";
        public string Task_GetModInfo { get; set; } = "Obtaining mod information...";
        public string Task_GetAffectedFiles { get; set; } = "Gathering affected files...";
        public string Task_Uninstall_DeleteFiles { get; set; } = "Deleting files...";
        public string Task_Uninstall_RestoreBackups { get; set; } = "Restoring backups...";
        public string Task_StoreResults { get; set; } = "Storing results...";
        public string Task_SubmitResults { get; set; } = "Submitting results...";
        public string Task_PromptUninstall { get; set; } = "Are you sure you want to uninstall this mod?";
        public string Task_PromptReinstall { get; set; } = "Are you sure you want to reinstall this mod?";
        public string Task_CommandExecutionFailed { get; set; } = "Failed to execute crucial installation command. The task cannot continue";

        public string About_Title { get; set; } = "About Modboy";
        public string About_MoreInfo { get; set; } = "More info...";

        public string ComboSelect_Title { get; set; } = "Modboy - Select one of the options";

        public string History_TaskCompletedSuccessfully { get; set; } = "Task [{0}] for mod [{1}] completed successfully";
        public string History_TaskCompletedUnsuccessfully { get; set; } = "Task [{0}] for mod [{1}] failed";
        public string History_Date { get; set; } = "Date";
        public string History_Event { get; set; } = "Event";
        public string History_Clear { get; set; } = "Clear History";

        public string Main_Overview { get; set; } = "Mods";
        public string Main_History { get; set; } = "History";
        public string Main_Settings { get; set; } = "Settings";
        public string Main_LoggedInAs { get; set; } = "Logged in as";
        public string Main_TrayShowHide { get; set; } = "Show/Hide";
        public string Main_TrayAbout { get; set; } = "About";
        public string Main_TrayHelp { get; set; } = "Help";
        public string Main_TraySubmitBugReport { get; set; } = "Submit Bug Report";
        public string Main_TrayExit { get; set; } = "Exit";

        public string Overview_NameFilter { get; set; } = "Name";
        public string Overview_GameFilter { get; set; } = "Game";
        public string Overview_Sorting { get; set; } = "Sorting";
        public string Overview_InstallDate { get; set; } = "Installed on";
        public string Overview_InQueue { get; set; } = "In queue";
        public string Overview_Installed { get; set; } = "Installed";
        public string Overview_OpenModPage { get; set; } = "Mod Profile";
        public string Overview_Abort { get; set; } = "Abort";
        public string Overview_Verify { get; set; } = "Verify";
        public string Overview_Reinstall { get; set; } = "Update";
        public string Overview_Uninstall { get; set; } = "Remove";
        public string Overview_VerifySuccessfulNotification { get; set; } = "Verification successful, mod installed correctly";
        public string Overview_VerifyUnsuccessfulNotification { get; set; } = "Verification unsuccessful, mod is either corrupt or not installed";

        public string ModSortingMethod_Name { get; set; } = "Name";
        public string ModSortingMethod_AuthorName { get; set; } = "Author";
        public string ModSortingMethod_GameName { get; set; } = "Game";
        public string ModSortingMethod_InstallDate { get; set; } = "Install Date";

        public string Prompt_Title { get; set; } = "Modboy - Prompt";
        public string Prompt_InvalidFile { get; set; } = "Selected file path is invalid";
        public string Prompt_InvalidDir { get; set; } = "Selected directory path is invalid";

        public string Settings_Authorization { get; set; } = "Authorization";
        public string Settings_General { get; set; } = "General";
        public string Settings_Aliases { get; set; } = "Aliases";
        public string Settings_Username { get; set; } = "Username";
        public string Settings_Password { get; set; } = "Password";
        public string Settings_LogIn { get; set; } = "Log In";
        public string Settings_LogOut { get; set; } = "Log Out";
        public string Settings_Language { get; set; } = "Language";
        public string Settings_ComputerName { get; set; } = "Computer Name";
        public string Settings_TempDownloadPath { get; set; } = "Temporary Download Path";
        public string Settings_UseBackup { get; set; } = "Backup overwritten/deleted files";
        public string Settings_BackupPath { get; set; } = "Backup Path";
        public string Settings_ShowNotifications { get; set; } = "Show tray notifications";
        public string Settings_AutoUpdate { get; set; } = "Automatically check for Modboy updates";
        public string Settings_SendBugReports { get; set; } = "Send bug reports";
        public string Settings_AliasKeyword { get; set; } = "Alias Keyword";
        public string Settings_AliasValue { get; set; } = "Alias Value";
        public string Settings_Save { get; set; } = "Save";
        public string Settings_Reset { get; set; } = "Reset";
        public string Settings_Cancel { get; set; } = "Cancel";
        public string Settings_NeedSaveSettingsPrompt { get; set; } = "This action will save your settings. Continue?";
        public string Settings_AuthFailed { get; set; } = "Failed to log in! Please check your credentials";

        public string Updater_Title { get; set; } = "Modboy - Updater";
        public string Updater_Status { get; set; } = "Updating...";
        public string Updater_CheckFailed { get; set; } = "Could not check for updates!";
        public string Updater_UpdateDownloadFailed { get; set; } = "Could not download updates!";
        public string Updater_UpdateInstallFailed { get; set; } = "Could not install updates!";
        public string Updater_UpdatePrompt { get; set; } = "New version of Modboy has been found. Do you want to update now?";

        public string BugReport_Title { get; set; } = "Modboy - Bug Reporter";
        public string BugReport_Description { get; set; } =
            "Sorry, Modboy has encountered an error from which it could not recover. If you were installing or uninstalling mods, they might become corrupt. Help us fix this error by sending us debug information.";
        public string BugReport_Message { get; set; } = "Additional information";
        public string BugReport_IncludeLogs { get; set; } = "Include anonymous log files";
        public string BugReport_IncludeDB { get; set; } = "Include local mod database";
        public string BugReport_MailBack { get; set; } = "Your email";
        public string BugReport_SendReport { get; set; } = "Send";
        public string BugReport_DiscardReport { get; set; } = "Don't Send";
        public string BugReport_NoMessageSpecified { get; set; } = "Please specify a detailed message to describe the error you're receiving.";
        public string BugReport_NoMailBackSpecified { get; set; } =
            "You haven't specified your email address. Because of this, we will not be able to contact you, in case we need further information regarding your report. " +
            "Are you sure you want to send the report without contact email address?";
        public string BugReport_ReportSent { get; set; } = "Report has been sent. We will try to fix this as fast as we can. Sorry for the inconvinience.";

        // ReSharper restore InconsistentNaming
        #endregion

        public Localization()
        {
            // Load new localization when settings change
            Settings.Stager.Current.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Settings.Language))
                    LoadLanguageFile(Settings.Stager.Current.Language);
            };
        }

        /// <summary>
        /// Loads the specified language file by language name
        /// </summary>
        public void LoadLanguageFile(string language)
        {
            // Is default language
            if (language == DefaultLanguage)
            {
                JsonConvert.PopulateObject(JsonConvert.SerializeObject(new Localization()), this);
            }
            // Is custom language
            else
            {
                // Find file
                string filePath = ResolveLanguageFilePath(language);
                if (filePath.IsBlank()) return;
                if (!File.Exists(filePath)) return;

                // Load data
                string data = File.ReadAllText(filePath);
                JsonConvert.PopulateObject(data, this);
            }

            // Raise a change event for all properties
            // ReSharper disable once ExplicitCallerInfoArgument
            RaisePropertyChanged(string.Empty);
        }

        public string Localize(TaskExecutionStatus value)
        {
            switch (value)
            {
                case TaskExecutionStatus.None:
                    return string.Empty;
                case TaskExecutionStatus.Verify:
                    return Task_Verify;
                case TaskExecutionStatus.VerifyGetModInfo:
                    return Task_GetModInfo;
                case TaskExecutionStatus.VerifyGetAffectedFiles:
                    return Task_GetAffectedFiles;
                case TaskExecutionStatus.VerifyGetVerificationPairs:
                    return Task_Verify_GetVerificationPairs;
                case TaskExecutionStatus.VerifyExecute:
                    return Task_Verify_Execute;
                case TaskExecutionStatus.Install:
                    return Task_Install;
                case TaskExecutionStatus.InstallGetModInfo:
                    return Task_GetModInfo;
                case TaskExecutionStatus.InstallDownload:
                    return Task_Install_Download;
                case TaskExecutionStatus.InstallUnpack:
                    return Task_Install_Unpack;
                case TaskExecutionStatus.InstallExecute:
                    return Task_Install_Execute;
                case TaskExecutionStatus.InstallStoreResults:
                    return Task_StoreResults;
                case TaskExecutionStatus.InstallSubmitResults:
                    return Task_SubmitResults;
                case TaskExecutionStatus.Uninstall:
                    return Task_Uninstall;
                case TaskExecutionStatus.UninstallGetModInfo:
                    return Task_GetModInfo;
                case TaskExecutionStatus.UninstallGetAffectedFiles:
                    return Task_GetAffectedFiles;
                case TaskExecutionStatus.UninstallDeleteFiles:
                    return Task_Uninstall_DeleteFiles;
                case TaskExecutionStatus.UninstallRestoreBackups:
                    return Task_Uninstall_RestoreBackups;
                case TaskExecutionStatus.UninstallStoreResults:
                    return Task_StoreResults;
                case TaskExecutionStatus.UninstallSubmitResults:
                    return Task_SubmitResults;
                default:
                    return LocalizationStringNotFound;
            }
        }

        public string Localize(OverviewSortingMethod value)
        {
            switch (value)
            {
                case OverviewSortingMethod.Name:
                    return ModSortingMethod_Name;
                case OverviewSortingMethod.AuthorName:
                    return ModSortingMethod_AuthorName;
                case OverviewSortingMethod.GameName:
                    return ModSortingMethod_GameName;
                case OverviewSortingMethod.InstallDate:
                    return ModSortingMethod_InstallDate;
                default:
                    return LocalizationStringNotFound;
            }
        }

        public string Localize(HistoryEntry value)
        {
            // Get string
            string str = value.Success
                ? History_TaskCompletedSuccessfully
                : History_TaskCompletedUnsuccessfully;

            // Format it
            return str.Format(value.TaskType, value.ModName);
        }
    }
}