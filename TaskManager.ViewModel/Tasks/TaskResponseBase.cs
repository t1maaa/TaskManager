using System;

namespace TaskManager.ViewModel.Tasks
{
    public class TaskResponseBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
