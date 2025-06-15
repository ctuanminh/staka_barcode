using System.Net.Http;
using Be.Common.Branch.Request;
using Be.Common.Branch.Response;
using Be.Common.Responses;
using Be.Common.utils;
using Be.Core.Entities;
using Be.Data.Repository;
using Be.Services.KiotViet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PuppeteerSharp;

namespace Be.Services.Pos
{
    public partial class BranchServiceImp(IRepository repository, IKiotVietService kiotvietService)
        : ServiceResponse, IBranchService
    {
        public Task<ApiResponse> CreateBranch(BranchRequest branchRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> DeleteBranch(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves all branches from the database.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ApiResponse> GetAllBranches()
        {
            var query = await repository.GetQueryable<Branch>().ToListAsync();
            return Ok(query);
        }

        public Task<ApiResponse> GetBranchById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetBranchesByCompany(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetBranchesByUser(int userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Synchronizes branches from KiotViet API to the local database.
        /// </summary>
        /// <param name="branchRequest"></param>
        /// <returns></returns>
        public async Task<ApiResponse> SyncBranch(BranchRequest branchRequest)
        {
            const string baseUrl = "https://public.kiotapi.com/branches";

            var (success, content) = await kiotvietService.CallApiAsync(baseUrl, branchRequest);
            if (success == false && content == null)
            {
                return BadRequest("Có lỗi trong quá trình thực hiện");
            }
            var branchPagedResponse = JsonConvert.DeserializeObject<BranchPagedResponse>(content);

            var branchList = new List<Branch>();
            foreach (var item in branchPagedResponse.Data)
            {
                var branchExist = repository.GetQueryable<Branch>()
                    .FirstOrDefault(x => x.BranchId == item.Id);
                if (branchExist is null)
                {
                    var branch = new Branch()
                    {
                        Id = item.Id,
                        BranchId = item.Id,
                        RetailerId = item.RetailerId,
                        BranchName = item.BranchName,
                        Address = item.Address,
                        ContactNumber = item.ContactNumber,
                        Status = item.Status,
                        CreatedAt = item.CreatedDate,
                        UpdatedAt = item.ModifiedDate
                    };
                    branchList.Add(branch);
                }
                else
                {
                    branchExist.BranchName = item.BranchName;
                    branchExist.Address = item.Address;
                    branchExist.ContactNumber = item.ContactNumber;
                    branchExist.Status = item.Status;
                    branchExist.UpdatedAt = item.ModifiedDate;
                    await repository.UpdateAsync(branchExist);
                    await repository.SaveChangeAsync();
                }
            }
            await repository.AddRangeAsync(branchList);
            await repository.SaveChangeAsync();
            return Ok(branchList);
        }

        public Task<ApiResponse> UpdateBranch(BranchRequest branchRequest)
        {
            throw new NotImplementedException();
        }
    }
}
