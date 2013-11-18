using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Higgs.Web.Helpers;

namespace Higgs.Web.Controls
{
    public class BuilderBase
    {
        public ControlBase CurrentControl { get; set; }
    }
    
    public static class BuilderBaseHelper
    {
        // TODO: Fix for this function that does not work in JqGrid control like ColumnBuilder.
        public static TBuilder IsVisible<TBuilder>(this TBuilder builder, bool isVisible = true)
            where TBuilder : BuilderBase
        {
            builder.CurrentControl.IsVisible = isVisible;

            return builder;
        }

        public static TBuilder IsVisible<TBuilder, TController>(this TBuilder builder, Expression<Func<TController, object>> routeExpression)
            where TBuilder : BuilderBase
            where TController : IController
        {
            builder.CurrentControl.IsVisible = MvcHelper.IsAuthorized(routeExpression);

            return builder;
        }
    }
}
