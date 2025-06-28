using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Be.Common.Order.Dto;

namespace Be.Services.Order
{
    public interface IOrderCheckedService
    {
        Task<bool> IsOrderChecked(long orderId, long branchId);
        Task<OrderCheckedDto> FindProductChecked(long orderId, string productBarCode, long branchId);
        Task<List<OrderCheckedDto>> GetOrderCheckedByOrderId(long orderId, long branchId);
        Task<OrderCheckedDto> AddOrderCheck(OrderCheckedDto orderChecked);
        Task<bool> UpdateOrderCheck(long orderCheckedId, double count);
    }
}
