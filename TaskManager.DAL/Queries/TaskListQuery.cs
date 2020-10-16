using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.DAL.Interfaces;
using TaskManager.Db;
using TaskManager.ViewModel;
using TaskManager.ViewModel.Tasks;

namespace TaskManager.DAL.Queries
{
    public class TaskListQuery : ITaskListQuery
    {
        private readonly ApplicationDbContext _dbContext;
        public TaskListQuery(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        private async Task<IQueryable<TaskResponse>> LoadTasks(Guid parentId = default)
        {
            var tasks = _dbContext.Tasks.AsNoTracking()
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
                });
               // .ToListAsync();
               /*
            foreach (var task in tasks)
            {
                task.Subtasks = await LoadTasks(task.Id) as ICollection<TaskResponse>;
            }
               */
            return tasks;
        }

        public async Task<ListResponse<TaskResponse>> RunAsync(TaskParameters parameters, Guid id = default)
        {
            var tasks = await LoadTasks(id);
           tasks = ApplyParameters(tasks, parameters);
            
            return new ListResponse<TaskResponse>()
            {
                Items = await tasks.ToListAsync(),
                TotalItemsCount = tasks.Count(),
                PageSize = 10
            };
        }

        private static IQueryable<TaskResponse> ApplyParameters(IQueryable<TaskResponse> query, TaskParameters parameters)
        {
            if (!string.IsNullOrWhiteSpace(parameters.Status))
            {
                if (int.TryParse(parameters.Status, out var intStatus))
                {
                    query = query.Where(q => (int)q.Status == intStatus);
                }
                //else
                //{ TODO: support string-value of status filter : Enum.Status.ToString(), dictionary<int,string> (like below)
                //  TODO:  and whatever that can not be converted to sql doesn't work in Where. Try to fix or remove.
                //  TODO: Possible ways to fix:
                //  TODO: 1.Rewrite to 'var filter = from query where ...'
                //  TODO: 2.Apply Status filter at the end but do query.ToList() before that then  foreach t.Status != parameters.Status => remove(t).
                //   var status = EnumToDictionary<Enums.Status>();
                //   parameters.Status = parameters.Status.ToLower();
                //   query = query.Where(q => status[(int)q.Status] == parameters.Status);
                //}

            }

            if (!string.IsNullOrWhiteSpace(parameters.Assignee))
            {
                query = query.Where(q => q.Assignee == parameters.Assignee);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Name))
            {
                query = query.Where(q => q.Name == parameters.Name);
            }

            if (parameters.Complexity != 0)
            {
                query = query.Where(q => q.Complexity == parameters.Complexity);
            }
            //parameters.Name?.Length
                return query
                        //= query.Where(q =>
                //    (q.Name == parameters.Name || parameters.Name == null)
                //    && (q.Assignee == parameters.Assignee || parameters.Assignee == null)
                //    && (q.Status.ToString().ToLower() == parameters.Status.ToLower() || parameters.Status == null)
                //    && (q.Complexity == parameters.Complexity || parameters.Complexity == 0)
                //    )
                ;
        }
    }
}
