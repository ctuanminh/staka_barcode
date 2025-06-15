namespace Be.Common.Request.Gateway
{
    public class KiotVietUpdatePriceBookRequest
    {
        public string Id { get; set; }
        public int Attempt { get; set; }
        public List<PriceBookNotification> Notifications { get; set; }
        public class PriceBookNotification
        {
            public string Action { get; set; }
            public List<PriceBookData> Data { get; set; }
        }

        public class PriceBookData
        {
            public long Id { get; set; }                   // Id bảng giá
            public string Name { get; set; }               // Tên bảng giá
            public bool IsActive { get; set; }             // Bảng giá đang hoạt động
            public bool IsGlobal { get; set; }             // Là bảng giá chung
            public DateTime StartDate { get; set; }        // Ngày bắt đầu
            public DateTime EndDate { get; set; }          // Ngày kết thúc
            public bool ForAllCusGroup { get; set; }       // Áp dụng cho tất cả nhóm KH
            public bool ForAllUser { get; set; }           // Áp dụng cho tất cả người dùng

            public List<PriceBookBranch> PriceBookBranches { get; set; }
            public List<PriceBookCustomerGroup> PriceBookCustomerGroups { get; set; }
            public List<PriceBookUser> PriceBookUsers { get; set; }
        }

        public class PriceBookBranch
        {
            public long Id { get; set; }                   // Id quan hệ
            public long PriceBookId { get; set; }          // Id bảng giá
            public long BranchId { get; set; }             // Id chi nhánh
            public string BranchName { get; set; }         // Tên chi nhánh
        }

        public class PriceBookCustomerGroup
        {
            public long Id { get; set; }                   // Id quan hệ
            public long PriceBookId { get; set; }          // Id bảng giá
            public long CustomerGroupId { get; set; }      // Id nhóm KH
            public string CustomerGroupName { get; set; }  // Tên nhóm KH
        }

        public class PriceBookUser
        {
            public long Id { get; set; }                   // Id quan hệ
            public long PriceBookId { get; set; }          // Id bảng giá
            public long UserId { get; set; }               // Id người dùng
            public string UserName { get; set; }           // Tên người dùng
        }
    }
}
