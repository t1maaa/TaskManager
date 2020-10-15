using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Tasks.Interfaces;
using TaskManager.Db;
using TaskManager.ViewModel;
using TaskManager.ViewModel.Tasks;

namespace TaskManager.Core.Tasks.Queries
{
    public class GetListQuery : IGetListQuery
    {
        private readonly ApplicationDbContext _dbContext;
        public GetListQuery(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        private async Task<List<TaskResponse>> LoadTasks(Guid parentId = default)
        {
            var tasks = await _dbContext.Tasks.AsNoTracking()
                .Where(t => (parentId != Guid.Empty && parentId != null)
                    ? t.ParentId == parentId
                    : t.ParentId == Guid.Empty || t.ParentId == null)
                .Select(t => new TaskResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    Assignee = t.Assignee,
                    CreatedAt = t.CreatedAt,
                    CompletedAt = t.CompletedAt,
                    SpentTime = t.SpentTime,
                    Status = t.Status,
                    Complexity = t.Complexity,
                    ParentId = t.ParentId
                })
                .ToListAsync();

            foreach (var task in tasks)
            {
                task.Subtasks = await LoadTasks(task.Id);
            }

            return tasks;
        }

        public async Task<ListResponse<TaskResponse>> RunAsync(Guid id = default)
        {
            var tasks = await LoadTasks(id);

            return new ListResponse<TaskResponse>()
            {
                Items = tasks,
                TotalItemsCount = tasks.Count,
                PageSize = 10
            };
        }
    }
}
