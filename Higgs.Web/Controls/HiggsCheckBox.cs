using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Higgs.Web.Controls
{
    public class HiggsCheckBox : InputControlBase
    {
        public static string CssCheckBox = "checkbox";

        internal HiggsCheckBox(HtmlHelper helper, string id) : base(helper)
        {
            Id = id;

            MainControl = new HtmlBuilder("input");
            MainControl.Attributes.Add("id", id);
            MainControl.Attributes.Add("name", id);
            MainControl.Attributes.Add("class", CssCheckBox);
            MainControl.Attributes.Add("type", "checkbox");

            ListTags.Add(MainControl);

            IsLabelBeforeControl = false;
        }
    }

    public static class HiggsCheckBoxHelper
    {
        public static HiggsCheckBox CreateCheckBox(this HtmlHelper helper, string id = null)
        {
            return new HiggsCheckBox(helper, id)
            {
                Helper = helper
            };
        }

        public static HiggsCheckBox CreateCheckBox<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, object>> bindedProperty, string id = null)
        {
            return helper.CreateCheckBox(id)
                                .BindData(helper.ViewData.Model, bindedProperty);
        }
    }
}
