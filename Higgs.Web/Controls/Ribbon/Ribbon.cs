using System.Collections.Generic;
using System.Text;
using Higgs.Core.Helpers;
using System.Web.Mvc;
using System.Linq;

namespace Higgs.Web.Controls.Ribbon
{
    public class Ribbon : ControlBase
    {
        public static string CssRibbon = "Ribbon";

        public List<RibbonTab> RibbonTabs { get; set; }
        public RibbonAppMenu RibbonAppMenu { get; set; }

        public Ribbon(HtmlHelper helper, string id) : base(helper)
        {
            Id = id;
            RibbonTabs = new List<RibbonTab>();
        }

        public void AddTab(RibbonTab tab)
        {
            RibbonTabs.Add(tab);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Add("<div id='{0}' class='{1}'>", Id, CssRibbon);
            sb.Add("<div class='header'>");

            foreach(var t in RibbonTabs.Where(x => x.IsVisible))
            {
                sb.Add(t.ToString());
            }

            if (RibbonAppMenu != null)
            {
                sb.Add(RibbonAppMenu.ToString());
            } 

            sb.Add("</div>");
            sb.Add("<div class='content'>");

            foreach (var t in RibbonTabs.Where(x => x.IsVisible && x.Groups.Any(y => y.IsVisible)))
            {
                sb.Add("<div id='{0}' class='RibbonContent'>", t.Id + "_content");
                
                foreach(var g in t.Groups.Where(x => x.IsVisible))
                {
                    sb.Add(g.ToString());
                }

                sb.Add("</div>");
            }

            sb.Add("</div>");
            sb.Add("<div class='b'></div>");
            sb.Add("</div>");

            return sb.ToString();
        }
    }
}


