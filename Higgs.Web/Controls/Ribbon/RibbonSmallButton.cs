using System.Web.Mvc;
using Higgs.Web.Helpers;

namespace Higgs.Web.Controls.Ribbon
{
    public class RibbonSmallButton : RibbonBaseButton
    {
        public RibbonSmallButton(HtmlHelper helper) : base(helper)
        {
            
        }

        public string IconPath { get; set; }

        public override string ToString()
        {
            return
                string.Format
                    (
                        "<div class='SmallButton' {2}>" +
                        "<img src='{0}' />" +
                        "<span>{1}</span>" +
                        "</div>",
                        Helper.ResolveUrl(IconPath),
                        Title,
                        !string.IsNullOrWhiteSpace(OnClickJavaScript) ? "onclick='" + OnClickJavaScript + "'" : string.Empty
                    );
        }
    }

    public static class RibbonSmallButtonHelper
    {
        public static RibbonButtonBuilder AddSmallButton(this RibbonGroupBuilder builder, string title, string iconPath)
        {
            var btt = new RibbonSmallButton(builder.CurrentGroup.Helper)
              {
                  Title = title,
                  IconPath = iconPath
              };
            
            builder.AddControl(btt);
            return new RibbonButtonBuilder(builder.CurrentRibbon);
        }
    }
}


