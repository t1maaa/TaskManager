using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TaskManager.ViewModel.Tasks
{
    public class CreateTaskRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
        [MaxLength(200)]
        public string Assignee { get; set; }
        public Guid? ParentId { get; set; } = default;
    }
}
