// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <TaskExecutionStatus.cs>
//  Created By: Alexey Golub
//  Date: 09/03/2016
// ------------------------------------------------------------------ 

namespace Modboy.Models.Internal
{
    public enum TaskExecutionStatus
    {
        None,

        Verify,
        VerifyGetModInfo,
        VerifyGetAffectedFiles,
        VerifyGetVerificationPairs,
        VerifyExecute,

        Install,
        InstallGetModInfo,
        InstallDownload,
        InstallUnpack,
        InstallExecute,
        InstallStoreResults,
        InstallSubmitResults,

        Uninstall,
        UninstallGetModInfo,
        UninstallGetAffectedFiles,
        UninstallDeleteFiles,
        UninstallRestoreBackups,
        UninstallStoreResults,
        UninstallSubmitResults
    }
}