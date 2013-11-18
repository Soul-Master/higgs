using System;
using System.Linq;
using System.Web.Mvc;

namespace Higgs.Web.Controls
{
    public static class ControlHelper
    {
        public static bool HasClass(this TagBuilder tag, params string[] className)
        {
            if(tag.Attributes.ContainsKey("class"))
            {
                var currentClasses = tag.Attributes["class"].Split(' ');

                return className.All(c => Array.IndexOf(currentClasses, c) >= 0);
            }

            return false;
        }

        public static bool RemoveClass(this TagBuilder tag, params string[] className)
        {
            if (tag.Attributes.ContainsKey("class"))
            {
                var currentClasses = tag.Attributes["class"].Split(' ');
                var newCssClass = currentClasses.Where(c => Array.IndexOf(className, c) < 0).Aggregate(string.Empty, (current, c) => current + (" " + c));

                if(string.IsNullOrEmpty(newCssClass))
                {
                    tag.Attributes.Remove("class");
                }
                else
                {
                    tag.Attributes["class"] = newCssClass.Substring(1);
                }

                return className.All(c => Array.IndexOf(currentClasses, c) >= 0);
            }

            return false;
        }
    }
}
