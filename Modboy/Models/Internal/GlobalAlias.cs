// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <GlobalAlias.cs>
//  Created By: Alexey Golub
//  Date: 12/03/2016
// ------------------------------------------------------------------ 

using GalaSoft.MvvmLight;

namespace Modboy.Models.Internal
{
    public class GlobalAlias : ObservableObject
    {
        private string _keyword;
        private string _value;

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

        public GlobalAlias(string keyword, string value)
        {
            Keyword = keyword.ToUpperInvariant().Trim();
            Value = value;
        }

        public GlobalAlias() { }
    }
}