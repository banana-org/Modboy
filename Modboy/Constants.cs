// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <Constants.cs>
//  Created By: Alexey Golub
//  Date: 13/02/2016
// ------------------------------------------------------------------ 

using System.Diagnostics.CodeAnalysis;

namespace Modboy
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Constants
    {
        public const int ExceptionReportLogDumpLines = 100;
        public const string ProtocolHandle = "modboy://";
        public const string UniformSeparator = "|";
        public static readonly string UserAgent = "Modboy (ver " + App.CurrentVersion + ")";

        public static readonly string URLProfilePage = "http://gamebanana.com/members/{0}";
        public static readonly string URLAboutPage = "http://gamebanana.com/wikis?page=modboy_client";
        public static readonly string URLHelp = "http://gamebanana.com/wikis?page=modboy_help";
        public static readonly string URLInstalledMods = "http://dev.gamebanana.com/members/modboy/installed/{0}";
        public static readonly string URLRegisterAccount = "http://gamebanana.com/members/account/register";
        public static readonly string URLResetPassword = "http://gamebanana.com/members/account/resetpassword/request";
        public static readonly string URLSubmitBugReport = "http://gamebanana.com/bugs/add?name=Modboy+Bug";
    }
}