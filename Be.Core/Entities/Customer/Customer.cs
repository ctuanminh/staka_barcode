using Be.Core.BaseEntities;

namespace Be.Core.Entities.Customer
{
    public class Customer : AuditedEntity<long>
    {
        public string Code { get; set; } // Mã khách hàng
        public long KiotId { get; set; }

        public string Name { get; set; } // Tên khách hàng

        public int Type { get; set; } // Loại khách hàng (0: Cá nhân, 1: Công ty)

        public bool? Gender { get; set; } // true: Nam, false: Nữ

        public DateTime? BirthDate { get; set; } // Ngày sinh

        public string ContactNumber { get; set; } // SĐT chính

        public string SubNumber { get; set; } // SĐT phụ

        public string IdentificationNumber { get; set; } // CCCD/CMND

        public string Address { get; set; } // Địa chỉ

        public string LocationName { get; set; } // Khu vực

        public string? Email { get; set; }

        public string Organization { get; set; } // Tên công ty

        public string Comments { get; set; } // Ghi chú

        public string TaxCode { get; set; } // Mã số thuế

        public int RetailerId { get; set; } // ID cửa hàng

        public DateTime? ModifiedDate { get; set; } // Thời gian cập nhật

        public ICollection<CustomerGroupDetail> CustomerGroupDetails { get; set; }
    }

    public class CustomerGroup : AuditedEntity<long>
    {
        public string Name { get; set; } // Tên nhóm khách hàng
        public string Description { get; set; } // Mô tả nhóm khách hàng
        public ICollection<CustomerGroupDetail> CustomerGroupDetails { get; set; } // Chi tiết nhóm khách hàng
    }

    public class CustomerGroupDetail : AuditedEntity<long>
    {
        public long CustomerId { get; set; } // ID khách hàng

        public long GroupId { get; set; } // ID nhóm khách hàng
        public Customer Customer { get; set; }
        public CustomerGroup CustomerGroup { get; set; }
    }

}
