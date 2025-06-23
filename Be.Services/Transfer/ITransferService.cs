using Be.Core.Entities;

namespace Be.Services.Transfer
{
    public interface ITransferService
    {
        Task<TransferChecked> AddOrUpdateProductCheck(TransferChecked transferChecked);
        Task<List<TransferChecked>> GetTransferChecks(string transferCode, long branchId, string userName);
    }
}
