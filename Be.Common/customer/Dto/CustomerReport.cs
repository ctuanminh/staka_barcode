namespace Be.Common.customer.Dto
{
    public partial class CustomerReport
    {

        public string GroupId { get; set; } // Nhóm khách hàng (ID)
        public string GroupName { get; set; } // Tên nhóm khách hàng        
        public int TotalCustomers { get; set; } // Tổng số khách hàng
        public int TotalCustomersReference { get; set; } // Tổng số khách hàng tháng cần so sánh
        public decimal CustomerGrowthPercent { get; set; } // Tỷ lệ tăng trưởng khách hàng
        public decimal CustomerBuybackPercent { get; set; } // Tỷ lệ khách hàng mua lại
        public decimal CustomerNoBuybackPercent { get; set; } // Tỷ lệ khách hàng không mua lại
        public decimal CustomerGroupRevenueRateMonth { get; set; } //Tỉ lệ doanh thu nhóm khách hàng/ tổng doanh thu chi nhánh của tháng hiện tại
        public decimal RevenueGrowthRate { get; set; } // Tỉ lệ tăng trưởng doanh thu
        public decimal RevenueMonth { get; set; } // Doanh thu tháng hiện tại
        public decimal RevenueMonthReference { get; set; } // Doanh thu tháng cần so sánh        
        public string BranchName { get; set; } // Tên chi nhánh
    }
}
