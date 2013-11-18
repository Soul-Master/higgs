using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Higgs.Web.Controls.JqGrid
{
    public class HiggsJqGrid : InputControlBase
    {
        private const string UndefinedId = "gUndefinedId";
        private const string PagerNamePattern = "{0}-pager";
        public HtmlBuilder GridPager;

        internal HiggsJqGrid(HtmlHelper helper, string id) : base(helper)
        {
            Id = id;
            IsSupportLabel = false;

            MainControl = new HtmlBuilder("table");
            MainControl.Attributes.Add("id", id ?? UndefinedId);
            MainControl.Attributes.Add("name", id ?? UndefinedId);

            GridPager = new HtmlBuilder("div");
            GridPager.Attributes.Add("id", string.Format(PagerNamePattern, id ?? UndefinedId));
            GridPager.Attributes.Add("name", string.Format(PagerNamePattern, id ?? UndefinedId));

            ListTags.Add(MainControl);
            ListTags.Add(GridPager);
        }
    }

    public static class HiggsJqGridHelper
    {
        public static HiggsJqGrid CreateJqGrid(this HtmlHelper helper, string id = null)
        {
            return new HiggsJqGrid(helper, id);
        }

        public static HiggsJqGrid CreateJqGrid<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, object>> bindedProperty, string id = null)
        {
            return helper.CreateJqGrid(id)
                                .BindData(helper.ViewData.Model, bindedProperty);
        }
    }
}
