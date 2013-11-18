using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Higgs.Web.Helpers;

namespace Higgs.Web.Controls
{
    public class HiggsButton : ControlBase
    {
        internal HiggsButton(HtmlHelper helper, string id, HtmlButtonType type) : base(helper)
        {
            Id = id;

            var control = new HtmlBuilder("button");
            if (!string.IsNullOrEmpty(id))
            {
                control.Attributes.Add("id", id);
                control.Attributes.Add("name", id);
            }
            control.Attributes.Add("type", "button");
            control.AddCssClass(type.ToString().ToLower());
            
            MainControl = control;
            ListTags.Add(control);
        }
        
        internal HtmlBuilder MainControl { get; set; }
    }

    public enum HtmlButtonType
    {
        Button,
        Submit,
        Reset
    }

    public static class HiggsButtonHelper
    {
        public static HiggsButton CreateButton(this HtmlHelper helper)
        {
            return helper.CreateButton(HtmlButtonType.Button);
        }

        public static HiggsButton CreateSubmitButton(this HtmlHelper helper, string id = null)
        {
            return helper.CreateButton(HtmlButtonType.Submit, id);
        }

        public static HiggsButton CreateResetButton(this HtmlHelper helper, string id = null)
        {
            return helper.CreateButton(HtmlButtonType.Reset, id);
        }

        public static HiggsButton CreateButton(this HtmlHelper helper, HtmlButtonType type, string id = null)
        {
            var button = new HiggsButton(helper, id, type)
            {
                Helper = helper
            };

            return button;
        }

        public static HiggsButton SetText(this HiggsButton button, string text)
        {
            var span = button.MainControl.ChildControls.SingleOrDefault(x => x.TagName == "span");

            if(span == null)
            {
                span = new HtmlBuilder("span");
                button.MainControl.ChildControls.Insert(button.MainControl.ChildControls.Count, span);
            }

            span.SetInnerText(text);

            return button;
        }

        public static HiggsButton SetIcon(this HiggsButton button, string iconPath)
        {
            var span = button.MainControl.ChildControls.SingleOrDefault(x => x.TagName == "span");

            if (span == null)
            {
                button.SetText(string.Empty);

                span = button.MainControl.ChildControls.SingleOrDefault(x => x.TagName == "span");
            }

            span.SetCss
            (
                new
                {
                    background = string.Format("url({0}) no-repeat left center", button.Helper.ResolveUrl(iconPath)),
                    paddingLeft = "22px"
                }
            );
            
            return button;
        }

        public static HiggsButton OnClick(this HiggsButton button, string script, bool condition = true)
        {
            if (condition)
            {
                button.MainControl.Attributes["onclick"] = script;
            }

            return button;
        }

        public static HiggsButton OnClick<T>(this HiggsButton button, Expression<Func<T, object>> exp, bool condition = true, string confirmMessage = "")
            where T : IController
        {
            if (condition)
            {
                if (MvcHelper.IsAuthorized(exp))
                {
                    var openUrl = string.Format("openUrl('{0}');", exp.GetLogicalPath());
                    var openUrlWithConfirm = string.Format("confirm('{0}') && {1}", confirmMessage, openUrl);

                    button.MainControl.Attributes["onclick"] = string.IsNullOrWhiteSpace(confirmMessage)
                                                                   ? openUrl
                                                                   : openUrlWithConfirm;
                }
                else
                {
                    button.Visible(false);
                }
            }

            return button;
        }

        public static HiggsButton Disabled(this HiggsButton input, bool flag = true)
        {
            if (flag)
            {
                input.MainControl.Attributes["disabled"] = "true";
            }

            return input;
        }
    }
}
