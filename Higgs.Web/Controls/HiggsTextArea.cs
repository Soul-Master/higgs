using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Higgs.Web.Controls
{
    public class HiggsTextArea : InputControlBase
    {
        private const string UndefinedId = "taUndefinedId";

        internal HiggsTextArea(HtmlHelper helper) : base(helper)
        {
            MainControl = new HtmlBuilder("textarea");
            MainControl.Attributes.Add("id", UndefinedId);
            MainControl.Attributes.Add("name", UndefinedId);

            ListTags.Add(MainControl);
        }

        internal HiggsTextArea(HtmlHelper helper, string id) : this(helper)
        {
            Id = id;

            MainControl.Attributes["id"] = Id;
            MainControl.Attributes["name"] = Id;
        }
    }

    public static class HiggsTextAreaHelper
    {
        public static HiggsTextArea CreateTextArea<TModel>(this HtmlHelper<TModel> helper, string id = null)
        {
            return new HiggsTextArea(helper, id)
            {
                Helper = helper
            };
        }

        public static HiggsTextArea CreateTextArea<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, object>> bindedProperty, string id = null)
        {
            return helper.CreateTextArea(id)
                                .BindData(helper.ViewData.Model, bindedProperty);
        }
    }
}
