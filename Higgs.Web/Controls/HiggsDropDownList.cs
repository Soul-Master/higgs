using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Higgs.Core;
using System.Linq;

namespace Higgs.Web.Controls
{
    public class HiggsDropDownList : InputControlBase
    {
        private const string UndefinedId = "ddlUndefinedId";
        public static string CssDropDownlist = "dropdownlist";
        public SelectList List { get; set; }

        internal HiggsDropDownList(HtmlHelper helper, SelectList list) : base(helper)
        {
            List = list;

            MainControl = new TagBuilder("select");
            MainControl.Attributes.Add("id", UndefinedId);
            MainControl.Attributes.Add("name", UndefinedId);
            MainControl.Attributes.Add("class", CssDropDownlist);
            MainControl.InnerHtml = string.Empty;

            if (list != null)
            {
                foreach (var item in list)
                {
                    MainControl.InnerHtml += string.Format("<option value=\"{0}\"{2}>{1}</option>", item.Value, item.Text, item.Selected ? "selected=\"true\"" : string.Empty);
                }
            }

            ListTags.Add(MainControl);
        }

        internal HiggsDropDownList(HtmlHelper helper, string id, SelectList list) : this(helper, list)
        {
            Id = id;
            MainControl.Attributes["id"] = id;
            MainControl.Attributes["name"] = id;
        }
    }

    public static class HiggsDropDownListHelper
    {
        public static HiggsDropDownList CreateDropDown(this HtmlHelper helper, SelectList list = null, string id = null)
        {
            return new HiggsDropDownList(helper, id, list);
        }

        public static HiggsDropDownList CreateDropDown(this HtmlHelper helper, Lazy<SelectList> list, string id = null)
        {
            return helper.CreateDropDown(list != null ? list.Value : null, id);
        }

        public static HiggsDropDownList CreateDropDown(this HtmlHelper helper, IValue<SelectList> list, string id = null)
        {
            return helper.CreateDropDown(list != null ? list.Value : null, id);
        }

        public static HiggsDropDownList CreateDropDown<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, object>> bindedProperty, SelectList list = null, string id = null)
        {
            return helper.CreateDropDown(list, id)
                                .BindData(helper.ViewData.Model, bindedProperty);
        }

        public static HiggsDropDownList CreateDropDown<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, object>> bindedProperty, Lazy<SelectList> list, string id = null)
        {
            return helper.CreateDropDown(bindedProperty, list != null ? list.Value : null, id);
        }

        public static HiggsDropDownList CreateDropDown<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, object>> bindedProperty, IValue<SelectList> list, string id = null)
        {
            return helper.CreateDropDown(bindedProperty, list != null ? list.Value : null, id);
        }

        public static HiggsDropDownList CreateDropDown<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, object>> bindedProperty, IEnumerable<String> list, string id = null)
        {
            var temp = new SelectList(list.Select(x => new SelectListItem {Text = x, Value = x}), "Value", "Text");
            return helper.CreateDropDown(bindedProperty, list != null ? temp : null, id);
        }
    }
}
