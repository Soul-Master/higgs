using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Higgs.Web.Helpers;

namespace Higgs.Web.Controls.Ribbon
{
    public class RibbonTab : ControlBase
    {
        public RibbonTab(HtmlHelper helper, string id, string title)
            : this(helper, id, title, null)
        {

        }

        public RibbonTab(HtmlHelper helper, string id, string title, string iconUrl) : base(helper)
        {
            Id = id;
            Title = title;
            IconUrl = iconUrl;
            Groups = new List<RibbonGroup>();
        }

        public string Title { get; set; }
        public string IconUrl { get; set; }
        public List<RibbonGroup> Groups { get; set; }
        public bool IsActive { get; set; }

        public override string ToString()
        {
            if (!Groups.Any(x => x.IsVisible)) return "";

            return
                string.Format
                    (
                        "<div id='{0}' class='RibbonTab {4}' ribbonContentId='{1}'>" +
                        "<div class='content'><span {2}>{3}</span></div>" +
                        "</div>",
                        Id,
                        Id + "_content",
                        !string.IsNullOrWhiteSpace(IconUrl) ?  string.Format("style='background:url({0}) no-repeat;' class='LabelWithIcon'", Helper.ResolveUrl(IconUrl)) : string.Empty,
                        Title,
                        IsActive ? "_active" : string.Empty
                    );
        }
    }
}


