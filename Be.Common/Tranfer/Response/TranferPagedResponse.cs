namespace Be.Common.Tranfer.Response
{
    public class TranferPagedResponse
    {
        public int Total { get; set; }
        public int PageSize { get; set; }
        public List<TransferResponse> Data { get; set; } = new List<TransferResponse>();
    }
    public class TransferDetail
    {
        public long ProductId { get; set; } // Id hàng hóa
        public string ProductCode { get; set; } // Mã hàng hóa
        public string ProductName { get; set; }
        public double SendQuantity { get; set; } // Số lượng hàng hóa chuyển
        public double TransferredQuantity { get; set; } // Số lượng hàng hóa nhận
        public decimal Price { get; set; } // Giá trị
        public decimal TotalTransfer { get; set; }
        public decimal TotalReceive { get; set; }
        public decimal SendPrice { get; set; } // Giá chuyển
        public decimal ReceivePrice { get; set; } // Giá nhận
        public bool Checked { get; set; }
        public string Unit { get; set; }
    }

    public class TransferResponse
    {
        public long Id { get; set; } // Id phiếu
        public string Code { get; set; } // Mã phiếu

        public int FromBranchId { get; set; } // Id chi nhánh chuyển
        public string FromBranchName { get; set; }

        public int ToBranchId { get; set; } // Id chi nhánh nhận
        public string ToBranchName { get; set; }
        public int Status { get; set; } // Trạng thái phiếu chuyển
        public string StatusValue
        {
            get
            {
                return Status switch
                {
                    1 => "Phiếu tạm",
                    2 => "Đang chuyển",
                    3 => "Hoàn thành",
                    _ => "Đã huỷ"
                };
            }
        }
        public long RetailerId { get; set; } // Id gian hàng
        public string Description { get; set; } // Ghi chú
        public DateTime? DispatchedDate { get; set; }
        public DateTime? ReceivedDate { get; set; }

        public List<TransferDetail> Details { get; set; } = [];
    }
}
