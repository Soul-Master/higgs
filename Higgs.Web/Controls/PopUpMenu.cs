using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Higgs.Core.Helpers;

namespace Higgs.Web.Controls
{
    public class PopUpMenu : ControlBase
    {
        public PopUpMenu(HtmlHelper helper, string id, int minWidth = 0) : base(helper)
        {
            Id = id;
            MinWidth = minWidth;
            Items = new List<PopUpMenuItem>();
        }

        public int MinWidth { get; set; }
        public List<PopUpMenuItem> Items { get; set; }
        
        private void Create(string targetControlId)
        {
            var script = new StringBuilder();
            script.AppendFormat("{0} = window.{0} = popupMenu('{0}'", Id);

            if (MinWidth > 0)
                script.AppendFormat(", {0}", MinWidth);
            script.Add(");");

            foreach (var mi in Items)
            {
                mi.PrepareScript(this, script);
            }
            script.Add();

            HiggsScriptManager.InsertScript("_" + targetControlId, "_" + Id, script.ToString());
        }

        public void PrepareScript(ControlBase parentControl)
        {
            Create(parentControl.Id);
        }

        public IHtmlString PrepareScript(string targetControlId)
        {
            Create(targetControlId);
            HiggsScriptManager.InsertScript("_" + targetControlId, "_" + Id, String.Format("$('#{0}').bindMenu({1})", targetControlId, Id));   

            return HiggsScriptManager.Create();
        }
    }

    public static class PopUpMenuHelper
    {
        public static PopUpMenu CreatePopUpMenu(this HtmlHelper helper, string id, int minWidth = 0)
        {
            var pm = new PopUpMenu(helper, id, minWidth);

            return pm;
        }
    }
}
