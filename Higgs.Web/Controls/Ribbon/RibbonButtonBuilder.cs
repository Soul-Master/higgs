using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Higgs.Web.Helpers;

namespace Higgs.Web.Controls.Ribbon
{
    public class RibbonButtonBuilder : RibbonGroupBuilder
    {
        internal RibbonButtonBuilder(Ribbon r)
            : base(r)
        {
            CurrentButton = CurrentGroup.Controls[CurrentGroup.Controls.Count - 1] as RibbonBaseButton;
            CurrentControl = CurrentButton;
        }

        internal RibbonBaseButton CurrentButton { get; set; }
    }

    public static class RibbonButtonBuilderHelper
    {
        public static RibbonButtonBuilder IsVisible<TController>(this RibbonButtonBuilder builder, Expression<Func<TController, object>> routeExpression)
            where TController : IController
        {
            return builder.IsVisible<RibbonButtonBuilder, TController>(routeExpression);
        }

        public static RibbonButtonBuilder OnClick(this RibbonButtonBuilder builder, string javaScript)
        {
            builder.CurrentButton.OnClickJavaScript = javaScript;

            return builder;
        }

        /// <summary>
        /// This method should be used when you want to bind action width current button and create complex JavaScript at the same time.
        /// 
        /// "{0}" in JavaScript script will be replace will action url.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="exp"></param>
        /// <param name="javaScript"></param>
        /// <returns></returns>
        public static RibbonButtonBuilder OnClick<T>(this RibbonButtonBuilder builder, Expression<Func<T, object>> exp, string javaScript)
            where T : IController
        {
            builder.CurrentButton.Visible(exp);
            builder.CurrentButton.OnClickJavaScript = string.Format(javaScript, exp.GetLogicalPath());

            return builder;
        }

        public static RibbonButtonBuilder OnClickOpenDialog<T>(this RibbonButtonBuilder builder, Expression<Func<T, object>> exp, string dialogIcon = null, string dialogTitle = null, bool isModal = false, string onCloseCallbackFn = null)
            where T : IController
        {
            if(dialogTitle == null)
            {
                dialogTitle = builder.CurrentButton.Title;
            }

            builder.CurrentButton.Visible(exp);
            builder.CurrentButton.OnClickJavaScript = string.Format("openDialog(\"{0}\", \"{1}\", {2}, \"{3}\", {4});", exp.GetLogicalPath(), dialogTitle, isModal.ToString().ToLower(), dialogIcon ?? "", onCloseCallbackFn ?? "null");

            return builder;
        }

        public static RibbonButtonBuilder OnClickOpenUrl<T>(this RibbonButtonBuilder builder, Expression<Func<T, object>> exp)
            where T : IController
        {
            builder.CurrentButton.Visible(exp);
            builder.CurrentButton.OnClickJavaScript = string.Format("openUrl(\"{0}\", this);", exp.GetLogicalPath());

            return builder;
        }

        public static RibbonButtonBuilder OnClickOpenUrl(this RibbonButtonBuilder builder, string url)
        {
            builder.CurrentButton.OnClickJavaScript = string.Format("openUrl(\"{0}\", this);", url);

            return builder;
        }
    }
}


