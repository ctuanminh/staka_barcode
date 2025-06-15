using Be.Common.Dtos.TaskList;
using Be.Common.Responses;

namespace Be.Services.Crm
{
    public interface ITaskListService
    {
        Task<ApiResponse> GetAllTasks();
        Task<ApiResponse> GetTaskById(int id);
        Task<ApiResponse> CreateTask(TaskListRequest taskRequest);
    }
}
