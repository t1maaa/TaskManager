using System;
using System.Collections.Generic;
using Status = TaskManager.Common.Task.Enums.Status;

namespace TaskManager.ViewModel.Tasks
{
    public class TaskResponse : TaskResponseBase
    {
        public string Description { get; set; }

        public string Assignee { get; set; }

        public Guid? ParentId { get; set; } = default;

        public DateTime CreatedAt { get; set; }

        public DateTime CompletedAt { get; set; }

        public string SpentTime { get; set; }

        public ICollection<TaskResponse> Subtasks { get; set; }

        public Status Status { get; set; }

        public int Complexity { get; set; }

        public TaskResponse()
        {
        }

        public TaskResponse(dynamic task)
        {
            if (task.GetType().FullName != "TaskManager.Db.Models.Task") return;
            Id = task.Id;
            Name = task.Name;
            Description = task.Description;
            Assignee = task.Assignee;
            ParentId = task.ParentId;
            CreatedAt = task.CreatedAt;
            CompletedAt = task.CompletedAt;
            SpentTime = task.SpentTime;
            Status = task.Status;
            Complexity = task.Complexity;
        }
    }
}
