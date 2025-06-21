using DevExpress.XtraReports.UI;
using System.Drawing;
using System.Linq;
using Be.Common.Order.Response;
using DevExpress.XtraPrinting;

namespace FrmMain.Report
{
    public partial class OrderReport : XtraReport
    {
        public OrderReport(OrderResponse order)
        {
            this.Margins = new System.Drawing.Printing.Margins(5, 5, 20, 20);

            this.Bands.AddRange(new Band[] {
            new TopMarginBand(),
            new BottomMarginBand(),
            new DetailBand() { HeightF = 30 },
            new ReportHeaderBand() { HeightF = 300 },
            new ReportFooterBand() { HeightF = 200 }
        });

            var fontHeader = new Font("Arial", 12, FontStyle.Bold);
            using var fontTitle = new Font("Arial", 14, FontStyle.Bold);
            var fontNormal = new Font("Arial", 10);

            var reportHeader = this.Bands[BandKind.ReportHeader] as ReportHeaderBand;
            const float marginLeft = 5;
            var pageWidth = this.PageWidth - this.Margins.Left - this.Margins.Right;

            var lblCompany = new XRLabel
            {
                Text = "CÔNG TY CỔ PHẦN STAKA (Khánh Anh)",
                Font = fontHeader,
                BoundsF = new RectangleF(marginLeft, 0, pageWidth, 30),
                TextAlignment = TextAlignment.MiddleCenter
            };
            reportHeader.Controls.Add(lblCompany);

            var lblPhone = new XRLabel
            {
                Text = "Điện thoại: 08323.88.7777 - 08333.88.777",
                Font = fontNormal,
                BoundsF = new RectangleF(marginLeft, 30, pageWidth, 20),
                TextAlignment = TextAlignment.MiddleCenter
            };
            reportHeader.Controls.Add(lblPhone);

            var lblTitle = new XRLabel
            {
                Text = "HÓA ĐƠN NHẬT HÀNG",
                Font = fontTitle,
                BoundsF = new RectangleF(marginLeft, 60, pageWidth, 30),
                TextAlignment = TextAlignment.MiddleCenter
            };
            reportHeader.Controls.Add(lblTitle);

            var lblCode = new XRLabel
            {
                Text = $"Số hóa đơn: {order.Code}",
                Font = fontNormal,
                BoundsF = new RectangleF(marginLeft, 100, pageWidth, 20)
            };
            reportHeader.Controls.Add(lblCode);

            var lblCustomer = new XRLabel
            {
                Text = $"Khách hàng: {order.CustomerName}",
                Font = fontNormal,
                BoundsF = new RectangleF(marginLeft, 130, pageWidth, 20)
            };
            reportHeader.Controls.Add(lblCustomer);

            var lblDate = new XRLabel
            {
                Text = $"Ngày lập hóa đơn: {order.PurchaseDate:dd/MM/yyyy HH:mm}",
                Font = fontNormal,
                BoundsF = new RectangleF(marginLeft, 160, pageWidth, 20)
            };
            reportHeader.Controls.Add(lblDate);

            var lblSoldBy = new XRLabel
            {
                Text = $"NVBH: {order.SoldByName}",
                Font = fontNormal,
                BoundsF = new RectangleF(marginLeft, 190, pageWidth, 20)
            };
            reportHeader.Controls.Add(lblSoldBy);

            var tableHeader = new XRTable { BoundsF = new RectangleF(marginLeft, 220, pageWidth, 30) };
            var headerRow = new XRTableRow();
            tableHeader.Rows.Add(headerRow);

            headerRow.Cells.Add(CreateCell("STT", fontHeader, 40));
            headerRow.Cells.Add(CreateCell("Mã hàng", fontHeader, 80));
            headerRow.Cells.Add(CreateCell("Tên hàng", fontHeader, 300));
            headerRow.Cells.Add(CreateCell("Vị trí", fontHeader, 80));
            headerRow.Cells.Add(CreateCell("ĐVT", fontHeader, 50));
            headerRow.Cells.Add(CreateCell("SL", fontHeader, 50));

            reportHeader.Controls.Add(tableHeader);

            var detailTable = new XRTable { BoundsF = new RectangleF(marginLeft, 0, pageWidth, 30) };
            var detailRow = new XRTableRow();
            detailTable.Rows.Add(detailRow);

            detailRow.Cells.Add(CreateCellWithExpression("RowNumber()", fontNormal, 40));
            detailRow.Cells.Add(CreateCell("[ProductCode]", fontNormal, 80));
            detailRow.Cells.Add(CreateCell("[ProductName]", fontNormal, 300));
            detailRow.Cells.Add(CreateCell("[Location]", fontNormal, 80));
            detailRow.Cells.Add(CreateCell("[Unit]", fontNormal, 50));
            detailRow.Cells.Add(CreateCell("[Quantity]", fontNormal, 50));

            this.Bands[BandKind.Detail].Controls.Add(detailTable);

            this.DataSource = order.OrderDetails;

            var reportFooter = this.Bands[BandKind.ReportFooter] as ReportFooterBand;

            var lblTotal = new XRLabel
            {
                Text = $"Tổng tiền hàng: {order.Total:N0}",
                Font = fontNormal,
                BoundsF = new RectangleF(marginLeft, 10, pageWidth, 20)
            };
            reportFooter.Controls.Add(lblTotal);

            var lblDiscount = new XRLabel
            {
                Text = $"Chiết khấu: {order.Discount:N0}",
                Font = fontNormal,
                BoundsF = new RectangleF(marginLeft, 35, pageWidth, 20)
            };
            reportFooter.Controls.Add(lblDiscount);

            var lblPayment = new XRLabel
            {
                Text = $"Đã thanh toán: {order.TotalPayment:N0}",
                Font = fontNormal,
                BoundsF = new RectangleF(marginLeft, 60, pageWidth, 20)
            };
            reportFooter.Controls.Add(lblPayment);

            var lblDebt = new XRLabel
            {
                Text = $"Nợ còn lại: {order.Total:N0}",
                Font = fontNormal,
                BoundsF = new RectangleF(marginLeft, 85, pageWidth, 20)
            };
            reportFooter.Controls.Add(lblDebt);
        }

        private XRTableCell CreateCell(string text, Font font, float width)
        {
            return new XRTableCell
            {
                Text = text,
                Font = font,
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                TextAlignment = TextAlignment.MiddleCenter,
                WidthF = width
            };
        }
        private XRTableCell CreateCellWithExpression(string expression, Font font, float width)
        {
            var cell = new XRTableCell
            {
                Font = font,
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                TextAlignment = TextAlignment.MiddleCenter,
                WidthF = width
            };
            cell.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", expression));
            return cell;
        }

        public class OrderDetailReportView
        {
            public int STT { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string Location { get; set; }
            public string Unit { get; set; }
            public double Quantity { get; set; }
            // ... các trường khác nếu cần
        }
    }
}
