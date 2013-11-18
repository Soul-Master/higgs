using System;
using System.Web.Mvc;
using System.Web.UI;

namespace Higgs.Web.Attributes
{
    public class CachedResult : OutputCacheAttribute
    {
        public CachedResult(int duration = int.MaxValue, OutputCacheLocation location = OutputCacheLocation.Server)
        {
            Duration = duration;
            VaryByParam = "none";
            Location = location;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Headers.Add("Cache-Date", string.Format("{0:r}", DateTime.Now.ToUniversalTime()));

            base.OnResultExecuting(filterContext);
        }
    }
}
