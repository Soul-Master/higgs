using System.Collections.Generic;
using System.Web.Mvc;

namespace Higgs.Web
{
    public class EmptyTempDataProvider : ITempDataProvider
    {
        // Methods
        public IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
        {
            return new Dictionary<string, object>();
        }

        public void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
        {
        }
    }
}
