using AutoMapper;
using Be.Common.Dtos.Product;
using Be.Common.Responses;
using Be.Common.Supply.Dto;
using Be.Common.Supply.Request;
using Be.Common.Supply.Response;
using Be.Core.Entities;
using Be.Data.Repository;
using Be.Services.KiotViet;
using Microsoft.EntityFrameworkCore;

namespace Be.Services.Supplier
{
    public class SupplierServiceImp(IRepository repository, IKiotVietService kiotVietService, IMapper Mapper)
        : ServiceResponse, ISupplyService
    {
        public async Task<bool> SynsSupplier()
        {
            var request = new SearchSupplierRequest()
            {
                PageSize = 200,
                CurrentItem = 0,
            };
            const string url = "https://public.kiotapi.com/suppliers";
            var currentPage = 1;
            int totalPages;
            const int pageSize = 200;
            var supplierList = new List<SupplierResponse>();
            do
            {
                request.CurrentItem = (currentPage - 1) * request.PageSize;
                var (success, content) = await kiotVietService.CallApiAsync(url, request);
                if (!success || string.IsNullOrWhiteSpace(content)) return false;
                var suppliersPaged = Newtonsoft.Json.JsonConvert.DeserializeObject<SupplierPagedData>(content);
                totalPages = (int)Math.Ceiling((double)suppliersPaged.Total / pageSize);
                supplierList.AddRange(suppliersPaged.Data);
                currentPage++;
            } while (currentPage <= totalPages);

            var existIds = await repository.GetQueryable<SupplierEntity>()
                .Select(s => s.KiotId)
                .ToListAsync();
            
            var newSuppliers = new List<SupplierEntity>();
            foreach (var supplierResponse in supplierList)
            {
                if (existIds.Contains(supplierResponse.Id))
                {
                    var existingEntity = await repository.GetQueryable<SupplierEntity>()
                        .FirstOrDefaultAsync(s => s.KiotId == supplierResponse.Id);

                    if (existingEntity == null) continue;
                    Mapper.Map(supplierResponse, existingEntity);
                    await repository.UpdateAsync(existingEntity);
                }
                else
                {
                    var supplierEntity = Mapper.Map<SupplierEntity>(supplierResponse);
                    supplierEntity.KiotId = supplierResponse.Id;
                    newSuppliers.Add(supplierEntity);
                }
            }

            if (newSuppliers.Any())
                await repository.AddRangeAsync(newSuppliers);

            await repository.SaveChangeAsync();
            return true;
        }

        public async Task<bool> SynsSupplyById(long supplyId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SynsSupplyByCode(string supplyCode)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SupplierDto>> GetSuppliers(bool isActive = true)
        {
            var suppliers = await repository.GetQueryable<SupplierEntity>()
                .Where(s => s.IsActive)
                .ToListAsync();
            var result = Mapper.Map<List<SupplierDto>>(suppliers);
            return result;
        }

        public async Task<SupplierEntity> GetSupplierByCode(long id)
        {
            var supplier = await repository.GetQueryable<SupplierEntity>()
                .FirstOrDefaultAsync(s => s.KiotId == id);
            return supplier;
        }
    }
}
