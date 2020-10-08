using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskManager.ViewModel;
using TaskManager.ViewModel.Tasks;

namespace TaskManager.DAL.Interfaces
{
    public interface ITaskListQuery
    {
        Task<ListResponse<TaskResponse>> RunAsync(Guid id = default);
    }
}
