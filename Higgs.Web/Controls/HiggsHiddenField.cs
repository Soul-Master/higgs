using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Higgs.Web.Controls
{
    public class HiggsHiddenField : InputControlBase
    {
        private const string UndefinedId = "hUndefinedId";

        internal HiggsHiddenField(HtmlHelper helper, string id) : base(helper)
        {
            Id = id;
            IsSupportLabel = false;
            
            MainControl = new HtmlBuilder("input");
            MainControl.Attributes.Add("id", id ?? UndefinedId);
            MainControl.Attributes.Add("name", id ?? UndefinedId);
            MainControl.Attributes.Add("type", "hidden");
            
            ListTags.Add(MainControl);
        }
    }

    public static class HiggsHiddenFieldHelper
    {
        public static HiggsHiddenField CreateHiddenField(this HtmlHelper helper, string id = null)
        {
            return new HiggsHiddenField(helper, id);
        }

        public static HiggsHiddenField CreateHiddenField<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, object>> bindedProperty, string id = null)
        {
            return helper.CreateHiddenField(id)
                                .BindData(helper.ViewData.Model, bindedProperty);
        }
    }
}
