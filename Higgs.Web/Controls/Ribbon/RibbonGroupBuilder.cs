namespace Higgs.Web.Controls.Ribbon
{
    public class RibbonGroupBuilder : RibbonTabBuilder
    {
        internal RibbonGroupBuilder(Ribbon r)
            : base(r)
        {
            CurrentGroup = CurrentTab.Groups[CurrentTab.Groups.Count - 1];
            CurrentControl = CurrentGroup;
        }

        public static RibbonGroupBuilder CreateGroupBuilder(Ribbon r)
        {
            return new RibbonGroupBuilder(r);
        }

        internal RibbonGroup CurrentGroup { get; set; }
    }

    public static class RibbonGroupBuilderHelper
    {
        public static RibbonGroupBuilder AddControl(this RibbonGroupBuilder builder, RibbonBaseControl control)
        {
            control.Helper = builder.CurrentRibbon.Helper;
            builder.CurrentGroup.Controls.Add(control);

            return builder;
        }
    }
}


