// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <CommandExecutionService.cs>
//  Created By: Alexey Golub
//  Date: 14/02/2016
// ------------------------------------------------------------------ 

using NegativeLayer.Extensions;
using NegativeLayer.Extensions.Types;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Modboy.Models.API;
using Modboy.Models.EventArgs;
using Modboy.Models.Internal;

namespace Modboy.Services
{
    public class CommandExecutionService
    {
        private readonly BackupService _backupService;
        private readonly AliasService _aliasService;
        private readonly WindowService _windowService;

        private string _workingDir = Environment.CurrentDirectory;
        private Command _command;
        private string _contextID;

        /// <summary>
        /// Event that triggers when the service started execution a new command
        /// </summary>
        public event EventHandler<CommandChangedArgs> CommandChanged;

        /// <summary>
        /// Event that triggers when a file was affected by the execution of a command
        /// </summary>
        public event EventHandler<CommandFileAffectedArgs> FileChangeMade;

        public CommandExecutionService(BackupService backupService, AliasService aliasService,
            WindowService windowService)
        {
            _backupService = backupService;
            _aliasService = aliasService;
            _windowService = windowService;
        }

        /// <summary>
        /// If the given string is a relative path, returns full path, otherwise returns the same string
        /// </summary>
        private string GetFullPath(string path)
        {
            if (path.IsBlank()) return path;
            return Path.IsPathRooted(path) ? path : Path.Combine(_workingDir, path);
        }

        #region Executions
        private bool ExecutePrompt(string message)
        {
            return _windowService.ShowPromptWindowAsync(message).GetResult();
        }

        private bool ExecutePromptPathAlias(bool isGlobal, bool isFile, string aliasKeyword, string aliasDisplayName,
            string[] defaultPaths)
        {
            // Parameter checks
            if (aliasKeyword.IsBlank())
                return false;
            if (aliasDisplayName.IsBlank())
                aliasDisplayName = aliasKeyword;

            // Check if alias is already known
            if (_aliasService.IsKnown(aliasKeyword))
                return true;

            // Check if any of the default paths exist
            if (defaultPaths.AnySafe())
            {
                // Find the first default path that exists
                string validDefaultPath = null;
                foreach (string defaultPath in defaultPaths)
                {
                    string absolutePath = Ext.GetPossibleAbsolutePaths(defaultPath).FirstOrDefault();
                    if (absolutePath.IsNotBlank())
                    {
                        validDefaultPath = absolutePath;
                        break;
                    }
                }

                // If found - set alias and return
                if (validDefaultPath.IsNotBlank())
                {
                    _aliasService.Set(new GlobalAlias(aliasKeyword, validDefaultPath));
                    return true;
                }
            }

            // Prompt the user and set alias
            var type = isFile ? AliasPromptType.File : AliasPromptType.Directory;
            string result = _windowService.ShowAliasPromptWindowAsync(type, aliasDisplayName).GetResult();
            if (result.IsNotBlank())
            {
                if (isGlobal)
                    _aliasService.Set(new GlobalAlias(aliasKeyword, result));
                else
                    _aliasService.Set(new LocalAlias(_contextID, aliasKeyword, result));
                return true;
            }

            return false;
        }

        private bool ExecutePromptStringAlias(bool isGlobal, string aliasKeyword, string aliasDisplayName)
        {
            // Parameter checks
            if (aliasKeyword.IsBlank())
                return false;
            if (aliasDisplayName.IsBlank())
                aliasDisplayName = aliasKeyword;

            // Check if alias is already known
            if (_aliasService.IsKnown(aliasKeyword))
                return true;

            // Prompt the user and set alias
            string result = _windowService.ShowAliasPromptWindowAsync(AliasPromptType.String, aliasDisplayName).GetResult();
            if (result.IsNotBlank())
            {
                if (isGlobal)
                    _aliasService.Set(new GlobalAlias(aliasKeyword, result));
                else
                    _aliasService.Set(new LocalAlias(_contextID, aliasKeyword, result));
                return true;
            }

            return false;
        }

        private bool ExecuteChangeDir(string path)
        {
            path = _aliasService.Process(path);

            // Parameter checks
            if (path.IsBlank()) return false;
            if (!Ext.IsValidDirectoryPath(path)) return false;

            // Change dir
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            _workingDir = path;

            // Set alias
            _aliasService.Set(new InternalAlias(InternalAliasKeyword.CurrentDirectory, _workingDir));

            return true;
        }

        private bool ExecuteCheckExists(string path)
        {
            path = _aliasService.Process(path);

            // Parameter checks
            if (path.IsBlank()) return false;

            path = GetFullPath(path);

            return File.Exists(path) || Directory.Exists(path);
        }

