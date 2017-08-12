// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <UpdateVersionInfo.cs>
//  Created By: Alexey Golub
//  Date: 24/02/2016
// ------------------------------------------------------------------ 

using System;
using Newtonsoft.Json;

namespace Modboy.Models.API
{
    public class UpdateVersionInfo
    {
        [JsonProperty("LatestVersion")]
        public string VersionString { get; set; }

        public Version Version => Version.Parse(VersionString);

        [JsonProperty("DownloadUrl")]
        public string DownloadURL { get; set; }

        private UpdateVersionInfo() { }

        public override string ToString()
        {
            return VersionString;
        }
    }
}