// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <VerificationPair.cs>
//  Created By: Alexey Golub
//  Date: 22/02/2016
// ------------------------------------------------------------------ 

using Newtonsoft.Json;

namespace Modboy.Models.API
{
    public class VerificationPair
    {
        [JsonProperty("Destination")]
        public string File { get; set; }

        [JsonProperty("Checksum")]
        public string Hash { get; set; }

        public VerificationPair(string file, string hash)
        {
            File = file;
            Hash = hash;
        }
    }
}