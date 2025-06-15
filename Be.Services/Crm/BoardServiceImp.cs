using Be.Common.Dtos.Crm;
using Be.Common.Responses;

namespace Be.Services.Crm
{
    public class BoardServiceImp : ServiceResponse, IBoardService
    {
        public Task<ApiResponse> CreateBoard(BoardRequest boardRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> DeleteBoard(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetAllBoard()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetBoardById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> UpdateBoard(BoardRequest boardRequest)
        {
            throw new NotImplementedException();
        }
    }
}
