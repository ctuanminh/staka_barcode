using Be.Common.Supply.Dto;

namespace Be.Services.Supplier
{
    public interface ISupplyService
    {
        Task<bool> SynsSupplier();
        Task<bool> SynsSupplyById(long supplyId);
        Task<bool> SynsSupplyByCode(string supplyCode);
        Task<List<SupplierDto>> GetSuppliers(bool isActive = true);
    }
}
