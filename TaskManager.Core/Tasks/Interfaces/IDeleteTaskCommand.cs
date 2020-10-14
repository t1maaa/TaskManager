using System.Threading.Tasks;
using TaskManager.ViewModel;
using TaskManager.ViewModel.Tasks;

namespace TaskManager.Core.Tasks.Interfaces
{
    public interface IDeleteTaskCommand
    {
        Task<ListResponse<TaskResponseBase>> ExecuteAsync(DeleteTaskRequest request);
    }
}
