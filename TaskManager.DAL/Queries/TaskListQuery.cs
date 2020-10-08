using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskManager.DAL.Interfaces;
using TaskManager.Db;
using TaskManager.Db.Models;
using TaskManager.ViewModel;
using TaskManager.ViewModel.Tasks;
using Status = TaskManager.ViewModel.Tasks.Status;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.DAL.Queries
{
    public class TaskListQuery : ITaskListQuery
    {
        private readonly ApplicationDbContext _dbContext;
        public TaskListQuery(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        private async Task<List<TaskResponse>> LoadTasks(Guid parentId = default)
        {
            /* if (parentId == Guid.Empty || parentId == null)
             {
 
             }*/
            var tasks = await _dbContext.Tasks
                .Where(t => (parentId != Guid.Empty && parentId != null) 
                    ? t.ParentId == parentId
                    : t.ParentId == Guid.Empty || t.ParentId == null)
                .Select(t => new TaskResponse //TODO: Брать Таски(не респонсы) и потом отдельно перегонять в респонсы, чтобы значения подтянулись вычисляемые
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Assignee = t.Assignee,
                CreatedAt = t.CreatedAt,
                CompletedAt = t.CompletedAt,
                SpentTime = t.SpentTime,
                Status = (Status) t.Status,
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
                //Items = new List<TaskResponse>(tasks.Select(t => new TaskResponse
                //{
                //    Id = t.Id,
                //    Name = t.Name,
                //    Description = t.Description,
                //    Assignee = t.Assignee,
                //    CreatedAt = t.CreatedAt,
                //    CompletedAt = t.CompletedAt,
                //    SpentTime = t.SpentTime,
                //    Status = (Status)t.Status,
                //    Complexity = t.Complexity, //TODO: КРАСАВА, считает, но теперь сделай DeepCopy в Model.Task, или думай еще как вместе с сабтасками грузить(но походу энивей копирование надо) или думай как апдейтить корректно в базе(но походу никак по нормальному)
                //    ParentId = t.ParentId
                //}))

                TotalItemsCount = tasks.Count,
                PageSize = 10 //TODO: from body param
                //TODO: paginations
                //TODO: filters
            };
        }
    }
}
