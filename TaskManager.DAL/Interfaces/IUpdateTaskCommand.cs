using System.Threading.Tasks;
using TaskManager.ViewModel.Tasks;

namespace TaskManager.DAL.Interfaces
{
    public interface IUpdateTaskCommand
    {
        Task<TaskResponse> ExecuteAsync(UpdateTaskRequest request);
    }
}
