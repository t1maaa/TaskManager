using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Tasks.Interfaces;
using TaskManager.Db;
using TaskManager.ViewModel.Tasks;
using Status = TaskManager.Common.Task.Enums.Status;
using Task = TaskManager.Db.Entities.Task;

namespace TaskManager.Core.Tasks.Commands
{
    public class CreateTaskCommand : ICreateTaskCommand
    {
        private readonly ApplicationDbContext _dbContext;

        public CreateTaskCommand(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task<TaskResponse> ExecuteAsync(CreateTaskRequest request)
        {
            Task parent = null;
            if (request.ParentId != Guid.Empty && request.ParentId != null) //
            {
                parent = _dbContext.Tasks
                    .Where(t => t.Id == request.ParentId)
                    .Include(t => t.Subtasks)
                    .FirstOrDefault();
            }

            var task = new Task
            {
                Name = request.Name,
                Description = request.Description,
                Assignee = request.Assignee,
                ParentId = parent?.Id,
                CreatedAt = DateTime.Now,
                CompletedAt = default,
                Subtasks = null,
                Status = Status.Created
            };

            parent?.Subtasks.Add(task);

            await _dbContext.Tasks.AddAsync(task);
            await _dbContext.SaveChangesAsync();

            return new TaskResponse
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Assignee = task.Assignee,
                ParentId = task.ParentId,
                CreatedAt = task.CreatedAt,
                CompletedAt = task.CompletedAt,
                SpentTime = task.SpentTime,
                Status = task.Status,
                Complexity = task.Complexity
            };
        }
    }
}
