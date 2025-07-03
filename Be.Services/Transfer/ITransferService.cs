using Be.Core.Entities;

namespace Be.Services.Transfer
{
    public interface ITransferService
    {
        Task<TransferEntity> GetTransferById(long transferId);
        Task<TransferEntity> AddOrUpdateTransfer(TransferEntity transfer);
        Task<TransferChecked> AddProductCheck(TransferChecked transferChecked);
        Task<TransferChecked> UpdateProductCheck(long transferId, TransferChecked transferChecked);

        Task<List<TransferChecked>> GetCheckedProductsByParentTransfer(long transferId, string transferCode, long branchId,
            string userName, bool transfer);
        Task<TransferChecked> GetCheckedProductByTransfer(long transferId, string transferCode, long branchId,
            string userName, bool transfer, string productBarCode, string productCode);
    }
}
