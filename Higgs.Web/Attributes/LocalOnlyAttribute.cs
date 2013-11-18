using System.Web;
using System.Web.Mvc;

namespace Higgs.Web.Attributes
{
    public class LocalOnlyAttribute : AuthorizeAttribute
    {
        public LocalOnlyAttribute()
        {
            Order = 1;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Request.IsLocal;
        }
    }
}
