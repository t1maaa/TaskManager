using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TaskManager.ViewModel.Tasks
{
    public enum Status
    {
        Created, InProgress, Paused, Done
    }

    public class TaskResponse : TaskResponseBase
    {
        private DateTime _completedAt;
        public string Description { get; set; }
        public string Assignee { get; set; }
        public Guid? ParentId { get; set; } = default;
        public DateTime CreatedAt { get; set; }

        public DateTime CompletedAt
        {
            get => _completedAt;
            set => _completedAt = value;
        }

        public string SpentTime { get; set; }
        public ICollection<TaskResponse> Subtasks { get; set; }// = new List<TaskResponse>();
        public Status Status { get; set; }
        public int Complexity { get; set; }
    }
}
