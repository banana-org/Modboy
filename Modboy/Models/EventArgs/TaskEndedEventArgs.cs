// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <TaskEndedEventArgs.cs>
//  Created By: Alexey Golub
//  Date: 02/03/2016
// ------------------------------------------------------------------ 

using Modboy.Models.Internal;

namespace Modboy.Models.EventArgs
{
    public class TaskEndedEventArgs : System.EventArgs
    {
        public Task Task { get; }
        public bool Success { get; }

        public TaskEndedEventArgs(Task task, bool success)
        {
            Task = task;
            Success = success;
        }
    }
}