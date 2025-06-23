using Be.Core.Entities;
using Be.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Be.Services.Transfer
{
    public class TransferServiceImp(IRepository repository) : ITransferService
    {
        public async Task<TransferChecked> AddOrUpdateProductCheck(TransferChecked transferChecked)
        {
            var transferCheckedExist = await repository.FindAsync<TransferChecked, long>(transferChecked.Id);
            if (transferCheckedExist != null)
            {
                await repository.UpdateAsync(transferCheckedExist);
            }
            else
            {
                await repository.AddAsync(transferChecked);
            }
            await repository.SaveChangeAsync();
            return transferChecked;
        }

        public async Task<List<TransferChecked>> GetTransferChecks(string transferCode, long branchId, string userName)
        {
            var ogTransferCode = transferCode.ToString().Split(".")[0];
            var transferCheckedList = await repository.GetQueryable<TransferChecked>()
                .Where(x => x.TransferCode == ogTransferCode 
                            && x.BranchId == branchId
                            && x.UserName == userName
                            && x.Checked).ToListAsync();
            return transferCheckedList;
        }
    }
}
