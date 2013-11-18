using System.Web.Mvc;
using Higgs.Web.Helpers;

namespace Higgs.Web.Controls.Ribbon
{
    public class RibbonAppMenu : ControlBase
    {
        public RibbonAppMenu(HtmlHelper helper, string id, string title, string iconUrl = null) : base(helper)
        {
            Id = id;
            Title = title;
            IconUrl = iconUrl;
        }

        public string Title { get; set; }
        public string IconUrl { get; set; }
        public PopUpMenu Menu { get; set; }

        public override string ToString()
        {
            if(Menu != null)
            {
                Menu.PrepareScript(Menu);

                HiggsScriptManager.AddScript
                (
                    "_" + Menu.Id,
                    string.Format
                    (
                        "$('#{0}')" +
                            ".bindMenu('#{0}', {1} );",
                        Id,
                        Menu.Id
                    )
                );
            }

            return
                string.Format
                    (
                        "<div id='{0}' class='RibbonAppMenu'>" +
                            "<div class='content'><span {1}>{2}</span><div class='dropDownIcon'></div></div>" +
                        "</div>",
                        Id,
                        !string.IsNullOrWhiteSpace(IconUrl) ?  string.Format("style='background:url({0}) no-repeat;' class='LabelWithIcon'", Helper.ResolveUrl(IconUrl)) : string.Empty,
                        Title
                    );
        }
    }
}


