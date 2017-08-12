// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <CommandChangedArgs.cs>
//  Created By: Alexey Golub
//  Date: 17/08/2016
// ------------------------------------------------------------------ 

using Modboy.Models.API;

namespace Modboy.Models.EventArgs
{
    public class CommandChangedArgs : System.EventArgs
    {
        public Command NewCommand { get; }

        public CommandChangedArgs(Command newCommand)
        {
            NewCommand = newCommand;
        }
    }
}