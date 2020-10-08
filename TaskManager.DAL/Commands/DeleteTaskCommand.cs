using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.DAL.Interfaces;
using TaskManager.Db;
using TaskManager.Db.Models;
using TaskManager.ViewModel;
using TaskManager.ViewModel.Tasks;
using Task = TaskManager.Db.Models.Task;

namespace TaskManager.DAL.Commands
{
    public class DeleteTaskCommand : IDeleteTaskCommand
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly List<TaskResponseBase> _deleted;
        public DeleteTaskCommand(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _deleted = new List<TaskResponseBase>();
        }

        public async Task<ListResponse<TaskResponseBase>> ExecuteAsync(DeleteTaskRequest request)
        {
            var task = _dbContext.Tasks.Where(t => t.Id == request.Id).Include(p => p.ParentObj.Subtasks).FirstOrDefault();//TODO: performance problem?

            if (task != null) //TODO: and != parentID?
            {
                _deleted.Add(await DeleteTask(task, request.KeepSubtasks));
                task.ParentObj.Subtasks.Remove(task);
            }
            
            await _dbContext.SaveChangesAsync();

            return new ListResponse<TaskResponseBase>
            {
                Items = _deleted,
                TotalItemsCount = _deleted.Count
            };
        }

        private async Task<TaskResponseBase> DeleteTask(Task task, bool keepSubtasks)
        { 
            var subtasks = await _dbContext.Tasks.Where(predicate: t => t.ParentId == task.Id)
                .ToListAsync();

            if (subtasks != null)
            {
                if (keepSubtasks)
                {
                    foreach (var subtask in subtasks)
                    {
                        subtask.ParentId = Guid.Empty;
                    }
                }
                else
                {
                    foreach (var subtask in subtasks)
                    {
                        _deleted.Add(item: await DeleteTask(task: subtask, keepSubtasks: keepSubtasks));
                    }
                }
            }
            _dbContext.Tasks.Remove(entity: task);
            return new TaskResponseBase
            {
                Id = task.Id,
                Name = task.Name
            };
        }

    }
}
