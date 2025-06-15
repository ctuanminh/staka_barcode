using Newtonsoft.Json;

namespace Be.Common.Gateway.Misa
{
    public class SaOrder
    {
        [JsonProperty("org_company_code")]
        public string OrgCompanyCode { get; set; }
        [JsonProperty("app_id")]
        public string AppId { get; set; }
        [JsonProperty("voucher")]
        public List<VoucherOrder> Voucher { get; set; }
        [JsonProperty("dictionary")]
        public List<object> Dictionary { get; set; }
    }
    public class VoucherOrder
    {
        [JsonProperty("detail")]
        public List<VoucherOrderDetail> Detail { get; set; }

        [JsonProperty("voucher_type")]
        public int VoucherType { get; set; }

        [JsonProperty("is_get_new_id")]
        public bool IsGetNewId { get; set; }

        [JsonProperty("org_refid")]
        public string OrgRefid { get; set; }

        [JsonProperty("is_allow_group")]
        public bool IsAllowGroup { get; set; }

        [JsonProperty("org_refno")]
        public string OrgRefno { get; set; }

        [JsonProperty("org_reftype")]
        public int OrgReftype { get; set; }

        [JsonProperty("org_reftype_name")]
        public string OrgReftypeName { get; set; }

        [JsonProperty("refno_finance")]
        public string RefnoFinance { get; set; }

        [JsonProperty("act_voucher_type")]
        public int ActVoucherType { get; set; }

        [JsonProperty("refid")]
        public string Refid { get; set; }

        [JsonProperty("branch_id")]
        public string BranchId { get; set; }

        [JsonProperty("account_object_id")]
        public string AccountObjectId { get; set; }

        [JsonProperty("employee_id")]
        public string EmployeeId { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("delivered_status")]
        public int DeliveredStatus { get; set; }

        [JsonProperty("due_day")]
        public int DueDay { get; set; }

        [JsonProperty("refdate")]
        public DateTime Refdate { get; set; }

        [JsonProperty("delivery_date")]
        public DateTime DeliveryDate { get; set; }

        [JsonProperty("is_calculated_cost")]
        public bool IsCalculatedCost { get; set; }

        [JsonProperty("exchange_rate")]
        public decimal ExchangeRate { get; set; }

        [JsonProperty("total_amount_oc")]
        public decimal TotalAmountOc { get; set; }

        [JsonProperty("total_amount")]
        public decimal TotalAmount { get; set; }

        [JsonProperty("total_discount_amount_oc")]
        public decimal TotalDiscountAmountOc { get; set; }

        [JsonProperty("total_discount_amount")]
        public decimal TotalDiscountAmount { get; set; }

        [JsonProperty("total_vat_amount_oc")]
        public decimal TotalVatAmountOc { get; set; }

        [JsonProperty("total_vat_amount")]
        public decimal TotalVatAmount { get; set; }

        [JsonProperty("last_year_invoice_amount_oc")]
        public decimal LastYearInvoiceAmountOc { get; set; }

        [JsonProperty("last_year_invoice_amount")]
        public decimal LastYearInvoiceAmount { get; set; }

        [JsonProperty("refno")]
        public string Refno { get; set; }

        [JsonProperty("account_object_name")]
        public string AccountObjectName { get; set; }

        [JsonProperty("account_object_code")]
        public string AccountObjectCode { get; set; }

        [JsonProperty("journal_memo")]
        public string JournalMemo { get; set; }

        [JsonProperty("currency_id")]
        public string CurrencyId { get; set; }

        [JsonProperty("employee_code")]
        public string EmployeeCode { get; set; }

        [JsonProperty("employee_name")]
        public string EmployeeName { get; set; }

        [JsonProperty("discount_type")]
        public int DiscountType { get; set; }

        [JsonProperty("discount_rate_voucher")]
        public decimal DiscountRateVoucher { get; set; }

        [JsonProperty("total_sale_amount_oc")]
        public decimal TotalSaleAmountOc { get; set; }

        [JsonProperty("total_sale_amount")]
        public decimal TotalSaleAmount { get; set; }

        [JsonProperty("total_amount_made")]
        public decimal TotalAmountMade { get; set; }

        [JsonProperty("total_amount_made_oc")]
        public decimal TotalAmountMadeOc { get; set; }

