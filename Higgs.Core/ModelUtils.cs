using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Higgs.Core.Helpers;

namespace Higgs.Core
{
    public static class ModelUtils
    {
        public static List<string> GetSelectedProperties<T>(Expression<Func<T, object>> selectedProperties)
        {
            var propertyNames = new List<string>();
            var ne = selectedProperties.Body as NewExpression;

            if (ne == null)
                throw new InvalidOperationException("Object constructor expected");

            foreach (var me in ne.Arguments.Select(arg => arg as MemberExpression))
            {
                if (me == null || me.Expression != selectedProperties.Parameters[0])
                    throw new InvalidOperationException("Object constructor argument should be a direct member");

                propertyNames.Add(me.Member.Name);
            }

            return propertyNames;
        }

        /// <summary>
        /// Mostly use to convert parent-class instance to child-class instance. This method will copy only .NET typed object like System.*.
        /// However, we can force this method to copy other complex type by allow property name.
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <typeparam name="TDerived"></typeparam>
        /// <param name="model"></param>
        /// <param name="allowPropertyName"></param>
        /// <returns></returns>
        public static TDerived CastAs<TDerived, TBase>(this TBase model, List<string> allowPropertyName = null)
            where TDerived : TBase
        {
            var baseType = typeof(TBase);
            var derivedType = typeof(TDerived);
            if (allowPropertyName == null)
            {
                allowPropertyName = new List<string>();
            }

            var derivedObj = Activator.CreateInstance<TDerived>();

            foreach (var pi in baseType.GetProperties())
            {
                if (!pi.CanRead || (!pi.PropertyType.FullName.StartsWith("System.") && !allowPropertyName.Contains(pi.Name))) continue;

                var derivedProperty = derivedType.GetProperty(pi.Name);

                if (derivedProperty.CanWrite)
                {
                    derivedProperty.SetValue(derivedObj, pi.GetValue(model, null), null);
                }
            }

            return derivedObj;
        }

        public static List<string> DefaultChangedPropertyList = new List<string>();
        public static List<string> DefaultIgnoredPropertyList = new List<string> { "Id", "ID" };
        public static List<string> IgnoreTypeNameList = new List<string>()
        {
            "System.Collection.*"
        };

        public static int UpdateModel(this object oldData, object changedData, IEnumerable changedProperties = null, IEnumerable ignoredProperties = null)
        {
            // TODO: Reflector meanless variable names.
            var changedPropDic = DefaultChangedPropertyList != null ? DefaultChangedPropertyList.ToDictionary(x => x, x => true) : new Dictionary<string, bool>();
            var ignoredPropDic = DefaultIgnoredPropertyList != null ? DefaultIgnoredPropertyList.ToDictionary(x => x, x => true)  : new Dictionary<string, bool>();
            var count = 0;

            if (changedProperties != null)
            {
                foreach (var p in changedProperties)
                {
                    changedPropDic[p.ToString()] = true;
                }
            }
            if (ignoredProperties != null)
            {
                foreach (var p in ignoredProperties)
                {
                    ignoredPropDic[p.ToString()] = true;
                }
            }

            var oldType = oldData.GetType();
            var changedType = changedData.GetType();
            var changedPropertyDic = changedType.GetProperties().ToDictionary(x => x.Name);
            var hasChanged = changedPropertyDic.Count > 0;
            var hasIgnored = ignoredPropDic.Count > 0;

            foreach (var pOld in oldType.GetProperties())
            {
                var pName = pOld.Name;
                var typeFullName = pOld.PropertyType.FullName;

                if (!pOld.CanWrite) continue;
                if (hasChanged && !changedPropertyDic.ContainsKey(pName)) continue;
                if (!changedPropertyDic.ContainsKey(pName)) continue;
                if (hasIgnored && ignoredPropDic.ContainsKey(pName)) continue;
                if (hasChanged && !changedPropDic.ContainsKey(pName)) continue;

                var changedProp = changedPropertyDic[pName];
                if (!changedProp.CanRead) continue;
                if (IgnoreTypeNameList != null)
                {
                    if (IgnoreTypeNameList.Any(x =>
                        {
                            if (x.EndsWith(".*"))
                            {
                                if (typeFullName.StartsWith(x.Substring(0, x.Length - 2))) return true;
                            }
                            else if(typeFullName == x)
                            {
                                return true;
                            }

                            return false;
                        }))
                    {
                        continue;
                    }
                }

                var newValue = changedProp.GetValue(changedData, null);
                pOld.SetValue(oldData, newValue, null);

                count++;
            }

            return count;
        }
    }
}


