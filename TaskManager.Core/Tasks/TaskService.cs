using System;
using System.Threading.Tasks;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Tasks.Interfaces;
using TaskManager.ViewModel;
using TaskManager.ViewModel.Tasks;

namespace TaskManager.Core.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ICreateTaskCommand _createCommand;
        private readonly IUpdateTaskCommand _updateCommand;
        private readonly IDeleteTaskCommand _deleteCommand;
        private readonly IGetListQuery _taskListQuery;

        public TaskService(ICreateTaskCommand create, IUpdateTaskCommand update, IDeleteTaskCommand delete,
            IGetListQuery taskList)
        {
            _createCommand = create;
            _deleteCommand = delete;
            _updateCommand = update;
            _taskListQuery = taskList;
        }
        public Task<TaskResponse> Create(CreateTaskRequest request)
        {
            return _createCommand.ExecuteAsync(request);
        }

        public Task<TaskResponse> Update(UpdateTaskRequest request)
        {
            return _updateCommand.ExecuteAsync(request);
        }

        public Task<ListResponse<TaskResponseBase>> Delete(DeleteTaskRequest request)
        {
            return _deleteCommand.ExecuteAsync(request);
        }

        public Task<ListResponse<TaskResponse>> Get(Guid id = default)
        {
            return _taskListQuery.RunAsync(id);
        }
    }
}
