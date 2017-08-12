// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <ExitCode.cs>
//  Created By: Alexey Golub
//  Date: 08/03/2016
// ------------------------------------------------------------------ 

namespace Modboy.Models.Internal
{
    public enum ExitCode
    {
        None = 0,
        UnhandledException,
        UserExit,
        AlreadyRunning,
        NotElevated,
        UpdateInstallerExecuted,
    }
}