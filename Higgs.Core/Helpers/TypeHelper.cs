using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;

namespace Higgs.Core.Helpers
{
    public static class TypeHelper
    {
        public static void InitializeInstance(this object obj)
        {
            // Initialize Field Value
            foreach (var fi in obj.GetType().GetFields())
            {
                foreach (var dv in
                    fi.GetCustomAttributes(true).OfType<DefaultValueAttribute>().Select(attr => attr))
                {
                    fi.SetValue(obj, dv.Value);
                }
            }

            // Initialize Property Value
            foreach (var pi in obj.GetType().GetProperties())
            {
                var pi1 = pi;

                foreach (var dv in from attr in pi.GetCustomAttributes(true)
                                   where (attr is DefaultValueAttribute) && pi1.CanWrite
                                   select attr as DefaultValueAttribute)
                {
                    pi.SetValue(obj, dv.Value, null);
                }
            }
        }

        public static TAttribute GetCustomAttribute<TAttribute>(this PropertyInfo pi, object obj)
        {
            return pi.GetCustomAttribute<TAttribute>(obj, true);
        }

        public static TAttribute GetCustomAttribute<TAttribute>(this PropertyInfo pi, object obj, bool inherit)
        {
            return GetCustomAttribute<TAttribute>(obj.GetType(), pi.Name, inherit);
        }

        public static List<TAttribute> GetAllCustomAttributes<TAttribute>(Type t, string propertyName, bool allowInheritAttribute)
        {
            var tempProperties = t.GetProperties();
            var result = new List<TAttribute>();
            var hasMatchedProperty = false;
            var allProperties = tempProperties.Where(pi => pi.Name == propertyName).ToList();

            foreach (var pi in allProperties)
            {
                hasMatchedProperty = true;
                var temp = pi.GetCustomAttributes(typeof(TAttribute), allowInheritAttribute) as TAttribute[];

                if (temp != null && temp.Length > 0)
                        result.AddRange(temp);
            }

            if (hasMatchedProperty)
            {
                if (t.BaseType != typeof(object) && t.BaseType != null)
                {
                    IEnumerable<TAttribute> temp = GetAllCustomAttributes<TAttribute>(t.BaseType, propertyName, allowInheritAttribute);

                    if (temp != null)
                        result.AddRange(temp);
                }

                var allInterfaces = t.GetInterfaces();

                foreach (var temp in
                    allInterfaces.Select(tInterface => GetAllCustomAttributes<TAttribute>(tInterface, propertyName, allowInheritAttribute)).Where(temp => temp != null))
                {
                    result.AddRange(temp);
                }
            }

            return result;
        }

        public static TAttribute GetCustomAttribute<TAttribute>(Type t, string propertyName, bool allowInheritAttribute)
        {
            var result = GetAllCustomAttributes<TAttribute>(t, propertyName, allowInheritAttribute);

            if (result != null && result.Count > 0)
            {
                return result[0];
            }

            return default(TAttribute);
        }
    }
}


