// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <AuthorInfo.cs>
//  Created By: Alexey Golub
//  Date: 16/02/2016
// ------------------------------------------------------------------ 

using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace Modboy.Models.API
{
    public class AuthorInfo : ObservableObject
    {
        private string _id;
        private string _name;

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

        public override string ToString()
        {
            return Name;
        }
    }
}