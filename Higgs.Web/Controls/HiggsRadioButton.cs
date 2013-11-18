using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Higgs.Web.Controls
{
    public class HiggsRadioButton : InputControlBase
    {
        private const string UndefinedId = "rUndefinedId";
        public static string CssRadioButtonGroup = "radioGroup";
        public static string CssRadioButton = "radio";
        public static string CssRadioButtonLabel = "radioLabel";
        public SelectList List { get; set; }
        public override string ScriptControlId
        {
            get
            {
                return string.Format("#{0} :radio", Id);
            }
        }

        internal HiggsRadioButton(HtmlHelper helper, SelectList list) : base(helper)
        {
            List = list;

            MainControl = new TagBuilder("span");
            MainControl.Attributes.Add("id", UndefinedId);
            MainControl.Attributes.Add("name", UndefinedId);
            MainControl.Attributes.Add("class", CssRadioButtonGroup);
            MainControl.InnerHtml = string.Empty;
        }

        internal HiggsRadioButton(HtmlHelper helper, string id, SelectList list)
            : this(helper, list)
        {
            Id = id;
            MainControl.Attributes["id"] = id;
            MainControl.Attributes["name"] = id;

            ListTags.Add(MainControl);
        }

        protected void CreateRadioItems()
        {
            if (List == null) return;

            var index = int.Parse(MainControl.Attributes["tabindex"] ?? "1");
            var i = 1;
            foreach (var item in List)
            {
                MainControl.InnerHtml += string.Format("<input type=\"radio\" id=\"{0}\" name=\"{1}\" class=\"{2}\" tabindex=\"{5}\" value=\"{3}\" {4}> ", Id + "_" + i, Id, CssRadioButton, item.Value, item.Selected ? "checked=\"on\"" : string.Empty, index++);
                MainControl.InnerHtml += string.Format("<label for=\"{0}\" class=\"{1}\">{2}</label> ", Id + "_" + i, CssRadioButtonLabel, item.Text);

                i++;
            }

            if (MainControl.Attributes.ContainsKey("tabindex"))
            {
                MainControl.Attributes.Remove("tabindex");
            }
        }

        public override string ToString()
        {
            CreateRadioItems();

            return base.ToString();
        }
    }

    public static class HiggsRadioButtonHelper
    {
        public static HiggsRadioButton CreateRadioButton(this HtmlHelper helper, SelectList list)
        {
            return new HiggsRadioButton(helper, list);
        }

        // TODO: Fix bug that does not create radio button
        public static HiggsRadioButton CreateRadioButton(this HtmlHelper helper, SelectList list, string id)
        {
            return new HiggsRadioButton(helper, id, list);
        }

        public static HiggsRadioButton CreateRadioButton<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, object>> bindedProperty, SelectList list, string id = null)
        {
            return helper.CreateRadioButton(list, id)
                                .BindData(helper.ViewData.Model, bindedProperty);
        }
    }
}
