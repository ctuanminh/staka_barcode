using System.Collections;
using System.Reflection;
using Microsoft.AspNetCore.WebUtilities;

namespace Be.Common.utils
{
    public static class QueryStringHelper
    {
        public static string BuildQueryString<T>(T obj, string baseUrl)
        {
            var queryParams = new List<KeyValuePair<string, string>>();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);
                if (value != null)
                {
                    var paramName = char.ToLower(prop.Name[0]) + prop.Name.Substring(1);

                    if (value is IEnumerable enumerable && !(value is string))
                    {
                        foreach (var item in enumerable)
                        {
                            if (item != null)
                            {
                                // Nếu item là DateTime, format đúng chuẩn
                                if (item is DateTime dtItem)
                                {
                                    queryParams.Add(new KeyValuePair<string, string>(
                                        paramName,
                                        dtItem.ToString("yyyy-MM-ddTHH:mm:ss")));
                                }
                                else
                                {
                                    queryParams.Add(new KeyValuePair<string, string>(
                                        paramName,
                                        item.ToString()));
                                }
                            }
                        }
                    }
                    else
                    {
                        // Xử lý riêng nếu là DateTime hoặc Nullable<DateTime>
                        if (value is DateTime dt)
                        {
                            queryParams.Add(new KeyValuePair<string, string>(
                                paramName,
                                dt.ToString("yyyy-MM-ddTHH:mm:ss")));
                        }
                        else if (value is DateTime?)
                        {
                            var nullableDt = (DateTime?)value;
                            if (nullableDt.HasValue)
                            {
                                queryParams.Add(new KeyValuePair<string, string>(
                                    paramName,
                                    nullableDt.Value.ToString("yyyy-MM-ddTHH:mm:ss")));
                            }
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
                    }
                }
            }

            return QueryHelpers.AddQueryString(baseUrl, queryParams);
        }
    }
}
