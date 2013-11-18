using System.Collections.Generic;
using System.Web.Mvc;

namespace Higgs.Web
{
    public class HiggsResult : JsonResult
    {
        // Methods
        public HiggsResult()
        {
            ErrorList = new Dictionary<string, List<string>>();
            IsComplete = true;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            Data = new { ErrorList, IsComplete, RedirectTo, ResultData };

            base.ExecuteResult(context);
        }

        // Properties
        public Dictionary<string, List<string>> ErrorList { get; set; }

        public bool IsComplete { get; set; }

        public string RedirectTo { get; set; }

        public object ResultData { get; set; }
    }
}
