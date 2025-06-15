using Be.Common.Dtos.Crm;
using Be.Common.Responses;

namespace Be.Services.Crm
{
    public interface IBoardService
    {
        Task<ApiResponse> GetAllBoard();
        Task<ApiResponse> GetBoardById(int id);
        Task<ApiResponse> CreateBoard(BoardRequest boardRequest);
        Task<ApiResponse> UpdateBoard(BoardRequest boardRequest);
        Task<ApiResponse> DeleteBoard(int id);
    }
}
