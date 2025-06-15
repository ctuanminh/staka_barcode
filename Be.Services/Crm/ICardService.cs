using Be.Common.Dtos.Crm;
using Be.Common.Responses;

namespace Be.Services.Crm
{
    public interface ICardService
    {
        Task<ApiResponse> GetAllCard();
        Task<ApiResponse> GetCardById(int id);
        Task<ApiResponse> CreateCard(CardRequest cardRequest);
        Task<ApiResponse> UpdateCard(CardRequest cardRequest);
        Task<ApiResponse> DeleteCard(int id);
        Task<ApiResponse> GetCardByDepartment(int departmentId);
        Task<ApiResponse> GetCardByUser(int userId);
        Task<ApiResponse> GetCardByStatus(int statusId);
        Task<ApiResponse> GetCardByTaskList(int taskListId);
        Task<ApiResponse> GetCardByTaskListAndStatus(int taskListId, int statusId);

        Task<ApiResponse> GetCardByTaskListAndUser(int taskListId, int userId);
        Task<ApiResponse> GetCardByTaskListAndDepartment(int taskListId, int departmentId);
        Task<ApiResponse> GetCardByTaskListAndDepartmentAndUser(int taskListId, int departmentId, int userId);

    }
}
