using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Be.Common.Responses.KiotViet
{
    public class ReturnInvoiceApiResponse
    {
        public int Total { get; set; }
        public int PageSize { get; set; } = 20; // mặc định 20, tối đa 100
        public List<ReturnInvoiceResponse> Data { get; set; }
    }
    public class ReturnInvoiceResponse
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public long? InvoiceId { get; set; }
        public DateTime ReturnDate { get; set; }

        public int BranchId { get; set; }
        public string BranchName { get; set; }

        public long ReceivedById { get; set; }
        public string SoldByName { get; set; }

        public long? CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }

        public decimal ReturnTotal { get; set; }
        public decimal TotalPayment { get; set; }

        public int Status { get; set; }
        public string StatusValue { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public List<Payment> Payments { get; set; }
        public List<ReturnDetail> ReturnDetails { get; set; }
    }
    public class Payment
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public byte? Status { get; set; }
        public string StatusValue { get; set; }
        public DateTime TransDate { get; set; }
        public string BankAccount { get; set; }
        public int? AccountId { get; set; }
        public string Description { get; set; }
    }
    public class ReturnDetail
    {
        public long ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public bool? UsePoint { get; set; }
        public decimal SubTotal { get; set; }
    }
}
