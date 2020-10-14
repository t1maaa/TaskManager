using System;
using System.Threading.Tasks;
using TaskManager.ViewModel;
using TaskManager.ViewModel.Tasks;

namespace TaskManager.Core.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponse> Create(CreateTaskRequest request);
        Task<TaskResponse> Update(UpdateTaskRequest request);
        Task<ListResponse<TaskResponseBase>> Delete(DeleteTaskRequest request);
        Task<ListResponse<TaskResponse>> Get(Guid id = default);

    }
}
