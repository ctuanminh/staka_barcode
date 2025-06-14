using System;
using System.Globalization;

namespace FrmMain.Utils
{
    public static class NumberFormatter
    {
        // Định dạng số nguyên có phân cách hàng nghìn (vd: 1,234)
        public static string FormatInteger(int value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }

        // Định dạng số thực với số chữ số thập phân tùy ý (vd: 1,234.56)
        public static string FormatDecimal(decimal value, int decimalPlaces = 0)
        {
            return value.ToString("N" + decimalPlaces, CultureInfo.InvariantCulture);
        }

        // Định dạng số theo văn hóa Việt Nam (1.000.000,50)
        public static string FormatDecimalVN(decimal value, int decimalPlaces = 2)
        {
            var culture = new CultureInfo("vi-VN");
            return value.ToString("N" + decimalPlaces, culture);
        }

        // Parse chuỗi thành số nguyên
        public static int ParseInteger(string input)
        {
            return int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int result) ? result : 0;
        }

        // Parse chuỗi thành số thực
        public static decimal ParseDecimal(string input)
        {
            return decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result) ? result : 0m;
        }

        public static decimal Ceiling(decimal value, int decimals = 2)
        {
            var factor = (decimal)Math.Pow(10, decimals);
            return Math.Ceiling(value * factor) / factor;
        }
    }
}
