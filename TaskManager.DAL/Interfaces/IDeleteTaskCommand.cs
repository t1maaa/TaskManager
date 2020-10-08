using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.ViewModel;
using TaskManager.ViewModel.Tasks;

namespace TaskManager.DAL.Interfaces
{
    public interface IDeleteTaskCommand
    {
        Task<ListResponse<TaskResponseBase>> ExecuteAsync(DeleteTaskRequest request);
    }
}
