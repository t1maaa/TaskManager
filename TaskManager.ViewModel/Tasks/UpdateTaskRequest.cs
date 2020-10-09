using System;
using System.ComponentModel.DataAnnotations;
using Status = TaskManager.Common.Task.Enums.Status;

namespace TaskManager.ViewModel.Tasks
{
    public class UpdateTaskRequest
    {
        [Required]
        public Guid Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string Assignee { get; set; }

        public Guid? ParentId { get; set; } = default;

        public Status Status { get; set; }
    }
}
