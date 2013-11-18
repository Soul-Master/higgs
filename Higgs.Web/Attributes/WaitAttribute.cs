using System;
using System.Threading;
using System.Web.Mvc;

namespace Higgs.Web.Attributes
{
    public class WaitAttribute : ActionFilterAttribute
    {
        public WaitAttribute(int millisecond)
        {
            if (millisecond <= 0)
                throw new Exception(Resources.WaitingTimeMustGreaterThanZero);

            WaitingTime = millisecond;
        }

        public int WaitingTime { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Thread.Sleep(WaitingTime);
        }
    }
}


