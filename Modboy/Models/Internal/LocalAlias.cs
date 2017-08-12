// -------------------------------------------------------------------------
// Solution: GameBananaClient
// Project: Modboy
// File: LocalAlias.cs
// 
// Created by: Tyrrrz
// On: 24.09.2016
// -------------------------------------------------------------------------

using GalaSoft.MvvmLight;

namespace Modboy.Models.Internal
{
    public class LocalAlias : ObservableObject
    {
        private string _contextID;
        private string _keyword;
        private string _value;

        public string ContextID
        {
            get { return _contextID; }
            set { Set(ref _contextID, value); }
        }

        public string Keyword
        {
            get { return _keyword; }
            set { Set(ref _keyword, value); }
        }

        public string Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }

        public LocalAlias(string contextID, string keyword, string value)
        {
            ContextID = contextID;
            Keyword = keyword.ToUpperInvariant().Trim();
            Value = value;
        }

        public LocalAlias() { }
    }
}