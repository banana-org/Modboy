// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <ProgressChangedEventArgs.cs>
//  Created By: Alexey Golub
//  Date: 17/08/2016
// ------------------------------------------------------------------ 

namespace Modboy.Models.EventArgs
{
    public class ProgressChangedEventArgs : System.EventArgs
    {
        public double Progress { get; }

        public ProgressChangedEventArgs(double progress)
        {
            Progress = progress;
        }
    }
}