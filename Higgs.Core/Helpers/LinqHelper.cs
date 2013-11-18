using System;
using System.Collections.Generic;

namespace Higgs.Core.Helpers
{
    public static class LinqHelper
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) return;

            foreach (var item in source)
            {
                action(item);
            }
        }

        public static bool Any<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool runAll)
        {
            var result = false;
            if (source == null) return false;

            foreach (var item in source)
            {
                result |= predicate(item);

                if (!runAll && result) break;
            }

            return result;
        }
    }
}
