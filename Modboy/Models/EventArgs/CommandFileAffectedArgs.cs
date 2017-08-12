// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <CommandFileAffectedArgs.cs>
//  Created By: Alexey Golub
//  Date: 08/03/2016
// ------------------------------------------------------------------ 

using Modboy.Models.API;

namespace Modboy.Models.EventArgs
{
    public class CommandFileAffectedArgs : System.EventArgs
    {
        public string FilePath { get; }
        public Command Command { get; }

        public CommandFileAffectedArgs(string filePath, Command command)
        {
            FilePath = filePath;
            Command = command;
        }
    }
}