using System.Threading.Tasks;
using TaskManager.ViewModel.Tasks;

namespace TaskManager.Core.Tasks.Interfaces
{
    public interface ICreateTaskCommand
    {
        Task<TaskResponse> ExecuteAsync(CreateTaskRequest request);
    }
}
