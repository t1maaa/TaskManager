using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Db.Models;
using Task = TaskManager.Db.Models.Task;

namespace TaskManager.Db
{
    public class SeedDb
    {
        private ApplicationDbContext _dbContext;

        public SeedDb(ApplicationDbContext context)
        {
            _dbContext = context;
            DataGenerator();
            _dbContext.SaveChangesAsync();
        }

        private void DataGenerator()
        {
            int numOfTasks = 3;
            int numOfSubtasks = 3;
            var tasks = new List<Task>(new Task[numOfTasks] /*{new Task(), new Task(), new Task()}*/);
            for (var i = 0; i < tasks.Count; i++)
            {
                tasks[i] = new Task();
                //   {
                //  Id = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                //    };
                //  var stopwatch = Stopwatch.StartNew();
                //   Thread.Sleep(1100);
                //  stopwatch.Stop();
            }

            var subtasksList = new List<List<Task>>(new List<Task>[tasks.Count]);
            for (var i = 0; i < subtasksList.Count; i++)
            {
                subtasksList[i] = new List<Task>(new Task[numOfSubtasks]);
                for (var j = 0; j < subtasksList[i].Count; j++)
                {
                    subtasksList[i][j] = new Task
                    {
                        Id = new Guid()
                    };
                    //  Id = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    //   };
                    //  var stopwatch = Stopwatch.StartNew();
                    //  Thread.Sleep(100);
                    //  stopwatch.Stop();
                }
            }

            int index = 0;
            tasks.ForEach(task =>
            {
               ++index;
                task.Name = $"Task {index}";
                task.Description = $"Description for task {index}";
                task.Assignee = $"Assignee for task {index}";
                task.CreatedAt = DateTime.Now;
                task.CompletedAt = default;
                task.Status = Status.Created;
                task.Subtasks = subtasksList[index - 1];
                task.ParentId = null;
                // task.Subtasks = subtasksList.Count > index ? subtasksList[index - 1] : subtasksList[^1];

                int subindex = 0;
                foreach (Task subtask in task.Subtasks)
                {
                    ++subindex;
                    subtask.Name = $"Subtask {subindex} for task {index}";
                    subtask.Description = $"Description for subtask {subindex} of task {index}";
                    subtask.Assignee = $"Assignee for subtask {subindex} of task {index}";
                    subtask.CreatedAt = DateTime.Now;
                    subtask.CompletedAt = default;
                    subtask.Status = Status.Created;
                    subtask.ParentId = task.Id;
                    subtask.Subtasks = new List<Task>(new Task[1]
                    {
                        new Task
                        {
                            //Id = numOfTasks + numOfSubtasks + subindex + 1,
                            Name = $"Sub-subtask for subtask {subindex} of task {index}",
                            Description = $"",
                            Assignee = $"",
                            CreatedAt = DateTime.Now,
                            CompletedAt = default,
                            Status = Status.Created,
                            ParentId = subtask.Id,
                            Subtasks = null

                        }

                    });
                }

             //  Console.WriteLine(_dbContext.Tasks.Add(task));
            });
            _dbContext.Tasks.AddRange(tasks);
            // await _dbContext.SaveChangesAsync();
            /*  var subtasks1 = new List<Task>();
              var subtasks2 = new List<Task>();
              var subtasks3 = new List<Task>();*/
            /*subtasks1.AddRange(new Task[] {new Task()});
            subtasks2.AddRange(new Task[] { new Task(), new Task()});
            subtasks3.AddRange(new Task[] {new Task(), new Task(), new Task()});
            int subindex = 1;
            int index = 1;
            foreach (var t in subtasks1, subtasks3)
            {
                t.Name = $"Subtask {index} for Task"
            }*/
            /*
            subtasks1.Add(new Task
            {
                Name = "Subtask 1 for Task 1",
                Description = "Description for subtask 1 of task 1",
                Assignee = "Assignee for subtask 1 of task 1",
                CreatedAt = DateTime.Now,
                CompletedAt = default,
                Subtasks = null,
                Status = Status.Created
            });
            subtasks1.Add(new Task
            {
                Name = "Subtask 2 for Task 1",
                Description = "Description for subtask 2 of task 1",
                Assignee = "Assignee for subtask 1 of task 1",
                CreatedAt = DateTime.Now,
                CompletedAt = default,
                Subtasks = null,
                Status = Status.Created
            });

            context.Tasks.AddRange(
                new Task 
            {
                Name = "Task1",
                Description = "Description for Task1",
                Assignee = "Assignee for Task 1",
                CreatedAt = DateTime.Now,
                CompletedAt = default,
                Subtasks = new List<Task>(),
                Status = Status.Created
            }, 
                new Task
                {
                    Name = "Subtask 1 for Task 1",
                    Description = "Description for subtask 1 of task 1",
                    Assignee = "Assignee for subtask 1 of task 1",
                    CreatedAt = DateTime.Now,
                    CompletedAt = default,
                    Subtasks = null,
                    Status = Status.Created
                },
                new Task
                {
                    Name = "Subtask 1 for Task 1",
                    Description = "Description for subtask 1 of task 1",
                    Assignee = "Assignee for subtask 1 of task 1",
                    CreatedAt = DateTime.Now,
                    CompletedAt = default,
                    Subtasks = null,
                    Status = Status.Created
                });
            */

        }
    }
}
