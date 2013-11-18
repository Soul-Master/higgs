using System;
using System.Linq;
using System.Web.Mvc;

namespace Higgs.Web
{
    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            TempDataProvider = new EmptyTempDataProvider();
        }

        protected ViewResult View(object model, ViewHierarchy siteMap)
        {
            var result = View(model);
            result.ViewData["HiggsViewPage-SiteMap"] = siteMap;

            return result;
        }

        protected HiggsResult Result
        {
            get
            {
                if (!ModelState.IsValid)
                {
                    var result2 = new HiggsResult
                    {
                        IsComplete = false
                    };

                    var result = result2;
                    Func<string, bool> predicate = key => ModelState[key].Errors.Count != 0;
                    foreach (var str in ModelState.Keys.Where(predicate))
                    {
                        result.ErrorList.Add
                        (
                            str, 
                            (
                                from error in ModelState[str].Errors 
                                select error.ErrorMessage
                            ).ToList<string>()
                        );
                    }

                    return result;
                }

                return new HiggsResult { IsComplete = true };
            }
        }
    }
}
