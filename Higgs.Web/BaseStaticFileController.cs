using System.Web.Mvc;

namespace Higgs.Web
{
    public class BaseStaticFileController : BaseController, IStaticFileController
    {
        public virtual ActionResult DefaultView(string path)
        {
            return View(path);
        }
    }
}
