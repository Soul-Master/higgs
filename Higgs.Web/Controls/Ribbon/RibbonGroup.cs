using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Higgs.Core.Helpers;
using System.Linq;

namespace Higgs.Web.Controls.Ribbon
{
    public class RibbonGroup : ControlBase
    {
        public RibbonGroup(HtmlHelper helper, string title) : base(helper)
        {
            Title = title;
            Controls = new List<RibbonBaseControl>();
        }

        public string Title { get; set; }
        public List<RibbonBaseControl> Controls { get; set; }
        public override bool IsVisible
        {
            get
            {
                return Controls != null && Controls.Any(x => x.IsVisible);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Add("<div {0} class='RibbonGroup'>", !String.IsNullOrEmpty(Id) ? "id='" + Id + "'" : string.Empty);
            sb.Add("<div class='content'>");
            
            foreach(var c in Controls.Where(x => x.IsVisible))
            {
                sb.Add(c.ToString());
            }

            sb.Add("</div>");
            sb.Add("<div class='GroupName'>{0}</div>", Title);
            sb.Add("</div>");

            return sb.ToString();
        }
    }
}


