using System.Web.Mvc;

namespace Higgs.Web.Controls.Ribbon
{
    public class RibbonBaseButton : RibbonBaseControl, IClickableControl
    {
        public RibbonBaseButton(HtmlHelper helper) : base(helper)
        {
            
        }

        public string Title { get; set; }
        public string OnClickJavaScript { get; set; }
    }
}


