// -------------------------------------------------------------------------
// Solution: GameBananaClient
// Project: Modboy
// File: InternalAlias.cs
// 
// Created by: Tyrrrz
// On: 24.09.2016
// -------------------------------------------------------------------------

using GalaSoft.MvvmLight;

namespace Modboy.Models.Internal
{
    public class InternalAlias : ObservableObject
    {
        private InternalAliasKeyword _internalKeyword;
        private string _value;

        public InternalAliasKeyword InternalKeyword
        {
            get { return _internalKeyword; }
            set
            {
                Set(ref _internalKeyword, value);
                RaisePropertyChanged(() => Keyword);
            }
        }

        public string Keyword
        {
            get
            {
                switch (InternalKeyword)
                {
                    case InternalAliasKeyword.ArchiveExtractedDirectory:
                        return "{ARCHIVE_PATH}";
                    case InternalAliasKeyword.CurrentDirectory:
                        return "{CURRENT_PATH}";
                    default:
                        return null;
                }
            }
        }

        public string Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }

        public InternalAlias(InternalAliasKeyword keyword, string value)
        {
            InternalKeyword = keyword;
            Value = value;
        }

        public InternalAlias() { }
    }
}
