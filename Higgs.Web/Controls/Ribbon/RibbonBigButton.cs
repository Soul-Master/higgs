using System.Web.Mvc;
using Higgs.Web.Helpers;

namespace Higgs.Web.Controls.Ribbon
{
    public class RibbonBigButton : RibbonBaseButton
    {
        public RibbonBigButton(HtmlHelper helper) : base(helper)
        {
            
        }

        public string IconPath { get; set; }

        public override string ToString()
        {
            return
                string.Format
                    (
                        "<div {3} class='BigButton' {2}>" +
                        "<img src='{0}' />" +
                        "<span>{1}</span>" +
                        "</div>",
                        Helper.ResolveUrl(IconPath),
                        Title,
                        !string.IsNullOrWhiteSpace(OnClickJavaScript) ? "onclick='" + OnClickJavaScript + "'" : string.Empty,
                        !string.IsNullOrEmpty(Id) ? "id='" + Id + "'" : string.Empty
                    );
        }
    }

    public static class RibbonBigButtonHelper
    {
        public static RibbonButtonBuilder BigButton(this RibbonGroupBuilder builder, string title, string iconPath, string id = null)
        {
            var btt = new RibbonBigButton(builder.CurrentGroup.Helper)
              {
                  Title = title,
                  IconPath = iconPath,
                  Id = id
              };

            builder.AddControl(btt);

            return new RibbonButtonBuilder(builder.CurrentRibbon);
        }
    }
}


