using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Be.Common.Tranfer.Request
{
    public class SearchTranferRequest
    {
        public int[]? ToBranchIds { get; set; } // IDs chi nhánh nhận
        public int[]? FromBranchIds { get; set; } // IDs chi nhánh chuyển
        public int[]? Status { get; set; } // Tình trạng phiếu chuyển

        public int? PageSize { get; set; } = 20; // Số items trong 1 trang, mặc định 20 items, tối đa 100 items
        public int? CurrentItem { get; set; } // Lấy dữ liệu từ bản ghi currentItem

        public DateTime? FromReceivedDate { get; set; } // Từ thời gian nhận chuyển hàng
        public DateTime? ToReceivedDate { get; set; } // Đến thời gian nhận chuyển hàng
        public DateTime? FromTransferDate { get; set; } // Từ thời gian chuyển hàng
        public DateTime? ToTransferDate { get; set; } // Đến thời gian chuyển hàng
    }
}
