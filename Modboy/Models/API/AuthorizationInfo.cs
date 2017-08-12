// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <AuthorizationInfo.cs>
//  Created By: Alexey Golub
//  Date: 24/02/2016
// ------------------------------------------------------------------ 

using GalaSoft.MvvmLight;

namespace Modboy.Models.API
{
    public class AuthorizationInfo : ObservableObject
    {
        private string _username;
        private string _password;

        public string Username
        {
            get { return _username; }
            set { Set(ref _username, value); }
        }

        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        public AuthorizationInfo(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public AuthorizationInfo() { }

        public override string ToString()
        {
            return $"[{Username}] [{Password}]";
        }
    }
}