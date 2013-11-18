using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using Higgs.Web.Helpers;

namespace Higgs.Web.Controls
{
    public class HiggsForm : ControlBase
    {
        public HiggsForm(HtmlHelper helper, string actionUrl) : base(helper)
        {
            Helper = helper;
            ActionUrl = actionUrl;
        }

        public string ActionUrl { get; set; }
        public string OnSuccessFn { get; set; }
        public string OnFailFn { get; set; }
        public bool IsAjaxForm { get; set; }
    }

    public class HiggsForm<T> : HiggsForm
    {
        internal HiggsForm(T model, HtmlHelper helper, string actionUrl) : base(helper, actionUrl)
        {
            Model = model;
        }

        public T Model { get; set; }
        public Dictionary<string, Action<StringBuilder, object>> ModelValueProvider { get; set; }

        public HiggsForm<T> CustomModelValueProvider(Dictionary<string, Action<StringBuilder, object>> valueProvider)
        {
            ModelValueProvider = valueProvider;

            return this;
        }

        public override string ToString()
        {
            if (IsAjaxForm)
            {
                HiggsScriptManager.AddjQueryObjectScript
                (
                    Id,
                    Id,
                    ".ajaxForm(${0}, {1}, {2})",
                    typeof (T).Name,
                    string.IsNullOrEmpty(OnSuccessFn) ? "null" : OnSuccessFn,
                    string.IsNullOrEmpty(OnFailFn) ? "null" : OnFailFn
                );
            }

            if (HiggsScriptManager.SerializedType == null)
                HiggsScriptManager.SerializedType = new List<Type>();

            // TODO: Rebuild model Serializer
            /*
            if (!HiggsScriptManager.SerializedType.Contains(modelType))
            {
                HiggsScriptManager.InsertScript(null, "__" + modelType, ModelSerializer.SerializeModel(Model, null, ModelValueProvider));
                HiggsScriptManager.SerializedType.Add(typeof(T));
            }
            */

            var tag = new TagBuilder("form");
            tag.Attributes["id"] = Id;
            tag.Attributes["action"] = ActionUrl;
            tag.Attributes["method"] = "post";

            return tag.ToString(TagRenderMode.StartTag);
        }
    }

    public static class HiggsFormHelper
    {
        public static HiggsForm<TModel> CreateHiggsForm<TModel>(this HtmlHelper helper, TModel model, bool isAjaxForm = true)
        {
            var routeData = helper.ViewContext.RouteData;
            var controller = routeData.GetRequiredString("controller");
            var action = routeData.GetRequiredString("action");

            var form = new HiggsForm<TModel>(model, helper, UrlHelper.GenerateUrl(null, action, controller, null, helper.RouteCollection, helper.ViewContext.RequestContext, true))
           {
               Id = string.Format("form{0}{1}_{2}", controller, action, typeof(TModel).Name),
               IsAjaxForm = isAjaxForm
           };

            form.Disabled(!MvcHelper.IsAuthorized(controller, action) || helper.IsReadOnly());
            
            return form;
        }

        public static HiggsForm<TModel> CreateHiggsForm<TModel>(this HtmlHelper<TModel> helper, bool isAjaxForm = true)
        {
            return helper.CreateHiggsForm(helper.ViewData.Model, isAjaxForm);
        }

        public static HiggsForm<TModel> OnCallback<TModel>(this HiggsForm<TModel> form, string fn)
        {
            form.OnSuccessFn = fn;
            form.OnFailFn = fn;

            return form;
        }

        public static HiggsForm<TModel> OnSuccess<TModel>(this HiggsForm<TModel> form, string fn)
        {
            form.OnSuccessFn = fn;

            return form;
        }

        public static HiggsForm<TModel> OnFail<TModel>(this HiggsForm<TModel> form, string fn)
        {
            form.OnFailFn = fn;

            return form;
        }

        public static HiggsForm<TModel> ActionUrl<TController, TModel>(this HiggsForm<TModel> form, Expression<Func<TController, object>> exp)
            where TController : IController
        {
            string controllerName, actionName;
            
            form.ActionUrl = form.Helper.ResolveUrl(exp.GetLogicalPath(out controllerName, out actionName));
            form.Id = string.Format("form{0}{1}_{2}", controllerName, actionName, typeof(TModel).Name);
            form.Disabled(!MvcHelper.IsAuthorized(controllerName, actionName));

            return form;
        }

        public static HiggsForm<TModel> ReadOnly<TModel>(this HiggsForm<TModel> form, bool isReadOnly = true)
        {
            if (!isReadOnly) return form;

            Action<string> addScript = id => HiggsScriptManager.AddjQueryObjectScript(form, ".readOnly()");

            if (string.IsNullOrEmpty(form.Id))
            {
                form.OnIdChange += new ControlChangeIdEventHandler(addScript);
            }
            else
            {
                addScript(form.Id);
            }

            return form;
        }

        public static HiggsForm<TModel> Disabled<TModel>(this HiggsForm<TModel> form, bool isReadOnly = true)
        {
            if (!isReadOnly) return form;

            Action<string> addScript = id => HiggsScriptManager.AddjQueryObjectScript(form, ".disableForm()");

            if (string.IsNullOrEmpty(form.Id))
            {
                form.OnIdChange += new ControlChangeIdEventHandler(addScript);
            }
            else
            {
                addScript(form.Id);
            }

            return form;
        }

        public static MvcHtmlString EndHiggsForm<T>(this HtmlHelper<T> helper, int runningScriptDelay = 100)
        {
            var html = HiggsScriptManager.Create(runningScriptDelay);
            
            return new MvcHtmlString(html.ToHtmlString() + "</form>");
        }
    }
}
