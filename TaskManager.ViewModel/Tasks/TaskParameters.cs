using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using TaskManager.Common.Extensions;
using TaskManager.Common.Task;

namespace TaskManager.ViewModel.Tasks
{
    public class TaskParameters
    {
        public string OrderBy { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Assignee { get; set; }

        [Range(0, 3)]
        public string Status { get; set; }

        [Range(0, int.MaxValue)]
        public int Complexity { get; set; }
    }
}
