using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.DAL.Interfaces;
using TaskManager.Db;
using TaskManager.ViewModel.Tasks;
using Status = TaskManager.Common.Task.Enums.Status;

namespace TaskManager.DAL.Commands
{
    public class UpdateTaskCommand : IUpdateTaskCommand
    {
        private readonly ApplicationDbContext _dbContext;

        public UpdateTaskCommand(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<TaskResponse> ExecuteAsync(UpdateTaskRequest request)
        {
            var task = await _dbContext.Tasks
                .FindAsync(request.Id);

            if (task != null)
            {
                task.Name = request.Name ?? task.Name;
                task.Description = request.Description ?? task.Description;
                task.Assignee = request.Assignee ?? task.Assignee;

                if (request.ParentId != default)
                {
                    var parent = await _dbContext.Tasks.FindAsync(request.ParentId);
                    if (parent != null)
                    {
                        task.ParentId = task.ParentId;
                    }
                }

                if (request.Status != default)
                {
                    task.Status = request.Status;

                    if (task.Status == Status.Done)
                    {
                        task.CompletedAt = DateTime.Now;
                    }
                }

                _dbContext.Entry(task).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return new TaskResponse(task);
            }

            return null;
        }
    }
}