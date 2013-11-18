using System.Web.Mvc;

namespace Higgs.Web.Controls.Ribbon
{
    public class RibbonBuilder : BuilderBase
    {
        internal RibbonBuilder(Ribbon r)
        {
            CurrentRibbon = r;
            CurrentControl = CurrentRibbon;
        }

        internal static RibbonBuilder CreateRibbonBuilder(Ribbon r)
        {
            return new RibbonBuilder(r);
        }

        internal Ribbon CurrentRibbon { get; set; }

        public override string ToString()
        {
            return CurrentRibbon.ToString();
        }
    }

    public static class RibbonBuilderHelper
    {
        public static RibbonBuilder CreateRibbon(this HtmlHelper helper, string id)
        {
            var r = new Ribbon(helper, id)
              {
                  Helper = helper
              };

            return RibbonBuilder.CreateRibbonBuilder(r);
        }

        public static RibbonTabBuilder Tab(this RibbonBuilder builder, string id, string title, string iconUrl = null)
        {
            var t = new RibbonTab(builder.CurrentRibbon.Helper, id, title, iconUrl);
            
            builder.CurrentRibbon.AddTab(t);

            return RibbonTabBuilder.CreateRibbonTabBuilder(builder.CurrentRibbon);
        }

        public static RibbonAppMenuBuilder AppMenu(this RibbonBuilder builder, string id, string title, string iconUrl = null)
        {
            var am = new RibbonAppMenu(builder.CurrentRibbon.Helper, id, title, iconUrl);

            builder.CurrentRibbon.RibbonAppMenu = am;

            return RibbonAppMenuBuilder.CreateRibbonAppMenuBuilder(builder.CurrentRibbon);   
        }
    }
}


