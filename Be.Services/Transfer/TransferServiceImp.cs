using Be.Common.Tranfer.Response;
using Be.Core.Entities;
using Be.Data.Repository;
using Be.Services.KiotViet;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Be.Services.Transfer
{
    public class TransferServiceImp(IRepository repository, IKiotVietService kiotVietService) : ITransferService
    {
        private readonly IKiotVietService _kiotVietService = kiotVietService;
        public async Task<TransferEntity> GetTransferById(long transferId)
        {
            return await repository.GetQueryable<TransferEntity>()
                .FirstOrDefaultAsync(t => t.TransferId == transferId);
        }

        public async Task<TransferEntity> AddOrUpdateTransfer(TransferEntity transfer)
        {
            var existing = await repository.GetQueryable<TransferEntity>()
                .FirstOrDefaultAsync(t => t.TransferId == transfer.TransferId);

            if (existing == null)
                await repository.AddAsync(transfer);
            else
                await repository.UpdateAsync(existing);

            await repository.SaveChangeAsync();
            return transfer;
        }

        public async Task<TransferChecked> AddProductCheck(TransferChecked transferChecked)
        {
            if(transferChecked == null) return null;
            await repository.AddAsync(transferChecked);
            await repository.SaveChangeAsync();
            return transferChecked;
        }
        public async Task<TransferChecked> UpdateProductCheck(long transferId, TransferChecked transferChecked)
        {
            var existing = await repository.GetQueryable<TransferChecked>()
                .FirstOrDefaultAsync(x => x.TransferId == transferId && x.Id == transferChecked.Id);
            if (existing == null) return null;
            existing.ScanCount = transferChecked.ScanCount;
            await repository.UpdateAsync(existing);
            await repository.SaveChangeAsync();
            return transferChecked;
        }

        public async Task<List<TransferChecked>> GetCheckedProductsByParentTransfer(
            long transferId,
            string transferCode,
            long branchId,
            string userName,
            bool transfer)
        {
            var transferFind = transferCode;

            // Nếu KHÔNG phải phiếu chuyển thì tìm phiếu cha (vd: "xxx.01." → "xxx")
            if (!transfer && transferCode.Contains('.'))
            {
                var transferCodeParent = transferCode.Split('.')[0];

                // Tìm trong DB local
                var transferLocal = await repository.GetQueryable<TransferEntity>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TransferCode == transferCodeParent);

                if (transferLocal == null)
                    return [];

                // Gọi KiotViet API để kiểm tra trạng thái của phiếu cha
                var apiUrl = $"https://public.kiotapi.com/transfers/{transferLocal.TransferId}";
                var (success, content) = await _kiotVietService.CallApiAsync(apiUrl, (string)null);
                if (!success || string.IsNullOrWhiteSpace(content))
                    return [];

                var transferParent = JsonConvert.DeserializeObject<TransferResponse>(content);
                if (transferParent?.Status != 4) // chỉ lấy nếu bị huỷ
                    return [];

                transferFind = transferCodeParent;
            }

            // Lấy danh sách TransferChecked phù hợp
            var transferCheckedList = await repository.GetQueryable<TransferChecked>()
                .AsNoTracking()
                .Where(x => x.TransferCode == transferFind
                            && x.BranchId == branchId
                            && x.UserName == userName
                            && x.Checked
                            && x.Transfer == transfer)
                .ToListAsync();

            return transferCheckedList;
        }

        public async Task<TransferChecked> GetCheckedProductByTransfer(long transferId, string transferCode, long branchId, string userName, bool transfer,
            string productBarCode, string productCode)
        {
            var transferChecked = await repository.GetQueryable<TransferChecked>().AsNoTracking().FirstOrDefaultAsync(x =>
                x.TransferCode == transferCode
                && x.ProductBarCode == productBarCode
                && x.ProductCode == productCode
                && x.BranchId == branchId
                && x.UserName == userName
                && x.Checked
                && x.Transfer == transfer
            );
            return transferChecked;
        }
    }
}
