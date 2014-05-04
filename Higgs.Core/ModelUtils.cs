using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        public static int UpdateModel(this object oldData, object changedData, IEnumerable changedProperties = null)
        {
            var changedPropertyList = new List<string>();
            var count = 0;

            if (changedProperties != null)
            {
                changedPropertyList.AddRange(from object propertyName in changedProperties
                                             select propertyName.ToString());
            }

            var oldType = oldData.GetType();
            var changedType = changedData.GetType();

            foreach (var pi in changedType.GetProperties()
                .Where
                (
                    x => x.Name.ToUpper() != "ID" &&
                         x.CanRead &&
                         x.CanWrite &&
                         (changedProperties == null || changedPropertyList.Contains(x.Name)) &&
                        x.PropertyType.FullName.StartsWith("System") &&
                        !x.PropertyType.FullName.StartsWith("System.Collection") &&
                        !x.PropertyType.FullName.StartsWith("System.Data.Entity.DynamicProxies")
                ))
            {
                var newValue = pi.GetValue(changedData, null);
                var oldProp = oldType.GetProperty(pi.Name);

                if (oldProp == null || !oldProp.CanWrite) continue;

                oldProp.SetValue(oldData, newValue, null);
                count++;
            }

            return count;
        }
    }
}


