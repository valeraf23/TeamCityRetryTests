using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamCityRetryTests.Helpers
{
    internal static class Extensions
    {
        public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            foreach (var (key, value) in collection.Where(item => !source.ContainsKey(item.Key)))
            {
                source.Add(key, value);
            }
        }

        public static Dictionary<string, string> ToDictionary(this IEnumerable<string> source) =>
            source.Select(a => a.Split(new[] {'='}, 2))
                .GroupBy(a => a[0], a => a.Length == 2 ? a[1] : string.Empty)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault());
    }
}