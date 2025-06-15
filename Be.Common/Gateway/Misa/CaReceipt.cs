using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Be.Common.Gateway.Misa
{

    //Phiếu thu chi (CaReceipt) là một đối tượng trong hệ thống kế toán,
    //dùng để ghi nhận các giao dịch thu chi tiền mặt hoặc chuyển khoản.
    //Dưới đây là mô tả chi tiết về cấu trúc của CaReceipt và các thành phần liên quan.
    public class CaReceipt
    {
        [JsonProperty("org_company_code")]
        public string OrgCompanyCode { get; set; }

        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("voucher")]
        public List<VoucherReceipt> Voucher { get; set; }

        [JsonProperty("dictionary")]
        public List<object> Dictionary { get; set; }
    }

    public class VoucherReceipt
    {
        [JsonProperty("detail")]
        public List<VoucherReceiptDetail> Detail { get; set; } 

        [JsonProperty("voucher_type")]
        public int VoucherType { get; set; }

        [JsonProperty("is_get_new_id")]
        public bool IsGetNewId { get; set; }

        [JsonProperty("org_refid")]
        public string OrgRefId { get; set; }

        [JsonProperty("is_allow_group")]
        public bool IsAllowGroup { get; set; }

        [JsonProperty("org_refno")]
        public string OrgRefNo { get; set; }

        [JsonProperty("org_reftype")]
        public int OrgRefType { get; set; }

        [JsonProperty("org_reftype_name")]
        public string OrgRefTypeName { get; set; }

        [JsonProperty("refno")]
        public string RefNo { get; set; }

        [JsonProperty("act_voucher_type")]
        public int ActVoucherType { get; set; }

        [JsonProperty("refid")]
        public string RefId { get; set; }

        [JsonProperty("account_object_id")]
        public string AccountObjectId { get; set; }

        [JsonProperty("branch_id")]
        public string BranchId { get; set; }

        [JsonProperty("employee_id")]
        public string EmployeeId { get; set; }

        [JsonProperty("reason_type_id")]
        public int ReasonTypeId { get; set; }

        [JsonProperty("display_on_book")]
        public int DisplayOnBook { get; set; }

        [JsonProperty("reforder")]
        public long RefOrder { get; set; }

        [JsonProperty("refdate")]
        public DateTime RefDate { get; set; }

        [JsonProperty("posted_date")]
        public DateTime PostedDate { get; set; }

        [JsonProperty("is_posted_finance")]
        public bool IsPostedFinance { get; set; }

        [JsonProperty("is_posted_management")]
        public bool IsPostedManagement { get; set; }

        [JsonProperty("is_posted_cash_book_finance")]
        public bool IsPostedCashBookFinance { get; set; }

        [JsonProperty("is_posted_cash_book_management")]
        public bool IsPostedCashBookManagement { get; set; }

        [JsonProperty("exchange_rate")]
        public double ExchangeRate { get; set; }

        [JsonProperty("total_amount_oc")]
        public double TotalAmountOc { get; set; }

        [JsonProperty("total_amount")]
        public double TotalAmount { get; set; }

        [JsonProperty("refno_finance")]
        public string RefNoFinance { get; set; }

        [JsonProperty("refno_management")]
        public string RefNoManagement { get; set; }

        [JsonProperty("account_object_name")]
        public string AccountObjectName { get; set; }

        [JsonProperty("account_object_address")]
        public string AccountObjectAddress { get; set; }

        [JsonProperty("account_object_contact_name")]
        public string AccountObjectContactName { get; set; }

        [JsonProperty("account_object_code")]
        public string AccountObjectCode { get; set; }

        [JsonProperty("journal_memo")]
        public string JournalMemo { get; set; }

        [JsonProperty("document_included")]
        public string DocumentIncluded { get; set; }

        [JsonProperty("currency_id")]
        public string CurrencyId { get; set; }

        [   JsonProperty("employee_code")]
        public string EmployeeCode { get; set; }

        [JsonProperty("employee_name")]
        public string EmployeeName { get; set; }

        [JsonProperty("ca_audit_refid")]
        public string CaAuditRefId { get; set; }

        [JsonProperty("excel_row_index")]
        public int ExcelRowIndex { get; set; }

        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }

        [JsonProperty("reftype")]
        public int RefType { get; set; }

        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("modified_by")]
        public string ModifiedBy { get; set; }

        [JsonProperty("auto_refno")]
        public bool AutoRefNo { get; set; }

        [JsonProperty("state")]
        public int State { get; set; }
    }
    public class VoucherReceiptDetail
    {
        [JsonProperty("ref_detail_id")]
        public string RefDetailId { get; set; }

        [JsonProperty("refid")]
        public string RefId { get; set; }

        [JsonProperty("account_object_id")]
        public string AccountObjectId { get; set; }

        [JsonProperty("sort_order")]
        public int SortOrder { get; set; }

        [JsonProperty("un_resonable_cost")]
        public bool UnResonableCost { get; set; }

        [JsonProperty("amount_oc")]
        public double AmountOc { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("cash_out_amount_finance")]
        public double CashOutAmountFinance { get; set; }

        [JsonProperty("cash_out_diff_amount_finance")]
        public double CashOutDiffAmountFinance { get; set; }

        [JsonProperty("cash_out_amount_management")]
        public double CashOutAmountManagement { get; set; }

        [JsonProperty("cash_out_diff_amount_management")]
        public double CashOutDiffAmountManagement { get; set; }

        [JsonProperty("cash_out_exchange_rate_finance")]
        public double CashOutExchangeRateFinance { get; set; }

        [JsonProperty("cash_out_exchange_rate_management")]
        public double CashOutExchangeRateManagement { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("debit_account")]
        public string DebitAccount { get; set; }

        [JsonProperty("credit_account")]
        public string CreditAccount { get; set; }

        [JsonProperty("account_object_code")]
        public string AccountObjectCode { get; set; }

        [JsonProperty("state")]
        public int State { get; set; }
    }
    public class DictionaryItem
    {
        [JsonProperty("dictionary_type")]
        public int DictionaryType { get; set; }

        [JsonProperty("account_object_id")]
        public string AccountObjectId { get; set; }

        [JsonProperty("due_time")]
        public int DueTime { get; set; }

        [JsonProperty("account_object_type")]
        public int AccountObjectType { get; set; }

        [JsonProperty("is_vendor")]
        public bool IsVendor { get; set; }

        [JsonProperty("is_customer")]
        public bool IsCustomer { get; set; }

        [JsonProperty("is_employee")]
        public bool IsEmployee { get; set; }

        [JsonProperty("inactive")]
        public bool Inactive { get; set; }

        [JsonProperty("agreement_salary")]
        public double AgreementSalary { get; set; }

        [JsonProperty("salary_coefficient")]
        public double SalaryCoefficient { get; set; }

        [JsonProperty("insurance_salary")]
        public double InsuranceSalary { get; set; }

        [JsonProperty("maximize_debt_amount")]
        public double MaximizeDebtAmount { get; set; }

        [JsonProperty("receiptable_debt_amount")]
        public double ReceiptableDebtAmount { get; set; }

        [JsonProperty("account_object_code")]
        public string AccountObjectCode { get; set; }

        [JsonProperty("account_object_name")]
        public string AccountObjectName { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("is_same_address")]
        public bool IsSameAddress { get; set; }

        [JsonProperty("pay_account")]
        public string PayAccount { get; set; }

        [JsonProperty("receive_account")]
        public string ReceiveAccount { get; set; }

        [JsonProperty("closing_amount")]
        public double ClosingAmount { get; set; }

        [JsonProperty("reftype")]
        public int RefType { get; set; }

        [JsonProperty("reftype_category")]
        public int RefTypeCategory { get; set; }

        [JsonProperty("branch_id")]
        public string BranchId { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }
    }

}