        private bool ExecuteCopy(string source, string destination)
        {
            source = _aliasService.Process(source);
            destination = _aliasService.Process(destination);

            // Parameter checks
            if (source.IsBlank() || destination.IsBlank()) return false;

            source = GetFullPath(source);
            destination = GetFullPath(destination);

            // Determine types of the given paths
            var sourceType = Ext.GetPathTargetType(source);
            var destinationType = Ext.GetPathTargetType(destination);

            if (sourceType != destinationType) return false;
            Ext.MakeDirectoryAtPath(destination);

            // If copying file
            if (sourceType == PathTargetType.File && File.Exists(source) &&
                Ext.IsValidFilePath(destination))
            {
                _backupService.BackupFile(destination, _contextID);

                File.Copy(source, destination, true);
                FileChangeMade?.Invoke(this, new CommandFileAffectedArgs(destination, _command));
                return true;
            }

            // If copying directory
            if (sourceType == PathTargetType.Directory && Directory.Exists(source) &&
                Ext.IsValidDirectoryPath(destination))
            {
                foreach (string file in Directory.EnumerateFiles(source))
                {
                    _backupService.BackupDirectory(destination, _contextID);

                    File.Copy(file, destination, true);
                    FileChangeMade?.Invoke(this, new CommandFileAffectedArgs(file, _command));
                }
                return true;
            }

            return false;
        }

        private bool ExecuteCmd(string cmd, string args)
        {
            cmd = _aliasService.Process(cmd);

            // Parameter checks
            if (cmd.IsBlank()) return false;

            // Create process object with or without command line arguments
            var process = args.IsNotBlank()
                ? Process.Start(cmd, args)
                : Process.Start(cmd);

            if (process == null) return false;
            process.WaitForExit();
            return true;
        }
        #endregion

        /// <summary>
        /// Run the command
        /// </summary>
        /// <returns>Whether or not it succeeded</returns>
        public bool ExecuteCommand(Command command, string contextID)
        {
            _command = command;
            CommandChanged?.Invoke(this, new CommandChangedArgs(_command));
            _contextID = contextID;
            bool success = false;

            if (_command.Type == CommandType.Prompt)
            {
                // Determine the type of the prompt
                string promptType = _command.Parameter("Type");

                // File
                if (promptType.EqualsInvariant("File"))
                {
                    success = ExecutePromptPathAlias(
                        _command.Parameter<bool>("IsGlobal"),
                        true,
                        _command.Parameter("AliasName"),
                        _command.Parameter("DisplayName"),
                        _command.ParameterArray("DefaultPaths"));
                }
                // Directory
                else if (promptType.EqualsInvariant("Dir"))
                {
                    success = ExecutePromptPathAlias(
                        _command.Parameter<bool>("IsGlobal"),
                        false,
                        _command.Parameter("AliasName"),
                        _command.Parameter("DisplayName"),
                        _command.ParameterArray("DefaultPaths"));
                }
                // String
                else if (promptType.EqualsInvariant("String"))
                {
                    success = ExecutePromptStringAlias(
                        _command.Parameter<bool>("IsGlobal"),
                        _command.Parameter("AliasName"),
                        _command.Parameter("DisplayName"));
                }
                // YesNo
                else
                {
                    success = ExecutePrompt(
                        _command.Parameter("Message"));
                }
            }
            else if (_command.Type == CommandType.ChangeDir)
            {
                success = ExecuteChangeDir(
                    _command.Parameter("Path"));
            }
            else if (_command.Type == CommandType.CheckExists)
            {
                success = ExecuteCheckExists(
                    _command.Parameter("Path"));
            }
            else if (_command.Type == CommandType.Copy)
            {
                string source = _command.Parameter("Source"); // is null if multiple
                var sources = _command.ParameterArray("Source"); // is null if single

                // Determine source
                if (sources.AnySafe())
                {
                    var sourcesTrimmed = sources.StripCommonStart().ToArray();
                    int sourceIndex =
                        _windowService.ShowComboSelectWindowAsync(Localization.Current.Command_Copy_SelectSource,
                            sourcesTrimmed).GetResult();
                    source = sources[sourceIndex];
                }

                success = ExecuteCopy(
                    source,
                    _command.Parameter("Destination"));
            }
            else if (_command.Type == CommandType.Execute)
            {
                success = ExecuteCmd(
                    _command.Parameter("Cmd"),
                    _command.Parameter("Args"));
            }

            // Logging
            var parametersPretty = _command.Parameters.Properties().Select(p => $"\t{p.Name}:{p.Value}");
            Logger.Record($"Command executed {(success ? "successfully" : "unsuccessfully")}" + Environment.NewLine +
                          $"Type: {_command.Type}" + Environment.NewLine +
                          $"Parameters:" + Environment.NewLine +
                          $"{parametersPretty.JoinToString(Environment.NewLine)}");

            return success;
        }
    }
}