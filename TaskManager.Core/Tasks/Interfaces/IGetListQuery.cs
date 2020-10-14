using System;
using System.Threading.Tasks;
using TaskManager.ViewModel;
using TaskManager.ViewModel.Tasks;

namespace TaskManager.Core.Tasks.Interfaces
{
    public interface IGetListQuery
    {
        Task<ListResponse<TaskResponse>> RunAsync(Guid id = default);
    }
}
