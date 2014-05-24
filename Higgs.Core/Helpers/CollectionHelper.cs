using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Higgs.Core.Helpers
{
    public static class CollectionHelper
    {
        public static T[] AddRange<T>(this T[] arr, IEnumerable<T> items)
        {
            if (arr == null || items == null) return arr;

            var temp = new List<T>(arr);
            temp.AddRange(items);

            return temp.ToArray();
        }

        public static T[] Add<T>(this T[] arr, params T[] items)
        {
            return arr.AddRange(items);
        }

        public static List<T> Clone<T>(this List<T> listToClone) 
            where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static List<KeyValuePair<string, string>> ToList(this NameValueCollection items)
        {
            if(items == null) return null;

            var list = new List<KeyValuePair<string, string>>();

            for(var i = 0; i < items.Count; i++)
            {
                list.Add(new KeyValuePair<string, string>(items.Keys[i],items[i]));
            }

            return list;
        }
    }
}
