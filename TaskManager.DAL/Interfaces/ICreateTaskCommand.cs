using System.Threading.Tasks;
using TaskManager.ViewModel.Tasks;

namespace TaskManager.DAL.Interfaces
{
    public interface ICreateTaskCommand
    {
        Task<TaskResponse> ExecuteAsync(CreateTaskRequest request);
    }
}
