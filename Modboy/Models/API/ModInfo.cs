// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <ModInfo.cs>
//  Created By: Alexey Golub
//  Date: 13/02/2016
// ------------------------------------------------------------------ 

using System;
using GalaSoft.MvvmLight;
using Modboy.Models.Converters;
using Newtonsoft.Json;

namespace Modboy.Models.API
{
    public class ModInfo : ObservableObject
    {
        private string _modID;
        private GameInfo _gameInfo = new GameInfo();
        private AuthorInfo _authorInfo = new AuthorInfo();
        private string _description;
        private string _name;
        private DateTime _timeAdded;
        private string _imageURL;
        private string _downloadURL;
        private string _pageURL;

        [JsonProperty("ID")]
        public string ModID
        {
            get { return _modID; }
            set { Set(ref _modID, value); }
        }

        [JsonProperty("Game")]
        public GameInfo GameInfo
        {
            get { return _gameInfo; }
            set { Set(ref _gameInfo, value); }
        }

        [JsonProperty("Submitter")]
        public AuthorInfo AuthorInfo
        {
            get { return _authorInfo; }
            set { Set(ref _authorInfo, value); }
        }

        [JsonProperty("Description")]
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }

        [JsonProperty("Name")]
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        [JsonProperty("TimeAdded")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime TimeAdded
        {
            get { return _timeAdded; }
            set { Set(ref _timeAdded, value); }
        }

        [JsonProperty("ThumbnailUrl")]
        public string ImageURL
        {
            get { return _imageURL; }
            set { Set(ref _imageURL, value); }
        }

        [JsonProperty("DownloadUrl")]
        public string DownloadURL
        {
            get { return _downloadURL; }
            set { Set(ref _downloadURL, value); }
        }

        [JsonProperty("ProfileUrl")]
        public string PageURL
        {
            get { return _pageURL; }
            set { Set(ref _pageURL, value); }
        }

        public ModInfo() { }

        public ModInfo(string modID)
        {
            ModID = modID;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}