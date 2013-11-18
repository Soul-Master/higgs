using System.Web.Mvc;

namespace Higgs.Web.Controls
{
    public static class HiggsLabelHelper
    {
        public static HiggsLabel CreateLabel(this HtmlHelper helper, string forInputId, string text)
        {
            return new HiggsLabel(helper, forInputId, text);
        }
    }
}