        [JsonProperty("ccy_exchange_operator")]
        public bool CcyExchangeOperator { get; set; }

        [JsonProperty("organization_unit_id")]
        public string OrganizationUnitId { get; set; }

        [JsonProperty("has_create_contract")]
        public bool HasCreateContract { get; set; }

        [JsonProperty("organization_unit_type_id")]
        public int OrganizationUnitTypeId { get; set; }

        [JsonProperty("revenue_status")]
        public int RevenueStatus { get; set; }

        [JsonProperty("old_revenue_status")]
        public int OldRevenueStatus { get; set; }

        [JsonProperty("total_receipted_amount")]
        public decimal TotalReceiptedAmount { get; set; }

        [JsonProperty("total_receipted_amount_oc")]
        public decimal TotalReceiptedAmountOc { get; set; }

        [JsonProperty("total_invoice_amount")]
        public decimal TotalInvoiceAmount { get; set; }

        [JsonProperty("old_total_invoice_amount")]
        public decimal OldTotalInvoiceAmount { get; set; }

        [JsonProperty("total_invoice_amount_oc")]
        public decimal TotalInvoiceAmountOc { get; set; }

        [JsonProperty("old_total_invoice_amount_oc")]
        public decimal OldTotalInvoiceAmountOc { get; set; }

        [JsonProperty("is_invoiced")]
        public bool IsInvoiced { get; set; }

        [JsonProperty("old_is_invoiced")]
        public bool OldIsInvoiced { get; set; }

        [JsonProperty("old_delivered_status")]
        public int OldDeliveredStatus { get; set; }

        [JsonProperty("crm_id")]
        public string CrmId { get; set; }

        [JsonProperty("is_update_revenue")]
        public bool IsUpdateRevenue { get; set; }

        [JsonProperty("check_quantity")]
        public bool CheckQuantity { get; set; }

        [JsonProperty("excel_row_index")]
        public int ExcelRowIndex { get; set; }

        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }

