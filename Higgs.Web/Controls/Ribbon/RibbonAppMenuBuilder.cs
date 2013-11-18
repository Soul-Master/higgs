namespace Higgs.Web.Controls.Ribbon
{
    public class RibbonAppMenuBuilder : RibbonBuilder
    {
        internal RibbonAppMenuBuilder(Ribbon r) : base(r)
        {

        }

        internal static RibbonAppMenuBuilder CreateRibbonAppMenuBuilder(Ribbon r)
        {
            return new RibbonAppMenuBuilder(r);
        }
    }

    public static class RibbonAppMenuBuilderHelper
    {
        public static RibbonAppMenuBuilder Menu(this RibbonAppMenuBuilder b, PopUpMenu p)
        {
            b.CurrentRibbon.RibbonAppMenu.Menu = p;

            return b;
        }
    }
}


