using System;

namespace Higgs.Web.Attributes
{
    /// <summary>
    /// Create custom Url mapping by designing your Url pattern to be routed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true, Inherited=true)]
    public class MapRouteAttribute : Attribute
    {
        public MapRouteAttribute(){}

        public MapRouteAttribute(string routeUrl)
        {
            RouteUrl = routeUrl;
        }

        public string RouteUrl { get; set; }
    }
}


