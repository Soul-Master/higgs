using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Higgs.Web
{
    /// <summary>
    /// Fix "Internet explorer was not able to open the Internet site. The requested site is either unavailable or can not be found.Please try again later."
    /// When use FileStreamResult as ActionResult and open with IE8.
    /// </summary>
    public class StreamResult : FileStreamResult
    {
        public StreamResult(Stream fileStream, string contentType) : base(fileStream, contentType) {}

        public override void ExecuteResult(ControllerContext context)
        {
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Cache.SetCacheability(HttpCacheability.Private);
            response.CacheControl = "private";

            base.ExecuteResult(context);
        }
    }
}
