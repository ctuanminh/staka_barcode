using Be.Common.Dtos.TaskList;
using Be.Common.Responses;

namespace Be.Services.Crm
{
    public class TaskListServiceImp : ServiceResponse, ITaskListService
    {
        public Task<ApiResponse> CreateTask(TaskListRequest taskRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetTaskById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
