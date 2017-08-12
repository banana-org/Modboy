// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <TaskEventArgs.cs>
//  Created By: Alexey Golub
//  Date: 02/03/2016
// ------------------------------------------------------------------ 

using Modboy.Models.Internal;

namespace Modboy.Models.EventArgs
{
    public class TaskEventArgs : System.EventArgs
    {
        public Task Task { get; }

        public TaskEventArgs(Task task)
        {
            Task = task;
        }
    }
}