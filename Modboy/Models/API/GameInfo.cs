// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <GameInfo.cs>
//  Created By: Alexey Golub
//  Date: 16/02/2016
// ------------------------------------------------------------------ 

using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace Modboy.Models.API
{
    public class GameInfo : ObservableObject
    {
        private string _id;
        private string _name;
        private string _abbreviation;

        [JsonProperty("Id")]
        public string ID
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        [JsonProperty("Name")]
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        [JsonProperty("Abbreviation")]
        public string Abbreviation
        {
            get { return _abbreviation; }
            set { Set(ref _abbreviation, value); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}