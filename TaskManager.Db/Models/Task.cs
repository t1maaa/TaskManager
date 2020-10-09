using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Status = TaskManager.Common.Task.Enums.Status;

namespace TaskManager.Db.Models
{
    public class Task
    {
        private string _spentTime = TimeSpan.Zero.ToString("g");
        private Status _status = Status.Created;

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string Assignee { get; set; }

        [ForeignKey("ParentId")]
        public Task ParentObj { get; set; }
        
        public Guid? ParentId { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime CompletedAt { get; set; }

        public string SpentTime
        {
            get
            {
                if (Status == Status.Done)
                {
                    var value = CompletedAt - CreatedAt;
                    return (value + SubtasksTimeCount(Subtasks)).ToString("g");
                }

                return TimeSpan.Zero.ToString("g");
            }
            set => _spentTime = value;
        }

        public List<Task> Subtasks { get; set; } = new List<Task>();

        public Status Status
        {
            get => _status;
            set
            {
                _status = _status switch
                {
                    Status.Created => value != Status.Done ? value : _status,
                    Status.InProgress => value != Status.Created ? value : _status,
                    Status.Paused => value != Status.Created ? value : _status,
                    Status.Done => value != Status.Created ? value : _status,
                    _ => _status
                };
            }
        }

        public int Complexity
        {
            get
            {
                int value = 1;
                if (Subtasks != null && Subtasks.Count > 0)
                    value += SubtasksCount(Subtasks);
                return value;
            }
            set { }
        }

        private static TimeSpan SubtasksTimeCount(ICollection<Task> tasks)
        {
            TimeSpan value = TimeSpan.Zero;
            foreach (Task task in tasks)
            {
                value += task.CompletedAt - task.CreatedAt;

                if (task.Subtasks.Count > 0)
                    value += SubtasksTimeCount(task.Subtasks);
            }

            return value;
        }

        private static int SubtasksCount(ICollection<Task> tasks)
        {
            if (tasks == null) return 0;
            int value = tasks.Count;
            if (value > 0)
                value += tasks.Sum(task =>
                    (task.Subtasks != null && task.Subtasks.Count > 0) ? SubtasksCount(task.Subtasks) : 0);
            return value;

        }
    }
}
