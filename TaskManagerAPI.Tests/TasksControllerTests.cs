using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TaskManager.Db;
using TaskManagerAPI.Controllers;

namespace TaskManagerAPI.Tests
{
    [TestClass]
    public class TasksControllerTests
    {
        private  TasksController _controller;
        private readonly ApplicationDbContext _dbContext;

        [TestInitialize]
        public void Init()
        {
            _controller = new TasksController(_dbContext);
        }

        [TestMethod]
        public void CreateTask()
        {
         //   await _controller.CreateTask()
        }

        [TestMethod]
        public void GetTask()
        {
        }

        [TestMethod]
        public void GetTasksList()
        {
        }

        [TestMethod]
        public void UpdateTask()
        {
        }

        [TestMethod]
        public void DeleteTask()
        {
        }
    }
}
