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

        public async Task<TransferChecked> AddOrUpdateProductCheck(TransferChecked transferChecked)
        {
            var existing = await repository.FindAsync<TransferChecked, long>(transferChecked.Id);

            if (existing == null)
                await repository.AddAsync(transferChecked);
            else
                await repository.UpdateAsync(existing);

            await repository.SaveChangeAsync();
            return transferChecked;
        }

        public async Task<List<TransferChecked>> GetCheckedProductsByParentTransfer(long transferId, string transferCode, long branchId, string userName, bool transfer)
        {
            //Tách transferCode để tìm transfer cha ví dụ: xxx.01. thì lấy xxx
            var transferCodeParent = transferCode.Split(".")[0];
            //Tìm transferId của transfer cha. Để lấy Id.
            var transferLocal = await repository.GetQueryable<TransferEntity>()
                .FirstOrDefaultAsync(t => t.TransferCode == transferCodeParent);
            if(transferLocal == null) return [];
            //Call Api để kiểm tra xem transfer cha có tồn tại và status = 4 hay không.
            var apiUrl = $"https://public.kiotapi.com/transfers/{transferLocal.TransferId}";
            var (success, content) = await _kiotVietService.CallApiAsync(apiUrl, (string)null);
            if (!success || string.IsNullOrEmpty(content)) return [];

            var transferParent = JsonConvert.DeserializeObject<TransferResponse>(content);
            if (transferParent.Status != 4) return [];
            // Tìm danh sách TransferChecked.
            var transferCheckedList = await repository.GetQueryable<TransferChecked>()
                .Where(x => x.TransferCode == transferCodeParent
                            && x.BranchId == branchId
                            && x.UserName == userName
                            && x.Checked
                            && x.Transfer == transfer
                ).ToListAsync();
            return transferCheckedList;
        }

        public async Task<TransferChecked> GetCheckedProductByTransfer(long transferId, string transferCode, long branchId, string userName, bool transfer,
            string productBarCode)
        {
            var transferChecked = await repository.GetQueryable<TransferChecked>().FirstOrDefaultAsync(x =>
                x.TransferCode == transferCode
                && x.ProductBarCode == productBarCode
                && x.BranchId == branchId
                && x.UserName == userName
                && x.Checked
                && x.Transfer == transfer
            );
            return transferChecked;
        }
    }
}
