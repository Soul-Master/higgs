using System.Collections.Generic;
using System.Web.Mvc;

namespace Higgs.Web
{
    public static class HtmlHelperExtension
    {
        private static Dictionary<uint, bool> GetTabIndexDic(HtmlHelper helper)
        {
            Dictionary<uint, bool> tabIndexDic;

            if(helper.ViewData.ContainsKey("__Higgs.TabIndex"))
            {
                tabIndexDic = (Dictionary<uint, bool>)helper.ViewData["__Higgs.TabIndex"];
            }
            else
            {
                helper.ViewData["__Higgs.TabIndex"] = tabIndexDic = new Dictionary<uint, bool>();
            }

            return tabIndexDic;
        }

        public static uint GetNextAvailableTabIndex(this HtmlHelper helper)
        {
            var tabIndexDic = GetTabIndexDic(helper);

            for(uint i = 1; i < 1000; i++)
            {
                if(tabIndexDic.ContainsKey(i)) continue;

                tabIndexDic.Add(i, true);
                return i;
            }

            return 1000;
        }

        public static MvcHtmlString GenerateScript(this HtmlHelper helper, int runningScriptDelay = 0)
        {
            return HiggsScriptManager.Create(runningScriptDelay);
        }
    }
}
