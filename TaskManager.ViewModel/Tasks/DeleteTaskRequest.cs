using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TaskManager.ViewModel.Tasks
{
    public class DeleteTaskRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public bool Confirmed { get; set; }

        [Required]
        public bool KeepSubtasks { get; set; }
    }
}
