using System.Web.Mvc;

namespace Higgs.Web
{
    public interface IStaticFileController
    {
        ActionResult DefaultView(string path);
    }
}