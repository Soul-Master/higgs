namespace Higgs.Web.Controls.Ribbon
{
    public class RibbonTabBuilder : RibbonBuilder
    {
        internal RibbonTabBuilder(Ribbon r) : base(r)
        {
            CurrentTab = r.RibbonTabs[r.RibbonTabs.Count - 1];
            CurrentControl = CurrentTab;
        }

        internal RibbonTab CurrentTab { get; set; }

        internal static RibbonTabBuilder CreateRibbonTabBuilder(Ribbon r)
        {
            return new RibbonTabBuilder(r);
        }
    }

    public static class RibbonTabBuilderHelper
    {
        public static RibbonTabBuilder Active(this RibbonTabBuilder builder)
        {
            builder.CurrentTab.IsActive = true;

            return builder;
        }

        public static RibbonGroupBuilder Group(this RibbonTabBuilder builder, string title, string id = null)
        {
            var g = new RibbonGroup(builder.CurrentRibbon.Helper, title)
            {
                Id =id
            };

            builder.CurrentTab.Groups.Add(g);

            return RibbonGroupBuilder.CreateGroupBuilder(builder.CurrentRibbon);
        }
    }
}


