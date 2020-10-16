using System;
using System.Threading.Tasks;
using TaskManager.ViewModel;
using TaskManager.ViewModel.Tasks;

namespace TaskManager.DAL.Interfaces
{
    public interface ITaskListQuery
    {
        Task<ListResponse<TaskResponse>> RunAsync(TaskParameters parameters, Guid id = default);
    }
}
