using System.Threading.Tasks;
using TaskManager.ViewModel.Tasks;

namespace TaskManager.Core.Tasks.Interfaces
{
    public interface IUpdateTaskCommand
    {
        Task<TaskResponse> ExecuteAsync(UpdateTaskRequest request);
    }
}
