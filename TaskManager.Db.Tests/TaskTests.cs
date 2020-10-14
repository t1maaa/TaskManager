using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TaskManager.Db.Entities;
using Status = TaskManager.Common.Task.Enums.Status;

namespace TaskManager.Db.Tests
{
    [TestClass]
    public class TaskTests
    {
        private Task GetTaskWithSubtasks(Task testTask)
        {
            testTask.CreatedAt = new DateTime(2020, 05, 20);
            testTask.CompletedAt = new DateTime(2020, 05, 21);
            for (int i = 0; i < 5; i++)
            {
                Task task = new Task {CreatedAt = new DateTime(2020, 05, 20), CompletedAt = new DateTime(2020, 05, 21)};

                Task subtask = new Task
                {
                    CreatedAt = new DateTime(2020, 05, 20), CompletedAt = new DateTime(2020, 05, 21)
                };

                testTask.Subtasks.Add(task); //add 5 subtasks
                testTask.Subtasks.Last().Subtasks.Add(subtask); //add 1 subtask for every subtasks of t obj (subtasks of 2nd level)
            }
            return testTask;
        }


        [TestMethod]
        public void Complexity_RecursiveCountingSubtasks2lvl()
        {
            Task t = GetTaskWithSubtasks(new Task());
            //Task t = new Task();
            
            //for (int i = 0; i < 5; i++)
            //{
            //    t.Subtasks.Add(new Task()); //add 5 subtasks
            //    t.Subtasks.Last().Subtasks.Add(new Task()); //add 1 subtask for every subtasks of t obj (subtasks of 2nd level)
            //}

            int expected = 11; // 1(t task) + 5 * (1 (subtask) + 1 (subtask of subtask))
            Assert.AreEqual(expected, t.Complexity, 0.1, "Complexity of task with 2 level of subtasks is wrong");

        }

        [TestMethod]
        public void Complexity_NoSubtasks()
        {
            Task t = new Task();
            int expected = 1;
            Assert.AreEqual(expected, t.Complexity, 0.1, "Complexity of task with no subtasks");
        }

        [TestMethod]
        public void SpentTime_WithNotDoneStatus()
        {
            Task t = new Task {CreatedAt = new DateTime(2020, 05, 20), CompletedAt = new DateTime(2020, 05, 21)};
            TimeSpan expected = TimeSpan.Zero;
            Assert.AreEqual(expected, t.SpentTime, "Wrong SpentTime with not done status task");

        }

        [TestMethod]
        public void SpentTime_WithDoneStatus()
        {
            Task t = new Task
            {
                CreatedAt = new DateTime(2020, 05, 20),
                CompletedAt = new DateTime(2020, 05, 21),
                Status = Status.Done
            };
            TimeSpan expected = t.CompletedAt - t.CreatedAt;
            Assert.AreEqual(expected, t.SpentTime, "Wrong SpentTime with done status task");

        }

        [TestMethod]
        public void SpentTime_WithDoneStatusAndSubtasks()
        {
            Task t = GetTaskWithSubtasks(new Task()); //task with 5 subtasks and 1 sub-subtask for every subtasks (11 in total include main Task) 
            t.Status = Status.Done;
            TimeSpan expected = new TimeSpan(11,0,0,0); 
            Assert.AreEqual(expected, t.SpentTime, "Wrong SpentTime with subtasks and done status in main task."); //TODO тест на доне статус у сабтасков перед тем как доне статус мейну присвоить, ѕотом как нить
        }

        [TestMethod]
        public void Test()
        {
            var list = new SeedTasks(3, 3, 3).Tasks;

           // list[list.Count - 1].Subtasks[];
            foreach (var t in list)
            {
                Console.WriteLine(t.Id);
                Console.WriteLine(t.Subtasks.Count);
                
            }
            Assert.AreEqual(SeedTasks.Cnt, 10);

        }
    }
}