        [JsonProperty("reftype")]
        public int Reftype { get; set; }

        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("auto_refno")]
        public bool AutoRefno { get; set; }

        [JsonProperty("state")]
        public int State { get; set; }
    }

    public class VoucherOrderDetail
    {
        [JsonProperty("ref_detail_id")]
        public string RefDetailId { get; set; }

        [JsonProperty("refid")]
        public string Refid { get; set; }

        [JsonProperty("inventory_item_id")]
        public string InventoryItemId { get; set; }

        [JsonProperty("unit_id")]
        public string UnitId { get; set; }

        [JsonProperty("main_unit_id")]
        public string MainUnitId { get; set; }

        [JsonProperty("organization_unit_id")]
        public string OrganizationUnitId { get; set; }

        [JsonProperty("sort_order")]
        public int SortOrder { get; set; }

        [JsonProperty("is_promotion")]
        public bool IsPromotion { get; set; }

        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }

        [JsonProperty("quantity_delivered_sa")]
        public decimal QuantityDeliveredSa { get; set; }

        [JsonProperty("quantity_delivered_in")]
        public decimal QuantityDeliveredIn { get; set; }

        [JsonProperty("panel_quantity")]
        public decimal PanelQuantity { get; set; }

        [JsonProperty("last_year_delivered_before_tax_amount_oc")]
        public decimal LastYearDeliveredBeforeTaxAmountOc { get; set; }

        [JsonProperty("last_year_delivered_before_tax_amount")]
        public decimal LastYearDeliveredBeforeTaxAmount { get; set; }

        [JsonProperty("panel_length_quantity")]
        public decimal PanelLengthQuantity { get; set; }

        [JsonProperty("panel_width_quantity")]
        public decimal PanelWidthQuantity { get; set; }

        [JsonProperty("panel_height_quantity")]
        public decimal PanelHeightQuantity { get; set; }

        [JsonProperty("panel_radius_quantity")]
        public decimal PanelRadiusQuantity { get; set; }

        [JsonProperty("main_quantity_delivered_sa")]
        public decimal MainQuantityDeliveredSa { get; set; }

        [JsonProperty("main_quantity_delivered_sa_last_year")]
        public decimal MainQuantityDeliveredSaLastYear { get; set; }

        [JsonProperty("main_quantity_delivered_in")]
        public decimal MainQuantityDeliveredIn { get; set; }

        [JsonProperty("main_quantity_delivered_in_last_year")]
        public decimal MainQuantityDeliveredInLastYear { get; set; }

        [JsonProperty("last_year_delivered_amount_oc")]
        public decimal LastYearDeliveredAmountOc { get; set; }

        [JsonProperty("last_year_delivered_amount")]
        public decimal LastYearDeliveredAmount { get; set; }

        [JsonProperty("quantity_delivered_in_last_year")]
        public decimal QuantityDeliveredInLastYear { get; set; }

        [JsonProperty("quantity_delivered_sa_last_year")]
        public decimal QuantityDeliveredSaLastYear { get; set; }

        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("unit_price_after_tax")]
        public decimal UnitPriceAfterTax { get; set; }

        [JsonProperty("amount_oc")]
        public decimal AmountOc { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("discount_rate")]
        public decimal DiscountRate { get; set; }

        [JsonProperty("main_convert_rate")]
        public decimal MainConvertRate { get; set; }

        [JsonProperty("main_quantity")]
        public decimal MainQuantity { get; set; }

        [JsonProperty("discount_amount_oc")]
        public decimal DiscountAmountOc { get; set; }

        [JsonProperty("discount_amount")]
        public decimal DiscountAmount { get; set; }

        [JsonProperty("vat_rate")]
        public decimal VatRate { get; set; }

        [JsonProperty("vat_amount_oc")]
        public decimal VatAmountOc { get; set; }

        [JsonProperty("vat_amount")]
        public decimal VatAmount { get; set; }

        [JsonProperty("main_unit_price")]
        public decimal MainUnitPrice { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("inventory_item_name")]
        public string InventoryItemName { get; set; }

        [JsonProperty("exchange_rate_operator")]
        public string ExchangeRateOperator { get; set; }

        [JsonProperty("inventory_item_code")]
        public string InventoryItemCode { get; set; }

        [JsonProperty("main_unit_name")]
        public string MainUnitName { get; set; }

        [JsonProperty("organization_unit_code")]
        public string OrganizationUnitCode { get; set; }

        [JsonProperty("unit_name")]
        public string UnitName { get; set; }

        [JsonProperty("is_unit_price_after_tax")]
        public bool IsUnitPriceAfterTax { get; set; }

        [JsonProperty("reftype")]
        public int Reftype { get; set; }

        [JsonProperty("account_object_id")]
        public string AccountObjectId { get; set; }

        [JsonProperty("quantity_delivered")]
        public decimal QuantityDelivered { get; set; }

        [JsonProperty("main_quantity_delivered")]
        public decimal MainQuantityDelivered { get; set; }

        [JsonProperty("quantity_remain")]
        public decimal QuantityRemain { get; set; }

        [JsonProperty("main_quantity_remain")]
        public decimal MainQuantityRemain { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("is_description")]
        public bool IsDescription { get; set; }

        [JsonProperty("discount_type")]
        public int DiscountType { get; set; }

        [JsonProperty("discount_rate_voucher")]
        public decimal DiscountRateVoucher { get; set; }

        [JsonProperty("organization_unit_name")]
        public string OrganizationUnitName { get; set; }

        [JsonProperty("is_description_import")]
        public bool IsDescriptionImport { get; set; }

        [JsonProperty("quantity_delivered_last_year")]
        public decimal QuantityDeliveredLastYear { get; set; }

        [JsonProperty("old_quantity_delivered_sa")]
        public decimal OldQuantityDeliveredSa { get; set; }

        [JsonProperty("old_quantity_delivered_in")]
        public decimal OldQuantityDeliveredIn { get; set; }

        [JsonProperty("old_main_quantity_delivered_sa")]
        public decimal OldMainQuantityDeliveredSa { get; set; }

        [JsonProperty("old_main_quantity_delivered_in")]
        public decimal OldMainQuantityDeliveredIn { get; set; }

        [JsonProperty("crm_id")]
        public string CrmId { get; set; }

        [JsonProperty("is_follow_serial_number")]
        public bool IsFollowSerialNumber { get; set; }

        [JsonProperty("is_allow_duplicate_serial_number")]
        public bool IsAllowDuplicateSerialNumber { get; set; }

        [JsonProperty("state")]
        public int State { get; set; }
    }
}
