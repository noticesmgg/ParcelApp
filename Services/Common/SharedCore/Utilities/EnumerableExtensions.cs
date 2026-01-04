using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCore.Utilities
{
    public static class EnumerableExtensions
    {
        public static T? GetKeyByValue<T>(this Dictionary<T, string> dictionary, string value)
        {
            foreach (var kvp in dictionary)
            {
                if (kvp.Value.EqualsIgnoreCase(value))
                {
                    return kvp.Key;
                }
            }

            return default(T);
        }


        /// <summary>
        /// Add if not exists in the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static void AddIfNotExists<T>(this List<T> lst, T data)
        {
            if (!lst.Contains(data))
                lst.Add(data);
        }

        /// <summary>
        /// Add if not exists in the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="dataList"></param>
        public static void AddIfNotExists<T>(this List<T> lst, List<T> dataList)
        {
            if (dataList.Any())
            {
                foreach (var data in dataList)
                {
                    if (!lst.Contains(data))
                        lst.Add(data);
                }
            }

        }

        public static string Combine<T>(this IEnumerable<T> source, string separator = ",")
        {
            if (source == null)
            {
                return string.Empty;
            }

            return string.Join(separator, source);
        }
    }
}
