using Be.Common.Supply.Dto;
using Be.Core.Entities;

namespace Be.Services.Supplier
{
    public interface ISupplyService
    {
        Task<bool> SynsSupplier();
        Task<bool> SynsSupplyById(long supplyId);
        Task<bool> SynsSupplyByCode(string supplyCode);
        Task<List<SupplierDto>> GetSuppliers(bool isActive = true);
        Task<SupplierEntity> GetSupplierByCode(long id);
    }
}
