// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <TaskStateChangedEventArgs.cs>
//  Created By: Alexey Golub
//  Date: 02/03/2016
// ------------------------------------------------------------------ 

using Modboy.Models.Internal;

namespace Modboy.Models.EventArgs
{
    public class TaskStateChangedEventArgs : System.EventArgs
    {
        public Task Task { get; }
        public TaskExecutionStatus Status { get; }
        public double Progress { get; }

        public TaskStateChangedEventArgs(Task task, TaskExecutionStatus status, double progress)
        {
            Task = task;
            Status = status;
            Progress = progress;
        }
    }
}