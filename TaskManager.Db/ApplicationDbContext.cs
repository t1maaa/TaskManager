using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TaskManager.Db.Entities;

namespace TaskManager.Db
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.Migrate();
            //Database.EnsureDeleted();
            /*if(Database.EnsureCreated())
                new SeedDb(this);*/
        }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }


        private static void SeedTasks(List<Task> tasks, ref ModelBuilder modelBuilder)
        {
            foreach (Task task in tasks)
            {
                if (task.Subtasks.Count > 0)
                    SeedTasks(task.Subtasks, ref modelBuilder); 
                modelBuilder.Entity<Task>().HasData(task);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           /* SeedTasks seedData = new SeedTasks(3, 3, 3);

            var tasks = seedData.Tasks;

            SeedTasks(ref tasks, ref modelBuilder);*/

           modelBuilder.Entity<Task>().HasMany(t => t.Subtasks).WithOne("ParentObj");
           // modelBuilder.UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);
            modelBuilder.UsePropertyAccessMode(PropertyAccessMode.Property);
            // modelBuilder.Entity<Task>().Property(e => e.Complexity)


            // modelBuilder.Entity<Task>().Property(t => t.Complexity).UsePropertyAccessMode(PropertyAccessMode.Property);
            //modelBuilder.Entity<Task>().Property(t => t.Complexity).ValueGeneratedOnAddOrUpdate();
            // modelBuilder.Entity<Task>().HasData(null);
            /*
            foreach (Task task in seed.Tasks)
            {
                while

                if (task.Subtasks.Count > 0)
                {
                    foreach (Task subtask in task.Subtasks)
                    {
                        
                    }
                }

                modelBuilder.Entity<Task>().HasData(task);
            }
            */
            // modelBuilder.Entity<Task>().HasData(seed.Tasks); 
            /*
            SeedTasks(3,3,3)

            #region hide

            /*
           int totalTasks = 0;
           int bitsUsed = 1;
           for (int i = 0; i < numOfLevels; i++)
           {
               totalTasks += (int)Math.Pow(numOfSubtasks, i);
           }
           #region GuidManipulation

           byte[] guid = new Guid().ToByteArray();

           if (totalTasks > 255)
               bitsUsed = totalTasks / 255 + 1;

           for (int i = 0; i <= bitsUsed; i++)
           {
               for (int j = 0; j < 255; j++)
               {
                   guid[^(1+i)]
               }
           }

           #endregion
           */

            /* var TasksSeedData = new Task[]
             {
                 new Task
                 {
                     Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0),
                     Name = "Task 1",
                     Description = null,
                     Assignee = null,
                     ParentId = null,
                     CreatedAt = default,
                     CompletedAt = default,
                     Subtasks = null,
                     Status = Status.Created
                 },
                 new Task
                 {
                     Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0),
                     Name = "Task 1 subtask 1",
                     Description = null,
                     Assignee = null,
                     ParentId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0),
                     CreatedAt = default,
                     CompletedAt = default,
                     Subtasks = null,
                     Status = Status.Created
                 },
                 new Task
                 {
                     Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1),
                     Name = "Task 1 subtask 1 subsubtask 1",
                     Description = null,
                     Assignee = null,
                     ParentId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0),
                     CreatedAt = default,
                     CompletedAt = default,
                     Subtasks = null,
                     Status = Status.Created
                 },
                 new Task
                 {
                     Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0),
                     Name = "Task 2",
                     Description = null,
                     Assignee = null,
                     ParentId = null,
                     CreatedAt = default,
                     CompletedAt = default,
                     Subtasks = null,
                     Status = Status.Created
                 },
                 new Task
                 {
                     Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0),
                     Name = "Task 2 subtask 1",
                     Description = null,
                     Assignee = null,
                     ParentId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0),
                     CreatedAt = default,
                     CompletedAt = default,
                     Subtasks = null,
                     Status = Status.Created
                 },
                 new Task
                 {
                     Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 1),
                     Name = "Task 2 subtask 1 subsubtask 1",
                     Description = null,
                     Assignee = null,
                     ParentId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0),
                     CreatedAt = default,
                     CompletedAt = default,
                     Subtasks = null,
                     Status = Status.Created
                 },
                 new Task
                 {
                     Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0),
                     Name = "Task 3",
                     Description = null,
                     Assignee = null,
                     ParentId = null,
                     CreatedAt = default,
                     CompletedAt = default,
                     Subtasks = null,
                     Status = Status.Created
                 },
                 new Task
                 {
                     Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 3, 1, 0),
                     Name = "Task 3 subtask 1",
                     Description = null,
                     Assignee = null,
                     ParentId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0),
                     CreatedAt = default,
                     CompletedAt = default,
                     Subtasks = null,
                     Status = Status.Created
                 },
                 new Task
                 {
                     Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 3, 1, 1),
                     Name = "Task 3 subtask 1 subsubtask 1",
                     Description = null,
                     Assignee = null,
                     ParentId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 3, 1, 0),
                     CreatedAt = default,
                     CompletedAt = default,
                     Subtasks = null,
                     Status = Status.Created
                 }
             };


            #endregion

            var TopLevelTasks = new List<Task>(new Task[numOfTopLevelTasks]);

            foreach (Task task in TopLevelTasks)
            {

            }

            for (int i = 0; i < numOfTopLevelTasks; i++)
            {
                TopLevelTasks[i].Name = $"Task {i}";
                TopLevelTasks[i].Description = $"Description for task {i}";
                TopLevelTasks[i].Assignee = $"Assignee for task {i}";
                TopLevelTasks[i].CreatedAt = DateTime.Now;
                TopLevelTasks[i].CompletedAt = default;
                TopLevelTasks[i].Status = Status.Created;
                TopLevelTasks[i].ParentId = null;


            }



            modelBuilder.Entity<Task>().HasData(TasksSeedData);

        }

        static int[] byteOrder = { 15, 14, 13, 12, 11, 10, 9, 8, 6, 7, 4, 5, 0, 1, 2, 3 };

        private static Guid NextGuid(Guid guid)
        {
            var bytes = guid.ToByteArray();
            var canIncrement = byteOrder.Any(i => ++bytes[i] != 0);
            return new Guid(canIncrement ? bytes : new byte[16]);
        }

        private static Task FillData(ref Task task, int index, int numOfSubtasks, int numOfLevels)
        {
            task.Name = $"Task {index}";
            task.Description = $"Description for task {index}";
            task.Assignee = $"Assignee for task {index}";
            task.CreatedAt = DateTime.Now;
            task.CompletedAt = default;
            task.Status = Status.Created;
            task.ParentId = null;
            task.Subtasks = subtasksList[index - 1];
        }*/
        }


    }
    }
