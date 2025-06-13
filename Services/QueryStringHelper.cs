using Microsoft.AspNetCore.WebUtilities;
using System.Collections;
using System.Reflection;

namespace Services
{
    public class QueryStringHelper
    {
        public static string BuildQueryString<T>(T obj, string baseUrl)
        {
            var queryParams = new List<KeyValuePair<string, string>>();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);
                if (value == null) continue;
                var paramName = char.ToLower(prop.Name[0]) + prop.Name.Substring(1);

                switch (value)
                {
                    case IEnumerable enumerable when !(value is string):
                    {
                        foreach (var item in enumerable)
                        {
                            switch (item)
                            {
                                case null:
                                    continue;
                                // Nếu item là DateTime, format đúng chuẩn
                                case DateTime dtItem:
                                    queryParams.Add(new KeyValuePair<string, string>(
                                        paramName,
                                        dtItem.ToString("yyyy-MM-ddTHH:mm:ss")));
                                    break;
                                default:
                                    queryParams.Add(new KeyValuePair<string, string>(
                                        paramName,
                                        item.ToString()));
                                    break;
                            }
                        }

                        break;
                    }
                    // Xử lý riêng nếu là DateTime hoặc Nullable<DateTime>
                    case DateTime dt:
                        queryParams.Add(new KeyValuePair<string, string>(
                            paramName,
                            dt.ToString("yyyy-MM-ddTHH:mm:ss")));
                        break;
                    default:
                    {
                        if (value is DateTime?)
                        {
                            var nullableDt = (DateTime?)value;
                            queryParams.Add(new KeyValuePair<string, string>(
                                paramName,
                                nullableDt.Value.ToString("yyyy-MM-ddTHH:mm:ss")));
                        }
                        else
                        {
                            var stringValue = value.ToString();
                            if (!string.IsNullOrEmpty(stringValue))
                            {
                                queryParams.Add(new KeyValuePair<string, string>(
                                    paramName,
                                    stringValue));
                            }
                        }

                        break;
                    }
                }
            }

            return QueryHelpers.AddQueryString(baseUrl, queryParams!);
        }
    }
}
