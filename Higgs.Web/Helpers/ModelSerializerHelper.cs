using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Higgs.Core.Helpers;

namespace Higgs.Web.Helpers
{
    public static class ModelSerializerHelper
    {
        public static Action<StringBuilder, object> ToArrayValueSerializer<T>(this Func<T, string> jsonValueFn)
        {
            Action<StringBuilder, object> action = (sb, obj) =>
           {
               var data = obj as IEnumerable<T>;

               if (data == null)
               {
                   sb.Add("[]");
                   return;
               }

               var collection = data.ToList();

               if (collection.Count > 1)
                {
                    sb.Add();
                    sb.Add("\t[");
                }
                else
                {
                    sb.Append("[");
                }

               for (var i = 0; i < collection.Count; i++)
                {
                    if (collection.Count > 1)
                    {
                        sb.Append("\t\t");
                    }
                    sb.Append("{");

                    var item = collection[i];
                    sb.Append(jsonValueFn(item));

                    if (collection.Count == 1)
                    {
                        sb.Append("}");
                    }
                    else
                    {
                        sb.Add(i == collection.Count - 1 ? "}" : "}, ");
                    }
                }

               sb.Append(collection.Count > 1 ? "\t]" : "]");
           };

            return action;
        }
    }
}
