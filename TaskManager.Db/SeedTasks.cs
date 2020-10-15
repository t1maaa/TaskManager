using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Db.Models;
using Status = TaskManager.Common.Task.Enums.Status;

namespace TaskManager.Db
{
    public class SeedTasks
    {
        private Guid _lastUsedGuid = NextGuid(new Guid());
        private Guid LastUsedGuid { get; set; }
        public int NumOfTopLevelTasks { get; set; }
        public int NumOfSubtasks { get; set; }
        public int NumOfLevels { get; set; }        
        public List<Task> Tasks { get; set; }
        public static int Cnt { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numOfLevelsTasks">Num of top level tasks (main tasks)</param>
        /// <param name="numOfSubtasks">Num of subtasks for each task</param>
        /// <param name="numOfLevels">Limits nesting level of subtasks (includes top level tasks)</param>
        public SeedTasks(int numOfLevelsTasks, int numOfSubtasks, int numOfLevels)
        {
            NumOfTopLevelTasks = numOfLevelsTasks;
            NumOfSubtasks = numOfSubtasks;
            NumOfLevels = numOfLevels;
            var tasksList = new List<Task>(new Task[NumOfTopLevelTasks]);
            FillTasksList(tasksList, 1);
            Tasks = tasksList;
        }
        static readonly int[] byteOrder = { 15, 14, 13, 12, 11, 10, 9, 8, 6, 7, 4, 5, 0, 1, 2, 3 };

        private static Guid NextGuid(Guid guid)
        {
            var bytes = guid.ToByteArray();
            var canIncrement = byteOrder.Any(i => ++bytes[i] != 0);
            
            return new Guid(canIncrement ? bytes : new byte[16]);
        }
        private void FillTasksList(List<Task> tasks, int level, Guid parentId = default)
        {
            if (level > NumOfLevels) return;
            for (int i = 0; i < tasks.Count; i++)
            {
                Cnt++;
                tasks[i] = new Task
                {
                    Id = NextGuid(LastUsedGuid),
                    Name = $"Task {i + 1} level {level}",
                    Description = $"Description for task {i + 1} level {level}",
                    Assignee = $"Assignee for task {i + 1} level {level}",
                    CreatedAt = DateTime.Now,
                    CompletedAt = default,
                    Status = Status.Created,
                    ParentId = parentId,
                    //Complexity = 1
                   // Subtasks = new List<Task>(new List<Task>(new List<Task>(NumOfSubtasks)))
                };
                LastUsedGuid = tasks[i].Id;
                if (level < NumOfLevels)
                {
                    tasks[i].Subtasks.AddRange(new Task[NumOfSubtasks]);
                    FillTasksList(tasks[i].Subtasks, level + 1, tasks[i].Id);
                }
                
            }

        }

    }
}
