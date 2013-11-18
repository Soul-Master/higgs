using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;
using Higgs.Core.Helpers;
using Higgs.Web.Helpers;

namespace Higgs.Web.Controls
{
    public class HiggsInput : InputControlBase
    {
        private const string UndefinedId = "txtUndefinedId";
        public static string CssTextbox = "textbox";
        public static string CssPassword = "password";
        public static string CssTextboxMenuButton = "textbox-menu-button";

        public static string PWaterMarkId = "{0}_waterMark";
        public static string PMenuButtonId = "{0}_menuButton";
        
        internal HiggsInput(HtmlHelper helper, HtmlInputType type) : base(helper)
        {
            MainControl = new HtmlBuilder("input");
            MainControl.Attributes.Add("id", UndefinedId);
            MainControl.Attributes.Add("name", UndefinedId);
            MainControl.Attributes.Add("type", Enum.GetName(typeof(HtmlInputType), type));
            MainControl.Attributes.Add("class", CssTextbox);

            if (type == HtmlInputType.password)
            {
                MainControl.Attributes["class"] += " " + CssPassword;
            }

            ListTags.Add(MainControl);
        }

        internal HiggsInput(HtmlHelper helper, string id, HtmlInputType type) : this(helper, type)
        {
            Id = id;

            MainControl.Attributes["id"] = Id;
            MainControl.Attributes["name"] = Id;
        }

        public HiggsInput WaterMark(string markText = null)
        {
            if (markText == null) markText = Name;
            var tagIndex = !IsLabelBeforeControl ? 0 : (HasLabel ? 1 : 0);

            if (ListTags[tagIndex].Attributes["type"] == HtmlInputType.password.ToString())
            {
                var control = new HtmlBuilder("input");
                control.Attributes.Add("id", PWaterMarkId.Eval(Id));
                control.Attributes.Add("name", PWaterMarkId.Eval(Id));
                control.Attributes.Add("type", HtmlInputType.text.ToString());
                control.Attributes.Add("class", CssTextbox + " " + CssPassword + " " + CssWaterMark);

                ListTags.Add(control);

                return this.WaterMark(markText, PWaterMarkId.Eval(Id));
            }

            return this.WaterMark(markText, null);
        }

        public HiggsInput BindMenu(PopUpMenu pm)
        {
            AddCustomButton(PMenuButtonId.Eval(Id), DropDownIconPath, "", "dropdown-button", pm.Id);

            if (!HiggsScriptManager.HasScriptGroup("_" + pm.Id))
                pm.PrepareScript(this);       

            return this;
        }

        public HiggsInput AddCustomButton(string buttonId, string buttonUrl, string buttonAlt, string buttonCssClass = "custom-dropdown-button", string menuId = null)
        {
            var link = new HtmlBuilder("a");
            link.Attributes.Add("id", buttonId);
            link.Attributes.Add("href", "javascript:void(0);");
            link.Attributes.Add("title", buttonAlt);
            link.Attributes.Add("class", CssTextboxMenuButton);
            if (this.HasClass(CssReadonly))
            {
                link.AddCssClass(CssReadonly);
            }

            var linkIcon = new HtmlBuilder("img");
            linkIcon.Attributes.Add("src", Helper.ResolveUrl(buttonUrl));
            linkIcon.Attributes.Add("alt", buttonAlt);

            if (buttonCssClass != null)
            {
                linkIcon.AddCssClass(buttonCssClass);
            }

            link.ChildControls.Add(linkIcon);

            ListTags.Add(link);
            AddCssClass(CssHasMenu);

            HiggsScriptManager.AddjQueryObjectScript(Id, Id, ".bindMenu({0}, '#{1}')", menuId ?? "null", buttonId);   

            return this;
        }
    }

    public static class HiggsTextBoxHelper
    {
        public static HiggsInput CreateCustomInput(this HtmlHelper helper, HtmlInputType type, string id = null)
        {
            return new HiggsInput(helper, id, type);
        }

        public static HiggsInput CreateTextBox(this HtmlHelper helper, string id = null)
        {
            return helper.CreateCustomInput(HtmlInputType.text, id);
        }

        public static HiggsInput CreateTextBox<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, object>> bindedProperty, string id = null)
        {
            return helper.CreateTextBox(id)
                                .BindData(helper.ViewData.Model, bindedProperty);
        }

        public static HiggsInput CreatePassword(this HtmlHelper helper, string id = null)
        {
            return helper.CreateCustomInput(HtmlInputType.password, id);
        }

        public static HiggsInput CreatePassword<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, object>> bindedProperty, string id = null)
        {
            return helper.CreatePassword(id)
                                .BindData(helper.ViewData.Model, bindedProperty);
        }

        public static HiggsInput AutoCompleteList<TController>(this HiggsInput txt, Expression<Func<TController, object>> routeExpression, string outputControl = null)
            where TController : IController
        {
            var path = routeExpression.GetLogicalPath();

            HiggsScriptManager.AddjQueryObjectScript
            (
                txt.Id,
                txt.Id,
                ".autocompleteSelectList('{0}', {1})",
                path,
                outputControl ?? "null"
            );

            return txt;
        }

        public static HiggsInput AutoCompleteList(this HiggsInput txt, string getUrlFn, string outputControl = null, string onItemSelectedFn = null)
        {
            HiggsScriptManager.AddjQueryObjectScript
            (
                txt.Id,
                txt.Id,
                ".autocompleteSelectList({0}, {1}, {2})",
                getUrlFn,
                outputControl ?? "null",
                onItemSelectedFn ?? "null"
            );

            return txt;
        }

        public static HiggsInput SetValue(this HiggsInput txt, string value)
        {
            txt.MainControl.Attributes["value"] = value;

            return txt;
        }

        public static HiggsInput MaxLength(this HiggsInput txt, int maxLength)
        {
            txt.MainControl.Attributes["maxlength"] = maxLength.ToString(CultureInfo.InvariantCulture);

            return txt;
        }
    }
    
// ReSharper disable InconsistentNaming
    public enum HtmlInputType
    {
        text, password, 
        search, url, tel, email,
        datetime, date, month, week, time, datetimelocal,
        number, color
    };

    public class AutoCompleteModel
    {
        public string value { get; set; }
        public string label { get; set; }
    }
// ReSharper restore InconsistentNaming
}
