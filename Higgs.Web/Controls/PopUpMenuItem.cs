using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using Higgs.Web.Helpers;

namespace Higgs.Web.Controls
{
    public class PopUpMenuItem : ControlBase
    {
        public PopUpMenuItem(HtmlHelper helper, string id, string label, string iconPath = null) : this(helper)
        {
            Id = id;
            Label = label;
            IconUrl = iconPath;
            IsSepMenu = false;
        }

        public PopUpMenuItem(HtmlHelper helper) : base(helper)
        {
            IsSepMenu = true;
        }

        public bool IsSepMenu { get; set; }
        public bool IsCheckBox { get; set; }
        public string GroupName { get;set; }
        public bool IsSelected { get; set; }
        public string IconUrl { get; set; }
        public string Label { get; set; }
        public string CustomHtml { get; set; }
        public string FnOnClick { get; set; }

        public void PrepareScript(PopUpMenu pm, StringBuilder script)
        {
            if (IsSepMenu)
            {
                script.AppendFormat("{0}.addMenu(popupMenuItem(", pm.Id);
                
                if(!string.IsNullOrWhiteSpace(Id))
                {
                    script.AppendFormat("'{0}'", Id);
                }
                script.Append(")");
            }
            else if (!string.IsNullOrWhiteSpace(CustomHtml))
            {
                script.AppendFormat("{0}.addMenu(popupMenuItem('{1}')", pm.Id, CustomHtml);
            }
            else
            {
                script.AppendFormat("{0}.addMenu(popupMenuItem(", pm.Id);

                if (!string.IsNullOrWhiteSpace(Id))
                    script.AppendFormat("'{0}'", Id);
                else
                    script.Append("null");

                script.AppendFormat(", '{0}'", Label);

                if (!string.IsNullOrWhiteSpace(IconUrl))
                    script.AppendFormat(", '{0}'", IconUrl);
                else
                    script.Append(", null");

                if (IsCheckBox)
                    script.AppendFormat(", {0}", IsSelected.ToString().ToLower());
                else if (!string.IsNullOrWhiteSpace(GroupName))
                {
                    script.AppendFormat(", '{0}'", GroupName);

                    if (IsSelected)
                        script.Append(", true");
                }

                script.Append(")");
            }

            if(!string.IsNullOrEmpty(FnOnClick))
            {
                script.AppendFormat(".click({0})", FnOnClick);
            }

            script.AppendLine(");");
        }
    }

    public static class PopUpMenuItemHelper
    {
        public static PopUpMenu AddMenuItem(this PopUpMenu pm, string id, string label, string iconUrl = null, bool isCheckBox = false, string groupName = null, bool isSelected = false, string customHtml = null, string fnOnClick = null)
        {
            var mi = new PopUpMenuItem(pm.Helper, id, label, iconUrl)
             {
                 IconUrl = iconUrl,
                 IsCheckBox = isCheckBox,
                 GroupName = groupName,
                 IsSelected = isSelected,
                 CustomHtml = customHtml,
                 FnOnClick = fnOnClick
             };

            pm.Items.Add(mi);

            return pm;
        }

        public static PopUpMenu AddMenuItem(this PopUpMenu pm)
        {
            var mi = new PopUpMenuItem(pm.Helper);
            pm.Items.Add(mi);
            
            return pm;
        }

        public static PopUpMenu AddMenuItem(this PopUpMenu pm, IEnumerable<string> itemList, string fnOnClick = null)
        {
            foreach (var item in itemList)
            {
                pm.AddMenuItem(pm.Id + (pm.Items.Count + 1), item, fnOnClick: fnOnClick);
            }

            return pm;
        }
        
        public static PopUpMenu OnClickOpenDialog<T>(this PopUpMenu menu, Expression<Func<T, object>> exp, string dialogTitle, string dialogIcon = null, bool isModal = false, string onCloseCallbackFn = null)
            where T : IController
        {
            if (menu.Items.Count == 0) throw new Exception("Menu must contain atleast one menu item.");
 
            var currentItem = menu.Items[menu.Items.Count - 1];

            currentItem.Visible(exp);
            currentItem.FnOnClick = "function(){" + string.Format("openDialog(\"{0}\", \"{1}\", {2}, \"{3}\", {4});", exp.GetLogicalPath(), dialogTitle, isModal.ToString().ToLower(), dialogIcon ?? "", onCloseCallbackFn ?? "null") + "}";

            return menu;
        }

        public static PopUpMenu OnClickOpenUrl<T>(this PopUpMenu menu, Expression<Func<T, object>> exp)
            where T : IController
        {
            if (menu.Items.Count == 0) throw new Exception("Menu must contain atleast one menu item.");
 
            var currentItem = menu.Items[menu.Items.Count - 1];

            currentItem.Visible(exp);
            currentItem.FnOnClick = "function(){" + string.Format("openUrl(\"{0}\", this);", exp.GetLogicalPath()) + "}";

            return menu;
        }

        public static PopUpMenu OnClickOpenUrl(this PopUpMenu menu, string url)
        {
            if (menu.Items.Count == 0) throw new Exception("Menu must contain atleast one menu item.");
 
            var currentItem = menu.Items[menu.Items.Count - 1];

            currentItem.FnOnClick = "function(){" + string.Format("openUrl(\"{0}\", this);", url) + "}";

            return menu;
        }
    }
}
