using Be.Common.Dtos.Crm;
using Be.Common.Responses;

namespace Be.Services.Crm
{
    public class CardServiceImp : ServiceResponse, ICardService
    {
        public Task<ApiResponse> CreateCard(CardRequest cardRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> DeleteCard(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetAllCard()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetCardByDepartment(int departmentId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetCardById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetCardByStatus(int statusId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetCardByTaskList(int taskListId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetCardByTaskListAndDepartment(int taskListId, int departmentId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetCardByTaskListAndDepartmentAndUser(int taskListId, int departmentId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetCardByTaskListAndStatus(int taskListId, int statusId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetCardByTaskListAndUser(int taskListId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetCardByUser(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> UpdateCard(CardRequest cardRequest)
        {
            throw new NotImplementedException();
        }
    }
}
