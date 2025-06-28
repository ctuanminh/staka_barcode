using Be.Common.Order.Dto;
using Be.Core.Entities;
using Be.Data.Repository;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;

namespace Be.Services.Order
{
    public class OrderCheckedServiceImp(IRepository repository) : IOrderCheckedService
    {
        public async Task<bool> IsOrderChecked(long orderId, long branchId)
        {
            var query = await repository.GetQueryable<OrderCheckedEntity>()
                .FirstOrDefaultAsync(c => c.OrderId == orderId
                    && c.BranchId == branchId);
            return query != null;
        }

        public async Task<OrderCheckedDto> FindProductChecked(long orderId, string productBarCode, long branchId)
        {
            var productChecked = await repository.GetQueryable<OrderCheckedEntity>()
                .FirstOrDefaultAsync(c => c.OrderId == orderId
                                          && c.BranchId == branchId
                                          && c.ProductBarCode == productBarCode)
                .Select(p => new OrderCheckedDto()
                {
                    Id = p.Id,
                    ProductCode = p.ProductCode,
                    Count = p.Count,
                });
            return productChecked;
        }

        public async Task<List<OrderCheckedDto>> GetOrderCheckedByOrderId(long orderId, long branchId)
        {
            var query = await repository.GetQueryable<OrderCheckedEntity>()
                .Where(c => c.OrderId == orderId && c.BranchId == branchId )
                .Select(c => new OrderCheckedDto
                {
                    OrderId = c.OrderId,
                    ProductBarCode = c.ProductBarCode,
                    ProductCode = c.ProductCode,
                    BranchId = c.BranchId,
                    Count = c.Count,
                })
                .ToListAsync();
            return query;
        }

        public async Task<OrderCheckedDto> AddOrderCheck(OrderCheckedDto orderChecked)
        {
            var entity = new OrderCheckedEntity
            {
                OrderId = orderChecked.OrderId,
                OrderCode = orderChecked.OrderCode,
                ProductBarCode = orderChecked.ProductBarCode,
                ProductCode = orderChecked.ProductCode,
                BranchId = orderChecked.BranchId,
                UserName = orderChecked.UserName,
                Count = orderChecked.Count
            };
            await repository.AddAsync(entity);
            await repository.SaveChangeAsync();
            return new OrderCheckedDto
            {
                OrderId = entity.OrderId,
                ProductBarCode = entity.ProductBarCode,
                ProductCode = entity.ProductCode,
                BranchId = entity.BranchId,
                UserName = entity.UserName,
                Count = entity.Count
            };
        }

        public async Task<bool> UpdateOrderCheck(long orderCheckedId, double count)
        {
            var existing = await repository.FindAsync<OrderCheckedEntity>(orderCheckedId);
            if (existing == null) return false;
            existing.Count = count;
            await repository.UpdateAsync(existing);
            await repository.SaveChangeAsync();
            return true;
        }
    }
}
